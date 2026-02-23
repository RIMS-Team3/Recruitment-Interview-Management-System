using System;
using System.Collections.Generic;

namespace RecruitmentInterviewManagementSystem.Models;

public partial class CompanySubscription
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public Guid ServicePackageId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? RemainingPost { get; set; }

    public bool? IsActive { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ServicePackage ServicePackage { get; set; } = null!;
}
