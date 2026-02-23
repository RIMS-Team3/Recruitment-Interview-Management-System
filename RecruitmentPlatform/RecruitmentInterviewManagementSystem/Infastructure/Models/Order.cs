using System;
using System.Collections.Generic;

namespace RecruitmentInterviewManagementSystem.Models;

public partial class Order
{
    public Guid Id { get; set; }

    public string? OrderCode { get; set; }

    public Guid EmployerId { get; set; }

    public decimal? TotalAmount { get; set; }

    public int? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? PaidAt { get; set; }

    public virtual EmployerProfile Employer { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
