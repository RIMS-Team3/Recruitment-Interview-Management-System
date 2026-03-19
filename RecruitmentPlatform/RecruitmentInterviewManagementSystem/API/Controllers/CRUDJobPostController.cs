using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentInterviewManagementSystem.API.DTOs;
using RecruitmentInterviewManagementSystem.DTOs;
using RecruitmentInterviewManagementSystem.Infastructure.Models; // Đã cập nhật namespace theo folder file của bạn
using RecruitmentInterviewManagementSystem.Models;
using System.Security.Claims;

namespace RecruitmentInterviewManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CRUDJobPostController : ControllerBase
{
    private readonly FakeTopcvContext _context;

    public CRUDJobPostController(FakeTopcvContext context)
    {
        _context = context;
    }

    // CREATE JOB
    [HttpPost("create")]
    public async Task<IActionResult> CreateJob(CRUDCreateJobPostRequest request)
    {
        // 1. Validate logic
        if (request.SalaryMin.HasValue && request.SalaryMax.HasValue && request.SalaryMin > request.SalaryMax)
            return BadRequest("Lương tối thiểu không được lớn hơn lương tối đa.");

        if (request.ExpireAt.HasValue && request.ExpireAt <= DateTime.UtcNow)
            return BadRequest("Ngày hết hạn phải là một ngày trong tương lai.");

        if (request.Experience < 0)
            return BadRequest("Kinh nghiệm không thể là số âm.");

        // 2. Lấy thông tin chủ sở hữu
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized("Không tìm thấy UserId trong token");

        var userId = Guid.Parse(userIdClaim.Value);
        var employer = await _context.EmployerProfiles.FirstOrDefaultAsync(e => e.Id == userId);
        if (employer == null) return BadRequest("Hồ sơ nhà tuyển dụng không tồn tại.");

        // 3. Tạo Job
        var job = new JobPost
        {
            Id = Guid.NewGuid(),
            CompanyId = employer.CompanyId,
            Title = request.Title,
            Description = request.Description,
            Requirement = request.Requirement,
            Benefit = request.Benefit,
            SalaryMin = request.SalaryMin,
            SalaryMax = request.SalaryMax,
            Location = request.Location,
            JobType = request.JobType,
            ExpireAt = request.ExpireAt,
            Experience = request.Experience, 
            IsActive = true,
            ViewCount = 0,
            CreatedAt = DateTime.UtcNow
        };

        _context.JobPosts.Add(job);
        await _context.SaveChangesAsync();

        return Ok(job);
    }

    // UPDATE JOB
    [HttpPut("update")]
    public async Task<IActionResult> UpdateJob(CRUDUpdateJobPostRequest request)
    {
        // 1. Validate logic
        if (request.SalaryMin.HasValue && request.SalaryMax.HasValue && request.SalaryMin > request.SalaryMax)
            return BadRequest("Lương tối thiểu không được lớn hơn lương tối đa.");

        if (request.Experience < 0)
            return BadRequest("Kinh nghiệm không thể là số âm.");

        // 2. Kiểm tra quyền sở hữu (Security check)
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized();
        var userId = Guid.Parse(userIdClaim.Value);

        var employer = await _context.EmployerProfiles.FirstOrDefaultAsync(e => e.Id == userId);
        if (employer == null) return BadRequest("Employer profile not found");

        // Tìm Job thuộc đúng Công ty của người đang đăng nhập
        var job = await _context.JobPosts
            .FirstOrDefaultAsync(j => j.Id == request.JobId && j.CompanyId == employer.CompanyId);

        if (job == null)
            return NotFound("Không tìm thấy tin tuyển dụng hoặc bạn không có quyền chỉnh sửa.");

        // 3. Cập nhật
        job.Title = request.Title;
        job.Description = request.Description;
        job.Requirement = request.Requirement;
        job.Benefit = request.Benefit;
        job.SalaryMin = request.SalaryMin;
        job.SalaryMax = request.SalaryMax;
        job.Location = request.Location;
        job.JobType = request.JobType;
        job.ExpireAt = request.ExpireAt;
        job.Experience = request.Experience; // Cập nhật kinh nghiệm

        await _context.SaveChangesAsync();
        return Ok(job);
    }

    // READ ALL JOB OF COMPANY
    [HttpGet("my-jobs")]
    public async Task<IActionResult> GetMyJobs()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized();

        var userId = Guid.Parse(userIdClaim.Value);
        var employer = await _context.EmployerProfiles.FirstOrDefaultAsync(e => e.Id == userId);
        if (employer == null) return Ok(new List<JobPost>());

        var jobs = await _context.JobPosts
            .Where(j => j.CompanyId == employer.CompanyId)
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync();

        return Ok(jobs);
    }

    // DELETE JOB
    [HttpDelete("{jobId}")]
    public async Task<IActionResult> DeleteJob(Guid jobId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        var userId = Guid.Parse(userIdClaim!.Value);
        var employer = await _context.EmployerProfiles.FirstOrDefaultAsync(e => e.Id == userId);

        // Chỉ cho phép xóa nếu Job thuộc công ty mình
        var job = await _context.JobPosts
            .FirstOrDefaultAsync(j => j.Id == jobId && j.CompanyId == employer!.CompanyId);

        if (job == null)
            return NotFound("Không có quyền xóa hoặc tin không tồn tại.");

        _context.JobPosts.Remove(job);
        await _context.SaveChangesAsync();

        return Ok("Đã xóa tin tuyển dụng thành công.");
    }
}