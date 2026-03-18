namespace RecruitmentInterviewManagementSystem.Applications.Features.Schedule
{
    public interface IViewListScheduleForCandidate
    {
        Task<List<ScheduleForCandidateDto>> Execute(string Token);
    }

    public class ScheduleForCandidateDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
    }
}
