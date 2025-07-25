using System;
using System.Collections.Generic;

namespace DMS.Models.Entities;

public partial class Transaction
{
    public int Id { get; set; }

    public int FeeId { get; set; }

    public string? PayerName { get; set; }

    public DateOnly PaymentDate { get; set; }

    public decimal Amount { get; set; }

    public virtual Fee Fee { get; set; } = null!;
}
