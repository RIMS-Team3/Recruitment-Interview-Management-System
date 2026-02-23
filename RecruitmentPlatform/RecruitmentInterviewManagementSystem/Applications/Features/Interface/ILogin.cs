using RecruitmentInterviewManagementSystem.Applications.DTOs;
using RecruitmentInterviewManagementSystem.Applications.Features.Auth.DTO;

namespace RecruitmentInterviewManagementSystem.Applications.Features.Interface
{
    public interface ILogin
    {
        Task<LoginResponse> LoginAsync(RequestLogin request);
    }
}
