using System.Collections.Generic;

namespace DMS_FE.Models
{
    public class RoomEditViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
        public int DormitoryId { get; set; }
        public List<DormitorySimpleViewModel> Dormitories { get; set; }
    }
} 