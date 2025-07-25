using System;
using System.Collections.Generic;

namespace DMS.Models.Entities;

public partial class Room
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public int Capacity { get; set; }

    public string Status { get; set; } = null!;

    public int DormitoryId { get; set; }

    public virtual Dormitory Dormitory { get; set; } = null!;

    public virtual ICollection<RoomFacility> RoomFacilities { get; set; } = new List<RoomFacility>();

    public virtual ICollection<StudentRoom> StudentRooms { get; set; } = new List<StudentRoom>();

    public virtual ICollection<UtilityReading> UtilityReadings { get; set; } = new List<UtilityReading>();
}
