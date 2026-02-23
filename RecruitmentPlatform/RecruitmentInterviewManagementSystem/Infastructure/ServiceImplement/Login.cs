using RecruitmentInterviewManagementSystem.Applications.DTOs;
using RecruitmentInterviewManagementSystem.Applications.Features.Auth.DTO;
using RecruitmentInterviewManagementSystem.Applications.Features.Interface;

namespace RecruitmentInterviewManagementSystem.Infastructure.ServiceImplement
{
    public class Login : ILogin
    {
        public Task<LoginResponse> LoginAsync(RequestLogin request)
        {
            throw new NotImplementedException();
        }
    }
}
