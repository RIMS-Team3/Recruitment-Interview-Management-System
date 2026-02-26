using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecruitmentInterviewManagementSystem.Infastructure.Models
{
    [Table("InterviewSlots")]
    public class InterviewsSlots
    {
        [Key]
        public Guid IdInterviewSlot { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public bool IsBooked { get; set; } = false;



        public Guid IdCompany { get; set; }

        public Interviews? Interview { get; set; }
    }
}
