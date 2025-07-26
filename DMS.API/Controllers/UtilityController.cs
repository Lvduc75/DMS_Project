using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DMS.Models.Entities;

namespace DMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UtilityController : ControllerBase
    {
        private readonly DormManagementContext _context;

        public UtilityController(DormManagementContext context)
        {
            _context = context;
        }

        // GET: api/utility
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetUtilityReadings()
        {
            var readings = await _context.UtilityReadings
                .Include(u => u.Room)
                .ThenInclude(r => r.Dormitory)
                .Include(u => u.Room)
                .ThenInclude(r => r.StudentRooms)
                .ThenInclude(sr => sr.Student)
                .OrderByDescending(u => u.ReadingMonth)
                .Select(u => new
                {
                    u.Id,
                    u.RoomId,
                    u.ReadingMonth,
                    u.Electric,
                    u.Water,
                    u.ImportedAt,
                    Room = u.Room != null ? new
                    {
                        u.Room.Id,
                        u.Room.Code,
                        u.Room.Capacity,
                        u.Room.Status,
                        u.Room.DormitoryId,
                        Dormitory = u.Room.Dormitory != null ? new
                        {
                            u.Room.Dormitory.Id,
                            u.Room.Dormitory.Name
                        } : null,
                        StudentRooms = u.Room.StudentRooms.Select(sr => new
                        {
                            sr.Id,
                            sr.StudentId,
                            sr.RoomId,
                            sr.StartDate,
                            sr.EndDate,
                            Student = sr.Student != null ? new
                            {
                                sr.Student.Id,
                                sr.Student.Name,
                                sr.Student.Email
                            } : null
                        }).ToList()
                    } : null
                })
                .ToListAsync();

            return Ok(readings);
        }

        // GET: api/utility/bill
        [HttpGet("bill")]
        public async Task<ActionResult<IEnumerable<object>>> GetRoomsForBilling()
        {
            var rooms = await _context.Rooms
                .Include(r => r.StudentRooms)
                .ThenInclude(sr => sr.Student)
                .Include(r => r.Dormitory)
                .Where(r => r.StudentRooms.Any())
                .Select(r => new
                {
                    r.Id,
                    r.Code,
                    r.Capacity,
                    r.Status,
                    r.DormitoryId,
                    Dormitory = r.Dormitory != null ? new
                    {
                        r.Dormitory.Id,
                        r.Dormitory.Name
                    } : null,
                    StudentRooms = r.StudentRooms.Select(sr => new
                    {
                        sr.Id,
                        sr.StudentId,
                        sr.RoomId,
                        sr.StartDate,
                        sr.EndDate,
                        Student = sr.Student != null ? new
                        {
                            sr.Student.Id,
                            sr.Student.Name,
                            sr.Student.Email
                        } : null
                    }).ToList()
                })
                .ToListAsync();

            return Ok(rooms);
        }

        // GET: api/utility/history
        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<object>>> GetUtilityHistory([FromQuery] int? roomId, [FromQuery] string? month)
        {
            var query = _context.UtilityReadings.AsQueryable();

            if (roomId.HasValue)
            {
                query = query.Where(u => u.RoomId == roomId.Value);
            }

            if (!string.IsNullOrEmpty(month))
            {
                if (DateOnly.TryParse(month + "-01", out var startDate))
                {
                    var endDate = startDate.AddMonths(1).AddDays(-1);
                    query = query.Where(u => u.ReadingMonth >= startDate && u.ReadingMonth <= endDate);
                }
            }

            var readings = await query
                .Include(u => u.Room)
                .ThenInclude(r => r.Dormitory)
                .Include(u => u.Room)
                .ThenInclude(r => r.StudentRooms)
                .ThenInclude(sr => sr.Student)
                .OrderByDescending(u => u.ReadingMonth)
                .Select(u => new
                {
                    u.Id,
                    u.RoomId,
                    u.ReadingMonth,
                    u.Electric,
                    u.Water,
                    u.ImportedAt,
                    Room = u.Room != null ? new
                    {
                        u.Room.Id,
                        u.Room.Code,
                        u.Room.Capacity,
                        u.Room.Status,
                        u.Room.DormitoryId,
                        Dormitory = u.Room.Dormitory != null ? new
                        {
                            u.Room.Dormitory.Id,
                            u.Room.Dormitory.Name
                        } : null,
                        StudentRooms = u.Room.StudentRooms.Select(sr => new
                        {
                            sr.Id,
                            sr.StudentId,
                            sr.RoomId,
                            sr.StartDate,
                            sr.EndDate,
                            Student = sr.Student != null ? new
                            {
                                sr.Student.Id,
                                sr.Student.Name,
                                sr.Student.Email
                            } : null
                        }).ToList()
                    } : null
                })
                .ToListAsync();

            return Ok(readings);
        }

        // GET: api/utility/monthly
        [HttpGet("monthly")]
        public async Task<ActionResult<object>> GetMonthlyReport([FromQuery] string month)
        {
            if (string.IsNullOrEmpty(month))
            {
                month = DateTime.Now.ToString("yyyy-MM");
            }

            if (DateOnly.TryParse(month + "-01", out var startDate))
            {
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var monthlyReport = await _context.UtilityReadings
                    .Include(u => u.Room)
                    .ThenInclude(r => r.Dormitory)
                    .Include(u => u.Room)
                    .ThenInclude(r => r.StudentRooms)
                    .ThenInclude(sr => sr.Student)
                    .Where(u => u.ReadingMonth >= startDate && u.ReadingMonth <= endDate)
                    .GroupBy(u => u.RoomId)
                    .Select(g => new
                    {
                        RoomId = g.Key,
                        Room = g.First().Room != null ? new
                        {
                            g.First().Room.Id,
                            g.First().Room.Code,
                            g.First().Room.Capacity,
                            g.First().Room.Status,
                            g.First().Room.DormitoryId,
                            Dormitory = g.First().Room.Dormitory != null ? new
                            {
                                g.First().Room.Dormitory.Id,
                                g.First().Room.Dormitory.Name
                            } : null
                        } : null,
                        TotalElectric = g.Sum(u => u.Electric),
                        TotalWater = g.Sum(u => u.Water),
                        Students = g.First().Room.StudentRooms.Select(sr => new
                        {
                            sr.Student.Id,
                            sr.Student.Name,
                            sr.Student.Email
                        }).ToList()
                    })
                    .ToListAsync();

                var result = new
                {
                    Month = month,
                    Reports = monthlyReport
                };

                return Ok(result);
            }

            return BadRequest("Invalid month format. Use yyyy-MM");
        }

        // GET: api/utility/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetUtilityReading(int id)
        {
            var reading = await _context.UtilityReadings
                .Include(u => u.Room)
                .ThenInclude(r => r.Dormitory)
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    u.Id,
                    u.RoomId,
                    u.ReadingMonth,
                    u.Electric,
                    u.Water,
                    u.ImportedAt,
                    Room = u.Room != null ? new
                    {
                        u.Room.Id,
                        u.Room.Code,
                        u.Room.Capacity,
                        u.Room.Status,
                        u.Room.DormitoryId,
                        Dormitory = u.Room.Dormitory != null ? new
                        {
                            u.Room.Dormitory.Id,
                            u.Room.Dormitory.Name
                        } : null
                    } : null
                })
                .FirstOrDefaultAsync();

            if (reading == null)
            {
                return NotFound();
            }

            return Ok(reading);
        }

        // POST: api/utility
        [HttpPost]
        public async Task<ActionResult<UtilityReading>> CreateUtilityReading([FromBody] UtilityReadingCreateDTO readingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if reading for this month already exists
            var existingReading = await _context.UtilityReadings
                .FirstOrDefaultAsync(u => u.RoomId == readingDto.RoomId && u.ReadingMonth == readingDto.ReadingMonth);

            if (existingReading != null)
            {
                return BadRequest($"Đã có chỉ số cho phòng này trong tháng {readingDto.ReadingMonth:yyyy-MM}");
            }

            var reading = new UtilityReading
            {
                RoomId = readingDto.RoomId,
                ReadingMonth = readingDto.ReadingMonth,
                Electric = readingDto.Electric,
                Water = readingDto.Water,
                ImportedAt = DateTime.UtcNow
            };

            _context.UtilityReadings.Add(reading);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUtilityReading), new { id = reading.Id }, reading);
        }

        // PUT: api/utility/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUtilityReading(int id, [FromBody] UtilityReadingUpdateDTO readingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reading = await _context.UtilityReadings.FindAsync(id);
            if (reading == null)
            {
                return NotFound();
            }

            reading.RoomId = readingDto.RoomId;
            reading.ReadingMonth = readingDto.ReadingMonth;
            reading.Electric = readingDto.Electric;
            reading.Water = readingDto.Water;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtilityReadingExists(id))
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

        // DELETE: api/utility/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUtilityReading(int id)
        {
            var reading = await _context.UtilityReadings.FindAsync(id);
            if (reading == null)
            {
                return NotFound();
            }

            _context.UtilityReadings.Remove(reading);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UtilityReadingExists(int id)
        {
            return _context.UtilityReadings.Any(e => e.Id == id);
        }
    }

    public class UtilityReadingCreateDTO
    {
        public int RoomId { get; set; }
        public DateOnly ReadingMonth { get; set; }
        public decimal Electric { get; set; }
        public decimal Water { get; set; }
    }

    public class UtilityReadingUpdateDTO
    {
        public int RoomId { get; set; }
        public DateOnly ReadingMonth { get; set; }
        public decimal Electric { get; set; }
        public decimal Water { get; set; }
    }
} 