
namespace RecruitmentInterviewManagementSystem.Domain.InterfacesRepository
{
    public interface IJobPostDetailRepository
    {
        Task<JobPostDetailDTO?> GetJobPostDetailAsync(Guid jobId);
    }
}