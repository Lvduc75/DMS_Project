namespace DMS_FE.Models
{
    public class RoomModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
        public int DormitoryId { get; set; }
        public string DormitoryName { get; set; }
    }
} 