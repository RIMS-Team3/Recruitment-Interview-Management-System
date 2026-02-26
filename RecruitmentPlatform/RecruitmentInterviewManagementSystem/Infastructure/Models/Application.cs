using RecruitmentInterviewManagementSystem.Infastructure.Models;
using System;
using System.Collections.Generic;

namespace RecruitmentInterviewManagementSystem.Models;

public partial class Application
{
    public Guid Id { get; set; }

    public Guid JobId { get; set; }

    public Guid CandidateId { get; set; }

    public Guid Cvid { get; set; }

    public int? Status { get; set; }

    public DateTime? AppliedAt { get; set; }

    public virtual CandidateProfile Candidate { get; set; } = null!;

    public virtual Cv Cv { get; set; } = null!;

    public virtual JobPost Job { get; set; } = null!;


    public virtual ICollection<Interviews>? Interviews { get; set; }

}
