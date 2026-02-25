using Microsoft.AspNetCore.Mvc;
using RecruitmentInterviewManagementSystem.Applications.Interface;
using RecruitmentInterviewManagementSystem.Applications.Features.Auth.DTO;

namespace RecruitmentInterviewManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IGoogleAuthService _googleAuthService;

    public AuthController(IGoogleAuthService googleAuthService)
    {
        _googleAuthService = googleAuthService;
    }

    [HttpPost("google")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
    {
        if (string.IsNullOrEmpty(request.IdToken))
        {
            return BadRequest("IdToken is required");
        }

        var payload = await _googleAuthService.VerifyTokenAsync(request.IdToken);

        return Ok(new
        {
            email = payload.Email,
            name = payload.Name,
            picture = payload.Picture
        });
    }
}