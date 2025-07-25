using System;
using System.Collections.Generic;

namespace DMS.Models.Entities;

public partial class RoomFacility
{
    public int Id { get; set; }

    public int RoomId { get; set; }

    public int FacilityId { get; set; }

    public int Quantity { get; set; }

    public virtual Facility Facility { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;
}
