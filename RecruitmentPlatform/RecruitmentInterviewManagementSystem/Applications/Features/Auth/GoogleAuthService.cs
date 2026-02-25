using Google.Apis.Auth;
using RecruitmentInterviewManagementSystem.Applications.Interface;

namespace RecruitmentInterviewManagementSystem.Applications.Features.Auth;

public class GoogleAuthService : IGoogleAuthService
{
    public async Task<GoogleJsonWebSignature.Payload> VerifyTokenAsync(string idToken)
    {
        var clientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");

        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new List<string> { clientId }
        };

        var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

        return payload;
    }
}