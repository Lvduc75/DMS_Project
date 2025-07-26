using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DMS.Models.Entities;
using DMS.Models.DTOs;

namespace DMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeeController : ControllerBase
    {
        private readonly DormManagementContext _context;

        public FeeController(DormManagementContext context)
        {
            _context = context;
        }

        // GET: api/fee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetFees()
        {
            var fees = await _context.Fees
                .Include(f => f.Student)
                .OrderByDescending(f => f.CreatedAt)
                .Select(f => new
                {
                    f.Id,
                    f.StudentId,
                    f.Type,
                    f.Amount,
                    f.Status,
                    f.CreatedAt,
                    f.DueDate,
                    Student = f.Student != null ? new
                    {
                        f.Student.Id,
                        f.Student.Name,
                        f.Student.Email
                    } : null
                })
                .ToListAsync();

            return Ok(fees);
        }

        // GET: api/fee/tracking
        [HttpGet("tracking")]
        public async Task<ActionResult<IEnumerable<object>>> GetTrackingFees()
        {
            var fees = await _context.Fees
                .Include(f => f.Student)
                .Include(f => f.Transactions)
                .Where(f => f.Status == "unpaid")
                .OrderBy(f => f.DueDate)
                .Select(f => new
                {
                    f.Id,
                    f.StudentId,
                    f.Type,
                    f.Amount,
                    f.Status,
                    f.CreatedAt,
                    f.DueDate,
                    Student = f.Student != null ? new
                    {
                        f.Student.Id,
                        f.Student.Name,
                        f.Student.Email
                    } : null,
                    Transactions = f.Transactions.Select(t => new
                    {
                        t.Id,
                        t.PayerName,
                        t.PaymentDate,
                        t.Amount
                    }).ToList()
                })
                .ToListAsync();

            return Ok(fees);
        }

        // GET: api/fee/overdue
        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<object>>> GetOverdueFees()
        {
            var fees = await _context.Fees
                .Include(f => f.Student)
                .Where(f => f.Status == "unpaid" && f.DueDate < DateOnly.FromDateTime(DateTime.Now))
                .OrderBy(f => f.DueDate)
                .Select(f => new
                {
                    f.Id,
                    f.StudentId,
                    f.Type,
                    f.Amount,
                    f.Status,
                    f.CreatedAt,
                    f.DueDate,
                    Student = f.Student != null ? new
                    {
                        f.Student.Id,
                        f.Student.Name,
                        f.Student.Email
                    } : null
                })
                .ToListAsync();

            return Ok(fees);
        }

        // GET: api/fee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetFee(int id)
        {
            var fee = await _context.Fees
                .Include(f => f.Student)
                .Include(f => f.Transactions)
                .Where(f => f.Id == id)
                .Select(f => new
                {
                    f.Id,
                    f.StudentId,
                    f.Type,
                    f.Amount,
                    f.Status,
                    f.CreatedAt,
                    f.DueDate,
                    Student = f.Student != null ? new
                    {
                        f.Student.Id,
                        f.Student.Name,
                        f.Student.Email
                    } : null,
                    Transactions = f.Transactions.Select(t => new
                    {
                        t.Id,
                        t.PayerName,
                        t.PaymentDate,
                        t.Amount
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (fee == null)
            {
                return NotFound();
            }

            return Ok(fee);
        }

        // POST: api/fee
        [HttpPost]
        public async Task<ActionResult<Fee>> CreateFee([FromBody] FeeCreateDTO feeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fee = new Fee
            {
                StudentId = feeDto.StudentId,
                Type = feeDto.Type,
                Amount = feeDto.Amount,
                Status = "unpaid",
                CreatedAt = DateTime.Now,
                DueDate = feeDto.DueDate
            };

            _context.Fees.Add(fee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFee), new { id = fee.Id }, fee);
        }

        // PUT: api/fee/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFee(int id, [FromBody] FeeUpdateDTO feeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fee = await _context.Fees.FindAsync(id);
            if (fee == null)
            {
                return NotFound();
            }

            fee.StudentId = feeDto.StudentId;
            fee.Type = feeDto.Type;
            fee.Amount = feeDto.Amount;
            fee.Status = feeDto.Status;
            fee.DueDate = feeDto.DueDate;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/fee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFee(int id)
        {
            var fee = await _context.Fees.FindAsync(id);
            if (fee == null)
            {
                return NotFound();
            }

            _context.Fees.Remove(fee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FeeExists(int id)
        {
            return _context.Fees.Any(e => e.Id == id);
        }
    }

    public class FeeCreateDTO
    {
        public int StudentId { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateOnly? DueDate { get; set; }
    }

    public class FeeUpdateDTO
    {
        public int StudentId { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateOnly? DueDate { get; set; }
    }
} 