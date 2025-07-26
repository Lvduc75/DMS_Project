using Microsoft.AspNetCore.Mvc;
using DMS.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMS_FE.Controllers
{
    public class TransactionController : Controller
    {
        private readonly DormManagementContext _context;

        public TransactionController(DormManagementContext context)
        {
            _context = context;
        }

        // GET: /Transaction/Manage
        public async Task<IActionResult> Manage()
        {
            ViewData["Title"] = "Quản Lý Giao Dịch";
            var transactions = await _context.Transactions
                .Include(t => t.Fee)
                .ThenInclude(f => f.Student)
                .OrderByDescending(t => t.PaymentDate)
                .ToListAsync();
            return View(transactions);
        }

        // GET: /Transaction/Summary
        public async Task<IActionResult> Summary(string? fromDate, string? toDate)
        {
            ViewData["Title"] = "Tổng Kết Giao Dịch";

            var query = _context.Transactions.AsQueryable();

            if (!string.IsNullOrEmpty(fromDate) && DateOnly.TryParse(fromDate, out var from))
            {
                query = query.Where(t => t.PaymentDate >= from);
            }

            if (!string.IsNullOrEmpty(toDate) && DateOnly.TryParse(toDate, out var to))
            {
                query = query.Where(t => t.PaymentDate <= to);
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

            ViewBag.Summary = summary;
            return View();
        }

        // GET: /Transaction/Create
        public IActionResult Create()
        {
            ViewData["Title"] = "Tạo Giao Dịch Mới";
            ViewBag.Fees = _context.Fees
                .Include(f => f.Student)
                .Where(f => f.Status == "unpaid")
                .ToList();
            return View();
        }

        // POST: /Transaction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FeeId,PayerName,Amount")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                var fee = await _context.Fees.FindAsync(transaction.FeeId);
                if (fee == null)
                {
                    ModelState.AddModelError("FeeId", "Phí không tồn tại");
                    ViewBag.Fees = _context.Fees
                        .Include(f => f.Student)
                        .Where(f => f.Status == "unpaid")
                        .ToList();
                    return View(transaction);
                }

                transaction.PaymentDate = DateOnly.FromDateTime(DateTime.Now);
                _context.Add(transaction);

                // Update fee status to paid
                fee.Status = "paid";
                _context.Update(fee);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Manage));
            }
            ViewBag.Fees = _context.Fees
                .Include(f => f.Student)
                .Where(f => f.Status == "unpaid")
                .ToList();
            return View(transaction);
        }

        // GET: /Transaction/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var transaction = await _context.Transactions
                .Include(t => t.Fee)
                .ThenInclude(f => f.Student)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
                return NotFound();

            ViewData["Title"] = "Chi Tiết Giao Dịch";
            return View(transaction);
        }

        // POST: /Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Fee)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction != null)
            {
                // Revert fee status to unpaid
                transaction.Fee.Status = "unpaid";
                _context.Update(transaction.Fee);

                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Manage));
        }
    }
} 