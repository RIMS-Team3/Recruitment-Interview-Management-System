using System;
using System.Collections.Generic;

namespace RecruitmentInterviewManagementSystem.Models;

public partial class SavedJob
{
    public Guid CandidateId { get; set; }

    public Guid JobId { get; set; }

    public DateTime? SavedAt { get; set; }

    public virtual CandidateProfile Candidate { get; set; } = null!;

    public virtual JobPost Job { get; set; } = null!;
}
