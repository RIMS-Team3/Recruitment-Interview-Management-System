using Microsoft.EntityFrameworkCore;
using RecruitmentInterviewManagementSystem.Applications.Features.JobPost.DTO;
using RecruitmentInterviewManagementSystem.Domain.Entities;
using RecruitmentInterviewManagementSystem.Domain.InterfacesRepository;
using RecruitmentInterviewManagementSystem.Models;
using System;

namespace RecruitmentInterviewManagementSystem.Infastructure.Repository
{
    public class JobPostRepository : IJobPostRepository
    {
        private readonly FakeTopcvContext _context;

        public JobPostRepository(FakeTopcvContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobPost>> GetAllAsync()
        {
            // Lấy danh sách Job đang hoạt động (IsActive = true)
            return await _context.JobPosts
                                 .Where(x => x.IsActive == true)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<JobPost>> GetFilteredJobsAsync(JobPostFilterRequest filter)
        {
            // Tạo truy vấn cơ bản
            var query = _context.JobPosts.AsQueryable();

            // 1. Luôn ưu tiên lấy các Job đang hoạt động
            query = query.Where(x => x.IsActive == true);

            // 2. MỚI: Lọc theo ID (Nếu có ID thì chỉ lấy 1 bản ghi đó)
            if (filter.Id.HasValue)
            {
                query = query.Where(x => x.Id == filter.Id.Value);
            }

            // 3. Lọc theo từ khóa (Search) - Tìm trong tiêu đề
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                string searchLower = filter.Search.ToLower();
                query = query.Where(x => x.Title != null && x.Title.ToLower().Contains(searchLower));
            }

            // 4. Lọc theo địa điểm
            if (!string.IsNullOrWhiteSpace(filter.Location))
            {
                query = query.Where(x => x.Location != null && x.Location.Contains(filter.Location));
            }

            // 5. SỬA: Lọc theo khoảng lương
            // Lấy các Job có mức lương khởi điểm cao hơn hoặc bằng MinSalary người dùng nhập
            if (filter.MinSalary.HasValue)
            {
                query = query.Where(x => x.SalaryMin >= filter.MinSalary.Value);
            }

            // Lấy các Job có mức lương trần thấp hơn hoặc bằng MaxSalary người dùng nhập
            if (filter.MaxSalary.HasValue)
            {
                query = query.Where(x => x.SalaryMax <= filter.MaxSalary.Value);
            }

            // 6. Lọc theo loại công việc
            if (filter.JobType.HasValue)
            {
                query = query.Where(x => x.JobType == filter.JobType.Value);
            }

            // Trả về danh sách mới nhất lên đầu
            return await query.OrderByDescending(x => x.CreatedAt).ToListAsync();
        }

    }
}
