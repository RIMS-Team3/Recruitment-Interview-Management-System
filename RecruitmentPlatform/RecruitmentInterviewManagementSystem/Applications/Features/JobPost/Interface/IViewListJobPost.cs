using RecruitmentInterviewManagementSystem.Applications.Features.JobPost.DTO;

namespace RecruitmentInterviewManagementSystem.Applications.Features.JobPost.Interface
{
    public  interface IViewListJobPost
    {
        //Task<List<JobPostItemDTO>> Excute(RequestGetViewListJobPostDTO request );
        Task<IEnumerable<JobPostItemDTO>> ExecuteAsync(RequestGetViewListJobPostDTO request);
    }
}
