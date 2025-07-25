using System;
using System.Collections.Generic;

namespace DMS.Models.Entities;

public partial class UtilityReading
{
    public int Id { get; set; }

    public int RoomId { get; set; }

    public DateOnly ReadingMonth { get; set; }

    public double Electric { get; set; }

    public double Water { get; set; }

    public DateTime ImportedAt { get; set; }

    public virtual Room Room { get; set; } = null!;
}
