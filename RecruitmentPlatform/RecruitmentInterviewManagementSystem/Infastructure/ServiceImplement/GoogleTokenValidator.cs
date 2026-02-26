using Google.Apis.Auth;
using RecruitmentInterviewManagementSystem.Applications.Features.Interface;

namespace RecruitmentInterviewManagementSystem.Infastructure.ServiceImplement;

public class GoogleTokenValidator : IGoogleTokenValidator
{
    public async Task<GoogleJsonWebSignature.Payload> ValidateAsync(string idToken)
    {
        return await GoogleJsonWebSignature.ValidateAsync(idToken);
    }
}