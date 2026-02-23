using System;
using System.Collections.Generic;

namespace RecruitmentInterviewManagementSystem.Models;

public partial class Company
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? TaxCode { get; set; }

    public string? Address { get; set; }

    public string? Website { get; set; }

    public string? Description { get; set; }

    public string? LogoUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<CompanySubscription> CompanySubscriptions { get; set; } = new List<CompanySubscription>();

    public virtual ICollection<EmployerProfile> EmployerProfiles { get; set; } = new List<EmployerProfile>();

    public virtual ICollection<JobPost> JobPosts { get; set; } = new List<JobPost>();
}
