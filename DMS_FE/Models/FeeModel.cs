using System;
using System.Collections.Generic;

namespace DMS_FE.Models
{
    public class FeeModel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = "unpaid";
        public DateTime CreatedAt { get; set; }
        public DateOnly? DueDate { get; set; }
        
        // Calculated properties
        public decimal TotalPaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        
        // Navigation properties
        public UserModel? Student { get; set; }
        public List<TransactionModel>? Transactions { get; set; }
    }

    public class TransactionModel
    {
        public int Id { get; set; }
        public int FeeId { get; set; }
        public string? PayerName { get; set; }
        public DateOnly PaymentDate { get; set; }
        public decimal Amount { get; set; }
        
        // Additional properties for display
        public string FeeType { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
    }
} 