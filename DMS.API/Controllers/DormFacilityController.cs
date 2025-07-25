using Microsoft.AspNetCore.Mvc;
using DMS.Models.Entities;
using System.Linq;

namespace DMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DormFacilityController : ControllerBase
    {
        private readonly DormManagementContext _context;
        public DormFacilityController(DormManagementContext context)
        {
            _context = context;
        }

        // GET: api/DormFacility
        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _context.DormFacilities
                .Select(df => new {
                    df.Id,
                    df.DormitoryId,
                    DormitoryName = df.Dormitory.Name,
                    df.FacilityId,
                    FacilityName = df.Facility.Name,
                    df.Quantity
                })
                .ToList();
            return Ok(list);
        }

        // POST: api/DormFacility
        [HttpPost]
        public IActionResult Create([FromBody] DormFacility df)
        {
            if (df.Quantity < 0)
                return BadRequest("Số lượng phải >= 0");
            _context.DormFacilities.Add(df);
            _context.SaveChanges();
            return Ok(df);
        }

        // PUT: api/DormFacility/{id}
        [HttpPut("{id}")]
        public IActionResult Edit(int id, [FromBody] DormFacility df)
        {
            var existing = _context.DormFacilities.FirstOrDefault(x => x.Id == id);
            if (existing == null) return NotFound();
            if (df.Quantity < 0)
                return BadRequest("Số lượng phải >= 0");
            existing.DormitoryId = df.DormitoryId;
            existing.FacilityId = df.FacilityId;
            existing.Quantity = df.Quantity;
            _context.SaveChanges();
            return Ok(existing);
        }

        // DELETE: api/DormFacility/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var df = _context.DormFacilities.FirstOrDefault(x => x.Id == id);
            if (df == null) return NotFound();
            _context.DormFacilities.Remove(df);
            _context.SaveChanges();
            return Ok();
        }
    }
} 