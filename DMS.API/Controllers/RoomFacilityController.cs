using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DMS.Models.Entities;
using DMS.Models.DTOs;

namespace DMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomFacilityController : ControllerBase
    {
        private readonly DormManagementContext _context;

        public RoomFacilityController(DormManagementContext context)
        {
            _context = context;
        }

        // GET: api/roomfacility
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetRoomFacilities()
        {
            var roomFacilities = await _context.RoomFacilities
                .Include(rf => rf.Room)
                .ThenInclude(r => r.Dormitory)
                .Include(rf => rf.Facility)
                .OrderBy(rf => rf.Room.Code)
                .ThenBy(rf => rf.Facility.Name)
                .Select(rf => new
                {
                    rf.Id,
                    rf.RoomId,
                    rf.FacilityId,
                    rf.Quantity,
                    Room = rf.Room != null ? new
                    {
                        rf.Room.Id,
                        rf.Room.Code,
                        rf.Room.Capacity,
                        rf.Room.Status,
                        Dormitory = rf.Room.Dormitory != null ? new
                        {
                            rf.Room.Dormitory.Id,
                            rf.Room.Dormitory.Name
                        } : null
                    } : null,
                    Facility = rf.Facility != null ? new
                    {
                        rf.Facility.Id,
                        rf.Facility.Name,
                        rf.Facility.UnitPrice
                    } : null
                })
                .ToListAsync();
            return Ok(roomFacilities);
        }

        // GET: api/roomfacility/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetRoomFacility(int id)
        {
            var roomFacility = await _context.RoomFacilities
                .Include(rf => rf.Room)
                .ThenInclude(r => r.Dormitory)
                .Include(rf => rf.Facility)
                .Where(rf => rf.Id == id)
                .Select(rf => new
                {
                    rf.Id,
                    rf.RoomId,
                    rf.FacilityId,
                    rf.Quantity,
                    Room = rf.Room != null ? new
                    {
                        rf.Room.Id,
                        rf.Room.Code,
                        rf.Room.Capacity,
                        rf.Room.Status,
                        Dormitory = rf.Room.Dormitory != null ? new
                        {
                            rf.Room.Dormitory.Id,
                            rf.Room.Dormitory.Name
                        } : null
                    } : null,
                    Facility = rf.Facility != null ? new
                    {
                        rf.Facility.Id,
                        rf.Facility.Name,
                        rf.Facility.UnitPrice
                    } : null
                })
                .FirstOrDefaultAsync();

            if (roomFacility == null)
            {
                return NotFound();
            }
            return Ok(roomFacility);
        }

        // GET: api/roomfacility/room/5
        [HttpGet("room/{roomId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetRoomFacilitiesByRoom(int roomId)
        {
            var roomFacilities = await _context.RoomFacilities
                .Include(rf => rf.Room)
                .ThenInclude(r => r.Dormitory)
                .Include(rf => rf.Facility)
                .Where(rf => rf.RoomId == roomId)
                .OrderBy(rf => rf.Facility.Name)
                .Select(rf => new
                {
                    rf.Id,
                    rf.RoomId,
                    rf.FacilityId,
                    rf.Quantity,
                    Room = rf.Room != null ? new
                    {
                        rf.Room.Id,
                        rf.Room.Code,
                        rf.Room.Capacity,
                        rf.Room.Status,
                        Dormitory = rf.Room.Dormitory != null ? new
                        {
                            rf.Room.Dormitory.Id,
                            rf.Room.Dormitory.Name
                        } : null
                    } : null,
                    Facility = rf.Facility != null ? new
                    {
                        rf.Facility.Id,
                        rf.Facility.Name,
                        rf.Facility.UnitPrice
                    } : null
                })
                .ToListAsync();
            return Ok(roomFacilities);
        }

        // POST: api/roomfacility
        [HttpPost]
        public async Task<ActionResult<RoomFacility>> CreateRoomFacility([FromBody] RoomFacilityCreateDTO roomFacilityDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kiểm tra xem đã có facility này trong room chưa
            var existing = await _context.RoomFacilities
                .FirstOrDefaultAsync(rf => rf.RoomId == roomFacilityDto.RoomId && rf.FacilityId == roomFacilityDto.FacilityId);

            if (existing != null)
            {
                return BadRequest("Tiện ích này đã được thêm vào phòng trước đó");
            }

            var roomFacility = new RoomFacility
            {
                RoomId = roomFacilityDto.RoomId,
                FacilityId = roomFacilityDto.FacilityId,
                Quantity = roomFacilityDto.Quantity
            };

            _context.RoomFacilities.Add(roomFacility);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRoomFacility), new { id = roomFacility.Id }, roomFacility);
        }

        // PUT: api/roomfacility/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoomFacility(int id, [FromBody] RoomFacilityUpdateDTO roomFacilityDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var roomFacility = await _context.RoomFacilities.FindAsync(id);
            if (roomFacility == null)
            {
                return NotFound();
            }

            // Kiểm tra xem có conflict với facility khác không
            var existing = await _context.RoomFacilities
                .FirstOrDefaultAsync(rf => rf.RoomId == roomFacilityDto.RoomId && 
                                         rf.FacilityId == roomFacilityDto.FacilityId && 
                                         rf.Id != id);

            if (existing != null)
            {
                return BadRequest("Tiện ích này đã được thêm vào phòng trước đó");
            }

            roomFacility.RoomId = roomFacilityDto.RoomId;
            roomFacility.FacilityId = roomFacilityDto.FacilityId;
            roomFacility.Quantity = roomFacilityDto.Quantity;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomFacilityExists(id))
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

        // DELETE: api/roomfacility/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomFacility(int id)
        {
            var roomFacility = await _context.RoomFacilities.FindAsync(id);
            if (roomFacility == null)
            {
                return NotFound();
            }

            _context.RoomFacilities.Remove(roomFacility);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool RoomFacilityExists(int id)
        {
            return _context.RoomFacilities.Any(e => e.Id == id);
        }
    }
} 