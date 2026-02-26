using Microsoft.EntityFrameworkCore;
using RecruitmentInterviewManagementSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecruitmentInterviewManagementSystem.Infastructure.Models
{
    [Table("Interviews")]
    [Index(nameof(IdInterviewSlot), IsUnique = true)] // 👈 BẮT BUỘC
    public class Interviews
    {
        [Key]
        public Guid IdInterview { get; set; }

        public decimal TechnicalScore { get; set; } = 0;
        public decimal SoftSkillScore { get; set; } = 0;
        public string? Decision { get; set; }
        public string? Status { get; set; }

        [Required]
        public Guid IdInterviewSlot { get; set; }

        [ForeignKey("IdInterviewSlot")]
        public virtual InterviewsSlots? InterviewsSlots { get; set; }

        public Guid IdApplication { get; set; }

        [ForeignKey("IdApplication")]
        public Application? application { get; set; }
    }
}
