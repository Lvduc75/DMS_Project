using Microsoft.AspNetCore.Mvc;
using DMS.Models.Entities;
using DMS.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly DormManagementContext _context;

        public TransactionController(DormManagementContext context)
        {
            _context = context;
        }

        // GET: /api/transaction/fee/{feeId}
        [HttpGet("fee/{feeId}")]
        public async Task<IActionResult> GetTransactionsByFee(int feeId)
        {
            var transactions = await _context.Transactions
                .Include(t => t.Fee)
                .ThenInclude(f => f.Student)
                .Where(t => t.FeeId == feeId)
                .Select(t => new TransactionResponseDTO
                {
                    Id = t.Id,
                    FeeId = t.FeeId,
                    PayerName = t.PayerName,
                    PaymentDate = t.PaymentDate,
                    Amount = t.Amount,
                    FeeType = t.Fee.Type,
                    StudentName = t.Fee.Student.Name
                })
                .ToListAsync();

            return Ok(transactions);
        }

        // GET: /api/transaction/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction(int id)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Fee)
                .ThenInclude(f => f.Student)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
                return NotFound();

            var transactionResponse = new TransactionResponseDTO
            {
                Id = transaction.Id,
                FeeId = transaction.FeeId,
                PayerName = transaction.PayerName,
                PaymentDate = transaction.PaymentDate,
                Amount = transaction.Amount,
                FeeType = transaction.Fee.Type,
                StudentName = transaction.Fee.Student.Name
            };

            return Ok(transactionResponse);
        }

        // POST: /api/transaction
        [HttpPost]
        public async Task<IActionResult> AddTransaction([FromBody] TransactionCreateDTO transactionDto)
        {
            var fee = await _context.Fees.FirstOrDefaultAsync(f => f.Id == transactionDto.FeeId);
            if (fee == null)
            {
                return BadRequest($"Fee with id {transactionDto.FeeId} does not exist.");
            }

            var transaction = new Transaction
            {
                FeeId = transactionDto.FeeId,
                PayerName = transactionDto.PayerName,
                PaymentDate = transactionDto.PaymentDate,
                Amount = transactionDto.Amount
            };

            _context.Transactions.Add(transaction);

            // Business rule: update fee to paid
            fee.Status = "paid";
            _context.Fees.Update(fee);

            await _context.SaveChangesAsync();

            var transactionResponse = new TransactionResponseDTO
            {
                Id = transaction.Id,
                FeeId = transaction.FeeId,
                PayerName = transaction.PayerName,
                PaymentDate = transaction.PaymentDate,
                Amount = transaction.Amount,
                FeeType = fee.Type,
                StudentName = fee.Student.Name
            };

            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transactionResponse);
        }

        // DELETE: /api/transaction/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Fee)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
                return NotFound();

            // Revert fee status to unpaid
            transaction.Fee.Status = "unpaid";
            _context.Fees.Update(transaction.Fee);

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: /api/transaction/student/{studentId}
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetTransactionsByStudent(int studentId)
        {
            var transactions = await _context.Transactions
                .Include(t => t.Fee)
                .ThenInclude(f => f.Student)
                .Where(t => t.Fee.StudentId == studentId)
                .Select(t => new TransactionResponseDTO
                {
                    Id = t.Id,
                    FeeId = t.FeeId,
                    PayerName = t.PayerName,
                    PaymentDate = t.PaymentDate,
                    Amount = t.Amount,
                    FeeType = t.Fee.Type,
                    StudentName = t.Fee.Student.Name
                })
                .OrderByDescending(t => t.PaymentDate)
                .ToListAsync();

            return Ok(transactions);
        }

        // GET: /api/transaction/summary
        [HttpGet("summary")]
        public async Task<IActionResult> GetTransactionSummary([FromQuery] DateOnly? fromDate, [FromQuery] DateOnly? toDate)
        {
            var query = _context.Transactions.AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(t => t.PaymentDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(t => t.PaymentDate <= toDate.Value);
            }

            var summary = await query
                .GroupBy(t => t.PaymentDate)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalAmount = g.Sum(t => t.Amount),
                    TransactionCount = g.Count()
                })
                .OrderByDescending(s => s.Date)
                .ToListAsync();

            return Ok(summary);
        }
    }
} 