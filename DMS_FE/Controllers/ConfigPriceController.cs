using Microsoft.AspNetCore.Mvc;
using DMS.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMS_FE.Controllers
{
    public class ConfigPriceController : Controller
    {
        private readonly DormManagementContext _context;

        public ConfigPriceController(DormManagementContext context)
        {
            _context = context;
        }

        // GET: /ConfigPrice/Manage
        public async Task<IActionResult> Manage()
        {
            ViewData["Title"] = "Cấu hình Giá";
            var configs = await _context.ConfigPrices.ToListAsync();
            return View(configs);
        }

        // GET: /ConfigPrice/Initialize
        public IActionResult Initialize()
        {
            ViewData["Title"] = "Khởi tạo Giá Mặc định";
            return View();
        }

        // POST: /ConfigPrice/Initialize
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InitializeConfirmed()
        {
            // Check if already initialized
            if (await _context.ConfigPrices.AnyAsync())
            {
                TempData["Error"] = "Giá đã được khởi tạo trước đó";
                return RedirectToAction(nameof(Manage));
            }

            var defaultPrices = new List<ConfigPrice>
            {
                new ConfigPrice { Type = "room", UnitPrice = 1500000, EffectiveFrom = DateOnly.FromDateTime(DateTime.Today) }, // 1.5M VND per month
                new ConfigPrice { Type = "electricity", UnitPrice = 3500, EffectiveFrom = DateOnly.FromDateTime(DateTime.Today) }, // 3,500 VND per kWh
                new ConfigPrice { Type = "water", UnitPrice = 15000, EffectiveFrom = DateOnly.FromDateTime(DateTime.Today) }, // 15,000 VND per m3
            };

            _context.ConfigPrices.AddRange(defaultPrices);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã khởi tạo giá mặc định thành công";
            return RedirectToAction(nameof(Manage));
        }

        // GET: /ConfigPrice/Create
        public IActionResult Create()
        {
            ViewData["Title"] = "Thêm Cấu hình Giá Mới";
            return View();
        }

        // POST: /ConfigPrice/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Type,UnitPrice")] ConfigPrice configPrice)
        {
            if (ModelState.IsValid)
            {
                // Check if type already exists
                var existingConfig = await _context.ConfigPrices.FirstOrDefaultAsync(c => c.Type == configPrice.Type);
                if (existingConfig != null)
                {
                    ModelState.AddModelError("Type", $"Loại '{configPrice.Type}' đã tồn tại");
                    return View(configPrice);
                }

                // Set EffectiveFrom to today if not provided
                configPrice.EffectiveFrom = DateOnly.FromDateTime(DateTime.Today);

                _context.Add(configPrice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Manage));
            }
            return View(configPrice);
        }

        // GET: /ConfigPrice/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var configPrice = await _context.ConfigPrices.FindAsync(id);
            if (configPrice == null)
                return NotFound();

            ViewData["Title"] = "Chỉnh Sửa Cấu hình Giá";
            return View(configPrice);
        }

        // POST: /ConfigPrice/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,UnitPrice")] ConfigPrice configPrice)
        {
            if (id != configPrice.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.ConfigPrices.FindAsync(id);
                    if (existing == null)
                        return NotFound();

                    existing.Type = configPrice.Type;
                    existing.UnitPrice = configPrice.UnitPrice;

                    _context.Update(existing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConfigPriceExists(configPrice.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Manage));
            }
            return View(configPrice);
        }

        // POST: /ConfigPrice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var configPrice = await _context.ConfigPrices.FindAsync(id);
            if (configPrice != null)
            {
                _context.ConfigPrices.Remove(configPrice);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Manage));
        }

        private bool ConfigPriceExists(int id)
        {
            return _context.ConfigPrices.Any(e => e.Id == id);
        }
    }
} 