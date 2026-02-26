using RecruitmentInterviewManagementSystem.Applications.Features.JobPostDetail.Interface;
using RecruitmentInterviewManagementSystem.Domain.InterfacesRepository;

namespace RecruitmentInterviewManagementSystem.Infastructure.ServiceImplement
{
    public class JobPostDetailService : IJobPostDetailService
    {
        private readonly IJobPostDetailRepository _repository;

        public JobPostDetailService(IJobPostDetailRepository repository)
        {
            _repository = repository;
        }

        public async Task<JobPostDetailDTO?> GetJobPostDetailAsync(Guid jobId)
        {
            return await _repository.GetJobPostDetailAsync(jobId);
        }
    }
}