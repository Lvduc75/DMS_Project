using System;
using System.Collections.Generic;

namespace DMS.Models.Entities;

public partial class Fee
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public string Type { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateOnly? DueDate { get; set; }

    public virtual User Student { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
