using System;
using System.Collections.Generic;

namespace DMS.Models.Entities;

public partial class DormFacility
{
    public int Id { get; set; }

    public int DormitoryId { get; set; }

    public int FacilityId { get; set; }

    public int Quantity { get; set; }

    public virtual Dormitory Dormitory { get; set; } = null!;

    public virtual Facility Facility { get; set; } = null!;
}
