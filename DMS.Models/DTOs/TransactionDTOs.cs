using System;

namespace DMS.Models.DTOs
{
    public class TransactionCreateDTO
    {
        public int FeeId { get; set; }
        public string PayerName { get; set; } = null!;
        public DateOnly PaymentDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public decimal Amount { get; set; }
    }

    public class TransactionResponseDTO
    {
        public int Id { get; set; }
        public int FeeId { get; set; }
        public string PayerName { get; set; } = null!;
        public DateOnly PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string FeeType { get; set; } = null!;
        public string StudentName { get; set; } = null!;
    }
} 