namespace DMS.Models.DTOs
{
    public class RoomFacilityCreateDTO
    {
        public int RoomId { get; set; }
        public int FacilityId { get; set; }
        public int Quantity { get; set; }
    }

    public class RoomFacilityUpdateDTO
    {
        public int RoomId { get; set; }
        public int FacilityId { get; set; }
        public int Quantity { get; set; }
    }
} 