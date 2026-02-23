using System;
using System.Collections.Generic;

namespace RecruitmentInterviewManagementSystem.Models;

public partial class CvSkill
{
    public Guid Cvid { get; set; }

    public string SkillName { get; set; } = null!;

    public int? Level { get; set; }

    public virtual Cv Cv { get; set; } = null!;
}
