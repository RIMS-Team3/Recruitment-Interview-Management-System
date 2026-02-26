namespace RecruitmentInterviewManagementSystem.Applications.Features.BookingInterviewSlot.DTO
{
    public class ResponseInterViewSlots
    {
        public string? NameCompany { get; set; }

        public string? Address { get; set; }

        public string? UrlWebsite { get; set; }

        public string? Description { get; set; }

        public string? UrlLogoImage { get; set; }

        public List<InterviewSlotItem>? interviewSlotItems { get; set; }

        public int NumberOfPages { get; set; } 
    }


    public class InterviewSlotItem
    {
        public Guid IdInterviewSlot { get; set; }
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool? IsBooked { get; set; }

        public Guid IdCompany { get; set; }
    }
}
