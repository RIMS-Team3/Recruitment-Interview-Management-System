using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecruitmentInterviewManagementSystem.Applications.Features.Advertisement.DTO;
using RecruitmentInterviewManagementSystem.Applications.Features.Advertisement.Interface;

namespace RecruitmentInterviewManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementsController : ControllerBase
    {
        private readonly IAdvertisementService _service;

        public AdvertisementsController(IAdvertisementService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var ad = await _service.GetByIdAsync(id);
            if (ad == null) return NotFound();
            return Ok(ad);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateAdvertisementDTO dto)
        {
            if (dto.ImageFile == null || dto.ImageFile.Length == 0) return BadRequest("File ảnh trống");
            var ad = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = ad.Id }, ad);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateAdvertisementDTO dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
