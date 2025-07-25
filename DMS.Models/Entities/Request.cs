using System;
using System.Collections.Generic;

namespace DMS.Models.Entities;

public partial class Request
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int? ManagerId { get; set; }

    public string Type { get; set; } = null!;

    public string? Description { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual User? Manager { get; set; }

    public virtual User Student { get; set; } = null!;
}
