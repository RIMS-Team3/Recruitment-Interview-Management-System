using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecruitmentInterviewManagementSystem.Applications.Features.ServicePackage.DTO;
using RecruitmentInterviewManagementSystem.Applications.Features.ServicePackage.Interface;

namespace RecruitmentInterviewManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicePackagesController : ControllerBase
    {
        private readonly IServicePackageService _servicePackageService;

        public ServicePackagesController(IServicePackageService servicePackageService)
        {
            _servicePackageService = servicePackageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _servicePackageService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _servicePackageService.GetByIdAsync(id);
            if (result == null) return NotFound("Không tìm thấy Service Package.");
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateServicePackageDto request)
        {
            var newId = await _servicePackageService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = newId }, new { Id = newId });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateServicePackageDto request)
        {
            var success = await _servicePackageService.UpdateAsync(id, request);
            if (!success) return NotFound("Không tìm thấy Service Package để cập nhật.");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _servicePackageService.DeleteAsync(id);
            if (!success) return NotFound("Không tìm thấy Service Package để xóa.");
            return NoContent();
        }
    }
}
