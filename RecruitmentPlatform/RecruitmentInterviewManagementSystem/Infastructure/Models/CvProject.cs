using System;
using System.Collections.Generic;

namespace RecruitmentInterviewManagementSystem.Models;

public partial class CvProject
{
    public Guid Id { get; set; }

    public Guid Cvid { get; set; }

    public string? ProjectName { get; set; }

    public string? Role { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? Description { get; set; }

    public virtual Cv Cv { get; set; } = null!;
}
