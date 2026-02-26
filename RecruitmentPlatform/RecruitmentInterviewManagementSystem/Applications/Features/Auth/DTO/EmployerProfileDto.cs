namespace RecruitmentInterviewManagementSystem.Applications.Features.Auth.DTO
{
    public class EmployerProfileDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CompanyId { get; set; }
        public string? Position { get; set; }
    }
}