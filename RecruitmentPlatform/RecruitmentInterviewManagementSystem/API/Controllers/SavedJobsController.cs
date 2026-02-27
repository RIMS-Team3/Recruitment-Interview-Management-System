using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentInterviewManagementSystem.Applications.Features.SavedJobs.DTO;
using RecruitmentInterviewManagementSystem.Models;

namespace RecruitmentInterviewManagementSystem.API.Controllers;

[Route("api/saved-jobs")]
[ApiController]
public class SavedJobsController : ControllerBase
{
    private readonly FakeTopcvContext _context;

    public SavedJobsController(FakeTopcvContext context)
    {
        _context = context;
    }

    [HttpGet("{candidateId:guid}")]
    public async Task<ActionResult<IEnumerable<SavedJobItemDto>>> GetSavedJobs(Guid candidateId)
    {
        var items = await _context.SavedJobs
            .AsNoTracking()
            .Where(x => x.CandidateId == candidateId)
            .OrderByDescending(x => x.SavedAt)
            .Select(x => new SavedJobItemDto
            {
                JobId = x.JobId,
                Title = x.Job.Title,
                Location = x.Job.Location,
                SalaryMin = x.Job.SalaryMin,
                SalaryMax = x.Job.SalaryMax,
                SavedAt = x.SavedAt
            })
            .ToListAsync();

        return Ok(items);
    }

    [HttpGet("{candidateId:guid}/ids")]
    public async Task<ActionResult<IEnumerable<Guid>>> GetSavedJobIds(Guid candidateId)
    {
        var jobIds = await _context.SavedJobs
            .AsNoTracking()
            .Where(x => x.CandidateId == candidateId)
            .Select(x => x.JobId)
            .ToListAsync();

        return Ok(jobIds);
    }

    [HttpPost]
    public async Task<IActionResult> SaveJob([FromBody] SaveJobRequest request)
    {
        var validationResult = await ValidateRequestAsync(request);
        if (validationResult is not null)
        {
            return validationResult;
        }

        var alreadySaved = await _context.SavedJobs
            .AnyAsync(x => x.CandidateId == request.CandidateId && x.JobId == request.JobId);

        if (alreadySaved)
        {
            return Ok(new { saved = true, message = "Job đã được lưu trước đó." });
        }

        _context.SavedJobs.Add(new SavedJob
        {
            CandidateId = request.CandidateId,
            JobId = request.JobId,
            SavedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
        return Ok(new { saved = true, message = "Lưu job thành công." });
    }

    [HttpDelete]
    public async Task<IActionResult> UnsaveJob([FromBody] SaveJobRequest request)
    {
        if (request.CandidateId == Guid.Empty || request.JobId == Guid.Empty)
        {
            return BadRequest(new { message = "CandidateId và JobId là bắt buộc." });
        }

        var savedJob = await _context.SavedJobs
            .FirstOrDefaultAsync(x => x.CandidateId == request.CandidateId && x.JobId == request.JobId);

        if (savedJob is null)
        {
            return NotFound(new { saved = false, message = "Không tìm thấy job đã lưu." });
        }

        _context.SavedJobs.Remove(savedJob);
        await _context.SaveChangesAsync();

        return Ok(new { saved = false, message = "Đã bỏ lưu job." });
    }

    [HttpPost("toggle")]
    public async Task<IActionResult> ToggleSavedJob([FromBody] SaveJobRequest request)
    {
        var validationResult = await ValidateRequestAsync(request);
        if (validationResult is not null)
        {
            return validationResult;
        }

        var savedJob = await _context.SavedJobs
            .FirstOrDefaultAsync(x => x.CandidateId == request.CandidateId && x.JobId == request.JobId);

        if (savedJob is null)
        {
            _context.SavedJobs.Add(new SavedJob
            {
                CandidateId = request.CandidateId,
                JobId = request.JobId,
                SavedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return Ok(new { saved = true, message = "Lưu job thành công." });
        }

        _context.SavedJobs.Remove(savedJob);
        await _context.SaveChangesAsync();

        return Ok(new { saved = false, message = "Đã bỏ lưu job." });
    }

    private async Task<IActionResult?> ValidateRequestAsync(SaveJobRequest request)
    {
        if (request.CandidateId == Guid.Empty || request.JobId == Guid.Empty)
        {
            return BadRequest(new { message = "CandidateId và JobId là bắt buộc." });
        }

        var candidateExists = await _context.CandidateProfiles
            .AsNoTracking()
            .AnyAsync(x => x.Id == request.CandidateId);

        if (!candidateExists)
        {
            return NotFound(new { message = "Candidate không tồn tại." });
        }

        var jobExists = await _context.JobPosts
            .AsNoTracking()
            .AnyAsync(x => x.Id == request.JobId);

        if (!jobExists)
        {
            return NotFound(new { message = "Job không tồn tại." });
        }

        return null;
    }
}