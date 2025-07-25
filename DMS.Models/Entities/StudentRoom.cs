using System;
using System.Collections.Generic;

namespace DMS.Models.Entities;

public partial class StudentRoom
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int RoomId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual Room Room { get; set; } = null!;

    public virtual User Student { get; set; } = null!;
}
