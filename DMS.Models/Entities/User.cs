using System;
using System.Collections.Generic;

namespace DMS.Models.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual ICollection<Fee> Fees { get; set; } = new List<Fee>();

    public virtual ICollection<Request> RequestManagers { get; set; } = new List<Request>();

    public virtual ICollection<Request> RequestStudents { get; set; } = new List<Request>();

    public virtual ICollection<StudentRoom> StudentRooms { get; set; } = new List<StudentRoom>();
}
