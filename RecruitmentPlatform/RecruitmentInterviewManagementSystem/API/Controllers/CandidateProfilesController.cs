using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecruitmentInterviewManagementSystem.Applications.Features.Auth.DTO;
using RecruitmentInterviewManagementSystem.Models;
using Microsoft.EntityFrameworkCore;


namespace RecruitmentInterviewManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateProfilesController : ControllerBase
    {
        private readonly FakeTopcvContext _db; 

        public CandidateProfilesController(FakeTopcvContext  context)
        {
            _db = context;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<CandidateProfileDto>> GetProfile(Guid userId)
        {
            var profile = await _db.CandidateProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) return NotFound("Không tìm thấy hồ sơ ứng viên.");

            return Ok(new CandidateProfileDto
            {
                Id = profile.Id,
                UserId = profile.UserId,
                DateOfBirth = profile.DateOfBirth,
                Gender = profile.Gender,
                Address = profile.Address,
                ExperienceYears = profile.ExperienceYears,
                CurrentSalary = profile.CurrentSalary,
                DesiredSalary = profile.DesiredSalary,
                JobLevel = profile.JobLevel,
                Summary = profile.Summary
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile(Guid id, CandidateProfileDto dto)
        {
            if (id != dto.Id) return BadRequest("Lỗi dữ liệu.");

            var profile = await _db.CandidateProfiles.FindAsync(id);
            if (profile == null) return NotFound("Không tìm thấy hồ sơ.");

            profile.DateOfBirth = dto.DateOfBirth;
            profile.Gender = dto.Gender;
            profile.Address = dto.Address;
            profile.ExperienceYears = dto.ExperienceYears;
            profile.CurrentSalary = dto.CurrentSalary;
            profile.DesiredSalary = dto.DesiredSalary;
            profile.JobLevel = dto.JobLevel;
            profile.Summary = dto.Summary;

            await _db.SaveChangesAsync();
            return Ok(new { message = "Cập nhật hồ sơ thành công!" });
        }
    }
}