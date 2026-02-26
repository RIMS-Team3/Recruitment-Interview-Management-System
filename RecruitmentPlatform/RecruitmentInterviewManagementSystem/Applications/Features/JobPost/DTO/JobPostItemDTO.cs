namespace RecruitmentInterviewManagementSystem.Applications.Features.JobPost.DTO
{
    public class JobPostItemDTO
    {
        public Guid IdJobPost { get; set; }
        public string? Title { get; set; }
        public string? Location { get; set; }
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public DateTime? ExpireAt { get; set; }
    }
}
