namespace RecruitmentInterviewManagementSystem.Applications.Features.BookingInterviewSlot.DTO
{
    public class RequestViewInterviewSlots
    {

        public Guid IdCompany { get; set; }

        public DateTime? ChooesDate { get; set; }

        public int CurrentPage { get; set; } = 1 ;
    }
}
