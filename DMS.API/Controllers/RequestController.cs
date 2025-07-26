using Microsoft.AspNetCore.Mvc;
using DMS.Models.Entities;
using DMS.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly DormManagementContext _context;

        public RequestController(DormManagementContext context)
        {
            _context = context;
        }

        // POST: /api/request
        [HttpPost]
        public async Task<IActionResult> CreateRequest([FromBody] RequestCreateDTO requestDto)
        {
            // Validate student exists
            var student = await _context.Users.FirstOrDefaultAsync(u => u.Id == requestDto.StudentId);
            if (student == null)
                return BadRequest("Student not found");

            var request = new Request
            {
                StudentId = requestDto.StudentId,
                Type = requestDto.Type,
                Description = requestDto.Description,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };

            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            var resultDto = new RequestResponseDTO
            {
                Id = request.Id,
                StudentId = request.StudentId,
                StudentName = student.Name,
                ManagerId = request.ManagerId,
                ManagerName = null,
                Type = request.Type,
                Description = request.Description,
                Status = request.Status,
                CreatedAt = request.CreatedAt
            };

            return CreatedAtAction(nameof(GetRequest), new { id = request.Id }, resultDto);
        }

        // GET: /api/request
        [HttpGet]
        public async Task<IActionResult> GetAllRequests()
        {
            var requests = await _context.Requests
                .Include(r => r.Student)
                .Include(r => r.Manager)
                .Select(r => new RequestResponseDTO
                {
                    Id = r.Id,
                    StudentId = r.StudentId,
                    StudentName = r.Student.Name,
                    ManagerId = r.ManagerId,
                    ManagerName = r.Manager != null ? r.Manager.Name : null,
                    Type = r.Type,
                    Description = r.Description,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt
                })
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return Ok(requests);
        }

        // GET: /api/request/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequest(int id)
        {
            var request = await _context.Requests
                .Include(r => r.Student)
                .Include(r => r.Manager)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
                return NotFound();

            var resultDto = new RequestResponseDTO
            {
                Id = request.Id,
                StudentId = request.StudentId,
                StudentName = request.Student.Name,
                ManagerId = request.ManagerId,
                ManagerName = request.Manager != null ? request.Manager.Name : null,
                Type = request.Type,
                Description = request.Description,
                Status = request.Status,
                CreatedAt = request.CreatedAt
            };

            return Ok(resultDto);
        }

        // GET: /api/request/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRequestsByUserId(int userId)
        {
            var requests = await _context.Requests
                .Include(r => r.Student)
                .Include(r => r.Manager)
                .Where(r => r.StudentId == userId)
                .Select(r => new RequestResponseDTO
                {
                    Id = r.Id,
                    StudentId = r.StudentId,
                    StudentName = r.Student.Name,
                    ManagerId = r.ManagerId,
                    ManagerName = r.Manager != null ? r.Manager.Name : null,
                    Type = r.Type,
                    Description = r.Description,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt
                })
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return Ok(requests);
        }

        // GET: /api/request/status/{status}
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetRequestsByStatus(string status)
        {
            var requests = await _context.Requests
                .Include(r => r.Student)
                .Include(r => r.Manager)
                .Where(r => r.Status == status)
                .Select(r => new RequestResponseDTO
                {
                    Id = r.Id,
                    StudentId = r.StudentId,
                    StudentName = r.Student.Name,
                    ManagerId = r.ManagerId,
                    ManagerName = r.Manager != null ? r.Manager.Name : null,
                    Type = r.Type,
                    Description = r.Description,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt
                })
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return Ok(requests);
        }

        // PUT: /api/request/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRequest(int id, [FromBody] RequestUpdateDTO requestDto)
        {
            var request = await _context.Requests
                .Include(r => r.Student)
                .Include(r => r.Manager)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
                return NotFound();

            // Validate manager if provided
            if (requestDto.ManagerId.HasValue)
            {
                var manager = await _context.Users.FirstOrDefaultAsync(u => u.Id == requestDto.ManagerId.Value);
                if (manager == null)
                    return BadRequest("Manager not found");
            }

            request.Type = requestDto.Type;
            request.Description = requestDto.Description;
            request.Status = requestDto.Status;
            request.ManagerId = requestDto.ManagerId;

            await _context.SaveChangesAsync();

            var resultDto = new RequestResponseDTO
            {
                Id = request.Id,
                StudentId = request.StudentId,
                StudentName = request.Student.Name,
                ManagerId = request.ManagerId,
                ManagerName = request.Manager != null ? request.Manager.Name : null,
                Type = request.Type,
                Description = request.Description,
                Status = request.Status,
                CreatedAt = request.CreatedAt
            };

            return Ok(resultDto);
        }

        // PUT: /api/request/{id}/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateRequestStatus(int id, [FromBody] string status)
        {
            var request = await _context.Requests
                .Include(r => r.Student)
                .Include(r => r.Manager)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
                return NotFound();

            request.Status = status;
            await _context.SaveChangesAsync();

            var resultDto = new RequestResponseDTO
            {
                Id = request.Id,
                StudentId = request.StudentId,
                StudentName = request.Student.Name,
                ManagerId = request.ManagerId,
                ManagerName = request.Manager != null ? request.Manager.Name : null,
                Type = request.Type,
                Description = request.Description,
                Status = request.Status,
                CreatedAt = request.CreatedAt
            };

            return Ok(resultDto);
        }

        // DELETE: /api/request/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.Requests.FirstOrDefaultAsync(r => r.Id == id);
            if (request == null)
                return NotFound();

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
} 