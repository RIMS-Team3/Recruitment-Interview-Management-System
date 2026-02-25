using Microsoft.Data.SqlClient.DataClassification;

namespace RecruitmentInterviewManagementSystem.Applications.Features.JobPost.DTO
{
    public class RequestGetViewListJobPostDTO
    {
        public decimal? Salary { get; set; }
        public int CurrentPage { get; set; } = 1;
    }
}
