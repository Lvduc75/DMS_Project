using System;
using System.Collections.Generic;

namespace DMS.Models.DTOs
{
    public class FeeCreateDTO
    {
        public int StudentId { get; set; }
        public string Type { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Status { get; set; } = "unpaid";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateOnly? DueDate { get; set; }
    }

    public class FeeUpdateDTO
    {
        public int StudentId { get; set; }
        public string Type { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Status { get; set; } = null!;
        public DateOnly? DueDate { get; set; }
    }

    public class FeeResponseDTO
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public string Type { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateOnly? DueDate { get; set; }
        
        // Calculated properties
        public decimal TotalPaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        
        // Navigation properties
        public List<TransactionResponseDTO> Transactions { get; set; } = new List<TransactionResponseDTO>();
    }
} 