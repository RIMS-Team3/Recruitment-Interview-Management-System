using System;
using System.Collections.Generic;

namespace RecruitmentInterviewManagementSystem.Models;

public partial class CvCertificate
{
    public Guid Id { get; set; }

    public Guid Cvid { get; set; }

    public string? CertificateName { get; set; }

    public string? Organization { get; set; }

    public DateOnly? IssueDate { get; set; }

    public DateOnly? ExpiredDate { get; set; }

    public virtual Cv Cv { get; set; } = null!;
}
