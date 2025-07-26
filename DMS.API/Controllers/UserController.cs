using DMS.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DormManagementContext _context;
        public UserController(DormManagementContext context)
        {
            _context = context;
        }

        // GET: api/user
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // GET: api/user/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // GET: api/user/students
        [HttpGet("students")]
        public async Task<ActionResult<IEnumerable<object>>> GetStudents()
        {
            var students = await _context.Users
                .Where(u => u.Role == "Student")
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Email,
                    u.Phone,
                    u.Role
                })
                .ToListAsync();

            return Ok(students);
        }

        // POST: api/user
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                return Conflict("Email already exists");
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        // PUT: api/user/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] User user)
        {
            if (id != user.Id) return BadRequest();
            var exist = await _context.Users.FindAsync(id);
            if (exist == null) return NotFound();
            exist.Name = user.Name;
            exist.Email = user.Email;
            exist.Phone = user.Phone;
            exist.Password = user.Password;
            exist.Role = user.Role;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PATCH: api/user/{id}/profile - Update profile for student
        [HttpPatch("{id}/profile")]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] object profileData)
        {
            var exist = await _context.Users.FindAsync(id);
            if (exist == null) return NotFound();
            
            // Parse dynamic data
            var data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(profileData.ToString());
            
            if (data.ContainsKey("Name"))
                exist.Name = data["Name"].ToString();
            if (data.ContainsKey("Email"))
                exist.Email = data["Email"].ToString();
            if (data.ContainsKey("Phone"))
                exist.Phone = data["Phone"].ToString();
            if (data.ContainsKey("NewPassword") && !string.IsNullOrEmpty(data["NewPassword"].ToString()))
                exist.Password = data["NewPassword"].ToString();
            
            await _context.SaveChangesAsync();
            return Ok(exist);
        }

        // DELETE: api/user/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 