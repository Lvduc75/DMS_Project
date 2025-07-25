using System;
using System.Collections.Generic;

namespace DMS.Models.Entities;

public partial class Dormitory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<DormFacility> DormFacilities { get; set; } = new List<DormFacility>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
