using RecruitmentInterviewManagementSystem.Applications.Features.JobPostDetail.Interface;

namespace RecruitmentInterviewManagementSystem.Applications.Features.JobPostDetail.Interface
{
    public interface IJobPostDetailService
    {
        Task<JobPostDetailDTO?> GetJobPostDetailAsync(Guid jobId);
    }
}