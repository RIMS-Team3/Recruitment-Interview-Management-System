namespace RecruitmentInterviewManagementSystem.Domain.Entities
{
    public class InterviewSlotEntity
    {
        public Guid Id { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public bool IsBooked { get; private set; }

        public InterviewSlotEntity(DateTime start, DateTime end)
        {
            if (start >= end) throw new Exception("Thời gian bắt đầu phải trước kết thúc.");
            Id = Guid.NewGuid();
            StartTime = start;
            EndTime = end;
            IsBooked = false;
        }

        public void Book() => IsBooked = true;
    }
}
