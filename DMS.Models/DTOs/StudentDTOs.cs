namespace DMS.Models.DTOs;

public class StudentDormitoryDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Status { get; set; } = null!;
    public int TotalRooms { get; set; }
    public int AvailableRooms { get; set; }
    public int OccupiedRooms { get; set; }
    public List<string> Facilities { get; set; } = new List<string>();
}

public class StudentRoomDTO
{
    public int Id { get; set; }
    public string RoomNumber { get; set; } = null!;
    public int Floor { get; set; }
    public string Status { get; set; } = null!;
    public string RoomType { get; set; } = null!;
    public decimal Price { get; set; }
    public int Capacity { get; set; }
    public int CurrentOccupants { get; set; }
    public string DormitoryName { get; set; } = null!;
    public List<string> Facilities { get; set; } = new List<string>();
}

public class StudentProfileDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string Role { get; set; } = null!;
    public string? CurrentRoom { get; set; }
    public string? DormitoryName { get; set; }
} 