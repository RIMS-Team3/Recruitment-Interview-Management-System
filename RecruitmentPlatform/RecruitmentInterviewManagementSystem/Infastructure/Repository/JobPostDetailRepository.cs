using Microsoft.EntityFrameworkCore;
using RecruitmentInterviewManagementSystem.Domain.InterfacesRepository;
using RecruitmentInterviewManagementSystem.Models;

namespace RecruitmentInterviewManagementSystem.Infastructure.Repository
{
    public class JobPostDetailRepository : IJobPostDetailRepository
    {
        private readonly FakeTopcvContext _context;

        public JobPostDetailRepository(FakeTopcvContext context)
        {
            _context = context;
        }

        public async Task<JobPostDetailDTO?> GetJobPostDetailAsync(Guid jobId)
        {
            var job = await _context.JobPosts
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == jobId);

            if (job == null)
                return null;

            return new JobPostDetailDTO
            {
                Id = job.Id,
                Title = job.Title,
                Description = job.Description,
                Requirement = job.Requirement,
                Benefit = job.Benefit,
                SalaryMin = job.SalaryMin,
                SalaryMax = job.SalaryMax,
                Location = job.Location,
                JobType = job.JobType,
                ExpireAt = job.ExpireAt,
                IsActive = job.IsActive,
                ViewCount = job.ViewCount,
                CreatedAt = job.CreatedAt,
                Experience = job.Experience,

                Company = new CompanyDTO
                {
                    Id = job.Company.Id,
                    Name = job.Company.Name,
                    Address = job.Company.Address,
                    Website = job.Company.Website,
                    LogoUrl = job.Company.LogoUrl,
                    Description = job.Company.Description
                }
            };
        }
    }
}