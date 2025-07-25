using System.Collections.Generic;

namespace DMS_FE.Models
{
    public class RoomManageViewModel
    {
        public List<RoomModel> Rooms { get; set; }
        public List<DormitoryModel> Dormitories { get; set; }
        public int? SelectedDormitoryId { get; set; }
    }
} 