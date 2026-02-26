using Microsoft.AspNetCore.Mvc;
using RecruitmentInterviewManagementSystem.Applications.Features.JobPostDetail.Interface;

namespace RecruitmentInterviewManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/jobs")]
    public class JobPostController : ControllerBase
    {
        private readonly IJobPostDetailService _service;

        public JobPostController(IJobPostDetailService service)
        {
            _service = service;
        }

        [HttpGet("{jobId}")]
        public async Task<IActionResult> GetJobDetail(Guid jobId)
        {
            var job = await _service.GetJobPostDetailAsync(jobId);

            if (job == null)
                return NotFound("Job not found");

            return Ok(job);
        }
    }
}