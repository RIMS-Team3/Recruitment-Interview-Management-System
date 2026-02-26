using RecruitmentInterviewManagementSystem.Applications.Features.JobPost.DTO;
using RecruitmentInterviewManagementSystem.Applications.Features.JobPost.Interface;

namespace RecruitmentInterviewManagementSystem.Infastructure.ServiceImplement
{
    public class ViewListJobPost : IViewListJobPost
    {
        public ViewListJobPost()
        {
        }

        public Task<IEnumerable<JobPostItemDTO>> ExecuteAsync(RequestGetViewListJobPostDTO request)
        {
            throw new NotImplementedException();
        }
    }
}
