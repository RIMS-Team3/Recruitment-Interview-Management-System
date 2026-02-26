using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentInterviewManagementSystem.Applications.Features.Auth.DTO;
using RecruitmentInterviewManagementSystem.Models;
namespace RecruitmentInterviewManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerProfilesController : ControllerBase
    {
        private readonly FakeTopcvContext _db;

        public EmployerProfilesController(FakeTopcvContext context)
        {
            _db = context;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<EmployerProfileDto>> GetProfile(Guid userId)
        {
            var profile = await _db.EmployerProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) return NotFound("Không tìm thấy hồ sơ nhà tuyển dụng.");

            return Ok(new EmployerProfileDto
            {
                Id = profile.Id,
                UserId = profile.UserId,
                CompanyId = profile.CompanyId,
                Position = profile.Position
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile(Guid id, EmployerProfileDto dto)
        {
            if (id != dto.Id) return BadRequest("Lỗi dữ liệu.");

            var profile = await _db.EmployerProfiles.FindAsync(id);
            if (profile == null) return NotFound("Không tìm thấy hồ sơ.");

            profile.Position = dto.Position;

            await _db.SaveChangesAsync();
            return Ok(new { message = "Cập nhật chức vụ thành công!" });
        }
    }
}
