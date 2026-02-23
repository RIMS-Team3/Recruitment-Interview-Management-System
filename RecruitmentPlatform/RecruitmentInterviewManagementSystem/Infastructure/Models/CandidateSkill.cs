using System;
using System.Collections.Generic;

namespace RecruitmentInterviewManagementSystem.Models;

public partial class CandidateSkill
{
    public Guid CandidateId { get; set; }

    public Guid SkillId { get; set; }

    public int? Level { get; set; }

    public virtual CandidateProfile Candidate { get; set; } = null!;

    public virtual Skill Skill { get; set; } = null!;
}
