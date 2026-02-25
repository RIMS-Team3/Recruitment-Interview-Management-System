using Google.Apis.Auth;

namespace RecruitmentInterviewManagementSystem.Applications.Interface;

public interface IGoogleAuthService
{
    Task<GoogleJsonWebSignature.Payload> VerifyTokenAsync(string idToken);
}