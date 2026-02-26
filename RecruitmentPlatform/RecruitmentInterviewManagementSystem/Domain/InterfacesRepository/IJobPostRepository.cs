using RecruitmentInterviewManagementSystem.Applications.Features.JobPost.DTO;
using RecruitmentInterviewManagementSystem.Domain.Entities;
using RecruitmentInterviewManagementSystem.Models;

namespace RecruitmentInterviewManagementSystem.Domain.InterfacesRepository
{
    public interface IJobPostRepository
    {
        //Task<bool> AddNewJobPost(JobPostEntity job);
        Task<IEnumerable<JobPost>> GetAllAsync();
        Task<IEnumerable<JobPost>> GetFilteredJobsAsync(JobPostFilterRequest filter);

    }
}
