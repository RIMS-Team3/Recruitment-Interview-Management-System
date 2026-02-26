namespace RecruitmentInterviewManagementSystem.Domain.Entities
{
    public class InterviewEnity
    {
        public Guid Id { get; private set; }
        public double? TechnicalScore { get; private set; }
        public double? SoftSkillScore { get; private set; }
        public string Status { get; private set; }
        public Guid IdInterviewSlot { get; private set; }
        public Guid IdApplication { get; private set; }

        public void UpdateResult(double tech, double soft)
        {
            TechnicalScore = tech;
            SoftSkillScore = soft;
            Status = (tech + soft) / 2 >= 5 ? "Passed" : "Failed";
        }
    }
}
