using Microsoft.EntityFrameworkCore;
using RecruitmentInterviewManagementSystem.Applications.Features.Schedule;
using RecruitmentInterviewManagementSystem.Models;

namespace RecruitmentInterviewManagementSystem.Infastructure.ServiceImplement
{
    public class ViewListScheduleForCandidate : IViewListScheduleForCandidate
    {
        private readonly FakeTopcvContext _db;

        public ViewListScheduleForCandidate(FakeTopcvContext fakeTopcvContext)
        {
            _db = fakeTopcvContext;
        }

        public async Task<List<ScheduleForCandidateDto>> Execute(string Token)
        {
            // 1. Tìm Token, Include thêm Application, Job và Company để lấy thông tin
            var interviewToken = await _db.InterviewBookingTokens
                .Include(t => t.Application)
                    .ThenInclude(a => a.Job)
                        .ThenInclude(j => j.Company)
                .FirstOrDefaultAsync(x => x.Token == Token);

            // 2. Kiểm tra tính hợp lệ của Token (Tồn tại, chưa dùng, chưa hết hạn)
            if (interviewToken == null || interviewToken.IsUsed || interviewToken.ExpiredAt < DateTime.UtcNow)
            {
                // Khuyên dùng: Trả về một List rỗng thay vì null để Frontend dễ xử lý vòng lặp hơn
                return new List<ScheduleForCandidateDto>();
            }

            // 3. Trích xuất các thông tin cần thiết
            var companyId = interviewToken.Application.Job.CompanyId;
            var jobTitle = interviewToken.Application.Job.Title;
            var companyName = interviewToken.Application.Job.Company.Name;
            var location = interviewToken.Application.Job.Location ?? interviewToken.Application.Job.Company.Address;

            // 4. Tìm các Slot phỏng vấn hợp lệ và gán vào DTO
            var availableSlots = await _db.InterviewsSlots
                .Where(slot => slot.IdCompany == companyId
                            && slot.IsBooked == false
                            && slot.StartTime > DateTime.UtcNow)
                .OrderBy(slot => slot.StartTime)
                .Select(slot => new ScheduleForCandidateDto
                {
                    Id = slot.IdInterviewSlot, 
                    Title = $"Phỏng vấn: {jobTitle}",
                    StartTime = slot.StartTime,
                    EndTime = slot.EndTime,
                    Location = location,
                    Description = $"Lịch phỏng vấn vị trí {jobTitle} tại {companyName}"
                })
                .ToListAsync();

            return availableSlots;
        }
    }
}