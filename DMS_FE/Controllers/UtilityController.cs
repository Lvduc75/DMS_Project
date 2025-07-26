using Microsoft.AspNetCore.Mvc;
using DMS_FE.Helpers;
using DMS_FE.Models;
using System.Text;
using Newtonsoft.Json;

namespace DMS_FE.Controllers
{
    public class UtilityController : Controller
    {
        private readonly ApiHelper _apiHelper;

        public UtilityController(ApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        // GET: /Utility/Manage
        public async Task<IActionResult> Manage()
        {
            ViewData["Title"] = "Quản Lý Chỉ Số Tiện ích";
            
            var response = await _apiHelper.GetAsync("/api/utility");
            if (response.IsSuccessStatusCode)
            {
                var readings = await response.Content.ReadFromJsonAsync<List<UtilityReadingModel>>();
                return View(readings ?? new List<UtilityReadingModel>());
            }
            
            return View(new List<UtilityReadingModel>());
        }

        // GET: /Utility/Bill
        public async Task<IActionResult> Bill()
        {
            ViewData["Title"] = "Tính Hóa Đơn Tiện ích";
            
            var response = await _apiHelper.GetAsync("/api/utility/bill");
            if (response.IsSuccessStatusCode)
            {
                var rooms = await response.Content.ReadFromJsonAsync<List<UtilityRoomModel>>();
                return View(rooms ?? new List<UtilityRoomModel>());
            }
            
            return View(new List<UtilityRoomModel>());
        }

        // GET: /Utility/History
        public async Task<IActionResult> History(int? roomId, string? month)
        {
            ViewData["Title"] = "Lịch Sử Tiêu Thụ";
            
            var queryParams = new List<string>();
            if (roomId.HasValue)
                queryParams.Add($"roomId={roomId}");
            if (!string.IsNullOrEmpty(month))
                queryParams.Add($"month={month}");
            
            var url = "/api/utility/history";
            if (queryParams.Any())
                url += "?" + string.Join("&", queryParams);
            
            var response = await _apiHelper.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var readings = await response.Content.ReadFromJsonAsync<List<UtilityReadingModel>>();
                ViewBag.Readings = readings ?? new List<UtilityReadingModel>();
            }
            else
            {
                ViewBag.Readings = new List<UtilityReadingModel>();
            }
            
            // Load rooms for filter
            var roomsResponse = await _apiHelper.GetAsync("/api/room");
            if (roomsResponse.IsSuccessStatusCode)
            {
                var rooms = await roomsResponse.Content.ReadFromJsonAsync<List<UtilityRoomModel>>();
                ViewBag.Rooms = rooms ?? new List<UtilityRoomModel>();
            }
            
            return View();
        }

        // GET: /Utility/Monthly
        public async Task<IActionResult> Monthly(string? month)
        {
            ViewData["Title"] = "Báo Cáo Tháng";
            
            if (string.IsNullOrEmpty(month))
            {
                month = DateTime.Now.ToString("yyyy-MM");
            }
            
            var response = await _apiHelper.GetAsync($"/api/utility/monthly?month={month}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<MonthlyReportModel>();
                ViewBag.MonthlyReport = result?.Reports ?? new List<MonthlyReportItemModel>();
                ViewBag.SelectedMonth = month;
            }
            else
            {
                ViewBag.MonthlyReport = new List<MonthlyReportItemModel>();
                ViewBag.SelectedMonth = month;
            }
            
            return View();
        }

        // GET: /Utility/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Title"] = "Thêm Chỉ Số Mới";
            
            // Load rooms
            var response = await _apiHelper.GetAsync("/api/room");
            if (response.IsSuccessStatusCode)
            {
                var rooms = await response.Content.ReadFromJsonAsync<List<UtilityRoomModel>>();
                ViewBag.Rooms = rooms ?? new List<UtilityRoomModel>();
            }
            
            return View();
        }

        // POST: /Utility/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UtilityReadingModel model)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                var response = await _apiHelper.PostAsync("/api/utility", content);
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Thêm chỉ số thành công!";
                    return RedirectToAction(nameof(Manage));
                }
                
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, error);
            }
            
            // Reload rooms
            var roomsResponse = await _apiHelper.GetAsync("/api/room");
            if (roomsResponse.IsSuccessStatusCode)
            {
                var rooms = await roomsResponse.Content.ReadFromJsonAsync<List<UtilityRoomModel>>();
                ViewBag.Rooms = rooms ?? new List<UtilityRoomModel>();
            }
            
            return View(model);
        }

        // GET: /Utility/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var response = await _apiHelper.GetAsync($"/api/utility/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var reading = await response.Content.ReadFromJsonAsync<UtilityReadingModel>();
            if (reading == null)
                return NotFound();

            ViewData["Title"] = "Chỉnh Sửa Chỉ Số";
            
            // Load rooms
            var roomsResponse = await _apiHelper.GetAsync("/api/room");
            if (roomsResponse.IsSuccessStatusCode)
            {
                var rooms = await roomsResponse.Content.ReadFromJsonAsync<List<UtilityRoomModel>>();
                ViewBag.Rooms = rooms ?? new List<UtilityRoomModel>();
            }
            
            return View(reading);
        }

        // POST: /Utility/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UtilityReadingModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                var response = await _apiHelper.PutAsync($"/api/utility/{id}", content);
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Cập nhật chỉ số thành công!";
                    return RedirectToAction(nameof(Manage));
                }
                
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, error);
            }
            
            // Reload rooms
            var roomsResponse = await _apiHelper.GetAsync("/api/room");
            if (roomsResponse.IsSuccessStatusCode)
            {
                var rooms = await roomsResponse.Content.ReadFromJsonAsync<List<UtilityRoomModel>>();
                ViewBag.Rooms = rooms ?? new List<UtilityRoomModel>();
            }
            
            return View(model);
        }

        // GET: /Utility/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var response = await _apiHelper.GetAsync($"/api/utility/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var reading = await response.Content.ReadFromJsonAsync<UtilityReadingModel>();
            if (reading == null)
                return NotFound();

            ViewData["Title"] = "Chi Tiết Chỉ Số";
            return View(reading);
        }

        // POST: /Utility/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _apiHelper.DeleteAsync($"/api/utility/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Xóa chỉ số thành công!";
            }
            else
            {
                TempData["Error"] = "Có lỗi xảy ra khi xóa chỉ số!";
            }
            
            return RedirectToAction(nameof(Manage));
        }
    }
} 