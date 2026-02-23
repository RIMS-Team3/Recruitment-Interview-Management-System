using System;
using System.Collections.Generic;

namespace RecruitmentInterviewManagementSystem.Models;

public partial class Skill
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<CandidateSkill> CandidateSkills { get; set; } = new List<CandidateSkill>();

    public virtual ICollection<JobPost> Jobs { get; set; } = new List<JobPost>();
}
