using System;
using System.Collections.Generic;

namespace DMS.Models.Entities;

public partial class UtilityReading
{
    public int Id { get; set; }

    public int RoomId { get; set; }

    public DateOnly ReadingMonth { get; set; }

    public decimal Electric { get; set; }

    public decimal Water { get; set; }

    public DateTime ImportedAt { get; set; }

    public virtual Room Room { get; set; } = null!;
}
