using Microsoft.AspNetCore.Mvc;
using DMS.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigPriceController : ControllerBase
    {
        private readonly DormManagementContext _context;

        public ConfigPriceController(DormManagementContext context)
        {
            _context = context;
        }

        // GET: /api/configprice
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var configs = await _context.ConfigPrices.ToListAsync();
            return Ok(configs);
        }

        // GET: /api/configprice/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var config = await _context.ConfigPrices.FirstOrDefaultAsync(c => c.Id == id);
            if (config == null) 
                return NotFound();
            
            return Ok(config);
        }

        // GET: /api/configprice/type/{type}
        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetByType(string type)
        {
            var config = await _context.ConfigPrices.FirstOrDefaultAsync(c => c.Type == type);
            if (config == null) 
                return NotFound();
            
            return Ok(config);
        }

        // POST: /api/configprice
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ConfigPrice configPrice)
        {
            // Check if type already exists
            var existingConfig = await _context.ConfigPrices.FirstOrDefaultAsync(c => c.Type == configPrice.Type);
            if (existingConfig != null)
            {
                return BadRequest($"Configuration for type '{configPrice.Type}' already exists");
            }

            _context.ConfigPrices.Add(configPrice);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetById), new { id = configPrice.Id }, configPrice);
        }

        // PUT: /api/configprice/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ConfigPrice configPrice)
        {
            var existing = await _context.ConfigPrices.FirstOrDefaultAsync(c => c.Id == id);
            if (existing == null) 
                return NotFound();

            existing.Type = configPrice.Type;
            existing.UnitPrice = configPrice.UnitPrice;

            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        // DELETE: /api/configprice/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var config = await _context.ConfigPrices.FirstOrDefaultAsync(c => c.Id == id);
            if (config == null) 
                return NotFound();

            _context.ConfigPrices.Remove(config);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        // POST: /api/configprice/initialize
        [HttpPost("initialize")]
        public async Task<IActionResult> InitializeDefaultPrices()
        {
            // Check if already initialized
            if (await _context.ConfigPrices.AnyAsync())
            {
                return BadRequest("Prices have already been initialized");
            }

            var defaultPrices = new List<ConfigPrice>
            {
                new ConfigPrice { Type = "room", UnitPrice = 1500000M, EffectiveFrom = DateOnly.FromDateTime(DateTime.Today) }, // 1.5M VND per month
                new ConfigPrice { Type = "electricity", UnitPrice = 3500M, EffectiveFrom = DateOnly.FromDateTime(DateTime.Today) }, // 3,500 VND per kWh
                new ConfigPrice { Type = "water", UnitPrice = 15000M, EffectiveFrom = DateOnly.FromDateTime(DateTime.Today) }, // 15,000 VND per m3
            };

            _context.ConfigPrices.AddRange(defaultPrices);
            await _context.SaveChangesAsync();

            return Ok(defaultPrices);
        }
    }
} 