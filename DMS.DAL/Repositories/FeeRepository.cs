using DMS.DAL.Interfaces;
using DMS.Models.Entities;
using DMS.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DMS.DAL.Repositories
{
    public class FeeRepository : IFeeRepository
    {
        private readonly DormManagementContext _context;

        public FeeRepository(DormManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Fee>> GetAllAsync()
        {
            return await _context.Fees.ToListAsync();
        }

        public async Task<IEnumerable<FeeResponseDTO>> GetAllWithFiltersAsync(string? type, int? month, int? year)
        {
            var query = _context.Fees.Include(f => f.Student).AsQueryable();

            if (!string.IsNullOrEmpty(type))
                query = query.Where(f => f.Type == type);

            if (month.HasValue && year.HasValue)
            {
                var startDate = new DateTime(year.Value, month.Value, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                query = query.Where(f => f.CreatedAt >= startDate && f.CreatedAt <= endDate);
            }

            return await query
                .Select(f => new FeeResponseDTO
                {
                    Id = f.Id,
                    StudentId = f.StudentId,
                    StudentName = f.Student.Name,
                    Type = f.Type,
                    Amount = f.Amount,
                    Status = f.Status,
                    CreatedAt = f.CreatedAt,
                    DueDate = f.DueDate,
                    TotalPaidAmount = 0, // Will be calculated separately
                    RemainingAmount = f.Amount,
                    Transactions = new List<TransactionResponseDTO>()
                })
                .ToListAsync();
        }

        public async Task<Fee?> GetByIdAsync(int id)
        {
            return await _context.Fees.FindAsync(id);
        }

        public async Task<FeeResponseDTO?> GetByIdWithDetailsAsync(int id)
        {
            var fee = await _context.Fees
                .Include(f => f.Student)
                .Include(f => f.Transactions)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (fee == null)
                return null;

            return new FeeResponseDTO
            {
                Id = fee.Id,
                StudentId = fee.StudentId,
                StudentName = fee.Student.Name,
                Type = fee.Type,
                Amount = fee.Amount,
                Status = fee.Status,
                CreatedAt = fee.CreatedAt,
                DueDate = fee.DueDate,
                TotalPaidAmount = fee.Transactions.Sum(t => t.Amount),
                RemainingAmount = fee.Amount - fee.Transactions.Sum(t => t.Amount),
                Transactions = fee.Transactions.Select(t => new TransactionResponseDTO
                {
                    Id = t.Id,
                    FeeId = t.FeeId,
                    PayerName = t.PayerName,
                    Amount = t.Amount,
                    PaymentDate = t.PaymentDate,
                    FeeType = fee.Type,
                    StudentName = fee.Student.Name
                }).ToList()
            };
        }

        public async Task<Fee> CreateAsync(Fee fee)
        {
            _context.Fees.Add(fee);
            await _context.SaveChangesAsync();
            return fee;
        }

        public async Task<Fee> UpdateAsync(Fee fee)
        {
            _context.Fees.Update(fee);
            await _context.SaveChangesAsync();
            return fee;
        }

        public async Task DeleteAsync(int id)
        {
            var fee = await _context.Fees.FindAsync(id);
            if (fee != null)
            {
                _context.Fees.Remove(fee);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<FeeResponseDTO>> GetByStudentAsync(int studentId)
        {
            return await _context.Fees
                .Include(f => f.Student)
                .Include(f => f.Transactions)
                .Where(f => f.StudentId == studentId)
                .Select(f => new FeeResponseDTO
                {
                    Id = f.Id,
                    StudentId = f.StudentId,
                    StudentName = f.Student.Name,
                    Type = f.Type,
                    Amount = f.Amount,
                    Status = f.Status,
                    CreatedAt = f.CreatedAt,
                    DueDate = f.DueDate,
                    TotalPaidAmount = f.Transactions.Sum(t => t.Amount),
                    RemainingAmount = f.Amount - f.Transactions.Sum(t => t.Amount),
                    Transactions = f.Transactions.Select(t => new TransactionResponseDTO
                    {
                        Id = t.Id,
                        FeeId = t.FeeId,
                        PayerName = t.PayerName,
                        Amount = t.Amount,
                        PaymentDate = t.PaymentDate,
                        FeeType = f.Type,
                        StudentName = f.Student.Name
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<FeeResponseDTO>> GetOverdueFeesAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            return await _context.Fees
                .Include(f => f.Student)
                .Include(f => f.Transactions)
                .Where(f => f.DueDate.HasValue && f.DueDate.Value < today && f.Status != "paid")
                .Select(f => new FeeResponseDTO
                {
                    Id = f.Id,
                    StudentId = f.StudentId,
                    StudentName = f.Student.Name,
                    Type = f.Type,
                    Amount = f.Amount,
                    Status = f.Status,
                    CreatedAt = f.CreatedAt,
                    DueDate = f.DueDate,
                    TotalPaidAmount = f.Transactions.Sum(t => t.Amount),
                    RemainingAmount = f.Amount - f.Transactions.Sum(t => t.Amount),
                    Transactions = f.Transactions.Select(t => new TransactionResponseDTO
                    {
                        Id = t.Id,
                        FeeId = t.FeeId,
                        PayerName = t.PayerName,
                        Amount = t.Amount,
                        PaymentDate = t.PaymentDate,
                        FeeType = f.Type,
                        StudentName = f.Student.Name
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<bool> StudentExistsAsync(int studentId)
        {
            return await _context.Users.AnyAsync(u => u.Id == studentId);
        }
    }
} 