using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using RecruitmentInterviewManagementSystem.Models;

namespace RecruitmentInterviewManagementSystem.Infastructure.Models
{
    [Table("InterviewBookingTokens")]
    public class InterviewBookingToken
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ApplicationId { get; set; }

        [ForeignKey("ApplicationId")]
        public Application Application { get; set; }

        [Required]
        public string Token { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime ExpiredAt { get; set; }

        public bool IsUsed { get; set; } = false;
    }
}
