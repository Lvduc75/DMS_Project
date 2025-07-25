using System;
using System.Collections.Generic;

namespace DMS.Models.Entities;

public partial class Facility
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public virtual ICollection<DormFacility> DormFacilities { get; set; } = new List<DormFacility>();

    public virtual ICollection<RoomFacility> RoomFacilities { get; set; } = new List<RoomFacility>();
}
