namespace RecruitmentInterviewManagementSystem.Applications.Features.BookingInterviewSlot.DTO
{
    public class RequestUpdateInterviewSlots
    {
        public Guid IdInterviewSlot { get; set; }

        public Guid IdCompany { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

    }
}
