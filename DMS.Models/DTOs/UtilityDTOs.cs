using System;

namespace DMS.Models.DTOs
{
    public class UtilityReadingCreateDTO
    {
        public int RoomId { get; set; }
        public string ReadingMonth { get; set; } = null!; // Format: yyyy-MM
        public decimal Electric { get; set; }
        public decimal Water { get; set; }
    }

    public class UtilityReadingResponseDTO
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string RoomCode { get; set; } = null!;
        public DateOnly ReadingMonth { get; set; }
        public decimal Electric { get; set; }
        public decimal Water { get; set; }
        public DateTime ImportedAt { get; set; }
    }

    public class UtilityBillDTO
    {
        public int RoomId { get; set; }
        public string RoomCode { get; set; } = null!;
        public string Month { get; set; } = null!;
        public decimal ElectricReading { get; set; }
        public decimal WaterReading { get; set; }
        public decimal ElectricPrice { get; set; }
        public decimal WaterPrice { get; set; }
        public decimal ElectricAmount { get; set; }
        public decimal WaterAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
} 