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

    public class RoomFacilityDTO
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int FacilityId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; } = "Active";
        public string? RoomName { get; set; }
        public string? FacilityName { get; set; }
    }
} 