using Microsoft.AspNetCore.Mvc;
using DMS_FE.Helpers;
using DMS_FE.Models;
using System.Text;
using Newtonsoft.Json;

namespace DMS_FE.Controllers
{
    public class FeeController : Controller
    {
        private readonly ApiHelper _apiHelper;

        public FeeController(ApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        // GET: /Fee/Manage
        public async Task<IActionResult> Manage()
        {
            ViewData["Title"] = "Quản Lý Phí";
            
            var response = await _apiHelper.GetAsync("/api/fee");
            if (response.IsSuccessStatusCode)
            {
                var fees = await response.Content.ReadFromJsonAsync<List<FeeModel>>();
                return View(fees ?? new List<FeeModel>());
            }
            
            return View(new List<FeeModel>());
        }

        // GET: /Fee/Tracking
        public async Task<IActionResult> Tracking()
        {
            ViewData["Title"] = "Theo Dõi Thanh Toán";
            
            var response = await _apiHelper.GetAsync("/api/fee/tracking");
            if (response.IsSuccessStatusCode)
            {
                var fees = await response.Content.ReadFromJsonAsync<List<FeeModel>>();
                return View(fees ?? new List<FeeModel>());
            }
            
            return View(new List<FeeModel>());
        }

        // GET: /Fee/Overdue
        public async Task<IActionResult> Overdue()
        {
            ViewData["Title"] = "Phí Quá Hạn";
            
            var response = await _apiHelper.GetAsync("/api/fee/overdue");
            if (response.IsSuccessStatusCode)
            {
                var fees = await response.Content.ReadFromJsonAsync<List<FeeModel>>();
                return View(fees ?? new List<FeeModel>());
            }
            
            return View(new List<FeeModel>());
        }

        // GET: /Fee/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Title"] = "Tạo Phí Mới";
            
            // Lấy danh sách sinh viên từ API
            var response = await _apiHelper.GetAsync("/api/user/students");
            if (response.IsSuccessStatusCode)
            {
                var students = await response.Content.ReadFromJsonAsync<List<UserModel>>();
                ViewBag.Students = students ?? new List<UserModel>();
            }
            else
            {
                ViewBag.Students = new List<UserModel>();
            }
            
            return View();
        }

        // POST: /Fee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FeeModel model)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                var response = await _apiHelper.PostAsync("/api/fee", content);
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Tạo phí thành công!";
                    return RedirectToAction(nameof(Manage));
                }
                
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, error);
            }
            
            // Reload students for dropdown
            var studentsResponse = await _apiHelper.GetAsync("/api/user/students");
            if (studentsResponse.IsSuccessStatusCode)
            {
                var students = await studentsResponse.Content.ReadFromJsonAsync<List<UserModel>>();
                ViewBag.Students = students ?? new List<UserModel>();
            }
            
            return View(model);
        }

        // GET: /Fee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var response = await _apiHelper.GetAsync($"/api/fee/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var fee = await response.Content.ReadFromJsonAsync<FeeModel>();
            if (fee == null)
                return NotFound();

            ViewData["Title"] = "Chỉnh Sửa Phí";
            
            // Load students
            var studentsResponse = await _apiHelper.GetAsync("/api/user/students");
            if (studentsResponse.IsSuccessStatusCode)
            {
                var students = await studentsResponse.Content.ReadFromJsonAsync<List<UserModel>>();
                ViewBag.Students = students ?? new List<UserModel>();
            }
            
            return View(fee);
        }

        // POST: /Fee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FeeModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                var response = await _apiHelper.PutAsync($"/api/fee/{id}", content);
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Cập nhật phí thành công!";
                    return RedirectToAction(nameof(Manage));
                }
                
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, error);
            }
            
            // Reload students
            var studentsResponse = await _apiHelper.GetAsync("/api/user/students");
            if (studentsResponse.IsSuccessStatusCode)
            {
                var students = await studentsResponse.Content.ReadFromJsonAsync<List<UserModel>>();
                ViewBag.Students = students ?? new List<UserModel>();
            }
            
            return View(model);
        }

        // GET: /Fee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var response = await _apiHelper.GetAsync($"/api/fee/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var fee = await response.Content.ReadFromJsonAsync<FeeModel>();
            if (fee == null)
                return NotFound();

            ViewData["Title"] = "Chi Tiết Phí";
            return View(fee);
        }

        // POST: /Fee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _apiHelper.DeleteAsync($"/api/fee/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Xóa phí thành công!";
            }
            else
            {
                TempData["Error"] = "Có lỗi xảy ra khi xóa phí!";
            }
            
            return RedirectToAction(nameof(Manage));
        }
    }
} 