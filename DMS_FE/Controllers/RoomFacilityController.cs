using Microsoft.AspNetCore.Mvc;
using DMS_FE.Helpers;
using DMS_FE.Models;
using System.Text;
using Newtonsoft.Json;

namespace DMS_FE.Controllers
{
    public class RoomFacilityController : Controller
    {
        private readonly ApiHelper _apiHelper;

        public RoomFacilityController(ApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        // GET: /RoomFacility/Manage
        public async Task<IActionResult> Manage()
        {
            ViewData["Title"] = "Quản Lý Tiện Ích Phòng";
            
            var response = await _apiHelper.GetAsync("/api/roomfacility");
            if (response.IsSuccessStatusCode)
            {
                var roomFacilities = await response.Content.ReadFromJsonAsync<List<RoomFacilityModel>>();
                return View(roomFacilities ?? new List<RoomFacilityModel>());
            }
            
            return View(new List<RoomFacilityModel>());
        }

        // GET: /RoomFacility/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Title"] = "Thêm Tiện Ích Phòng";
            
            // Load rooms
            var roomsResponse = await _apiHelper.GetAsync("/api/room");
            if (roomsResponse.IsSuccessStatusCode)
            {
                var rooms = await roomsResponse.Content.ReadFromJsonAsync<List<RoomModel>>();
                ViewBag.Rooms = rooms ?? new List<RoomModel>();
            }
            
            // Load facilities
            var facilitiesResponse = await _apiHelper.GetAsync("/api/facility");
            if (facilitiesResponse.IsSuccessStatusCode)
            {
                var facilities = await facilitiesResponse.Content.ReadFromJsonAsync<List<FacilityModel>>();
                ViewBag.Facilities = facilities ?? new List<FacilityModel>();
            }
            
            return View();
        }

        // POST: /RoomFacility/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomFacilityModel model)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                var response = await _apiHelper.PostAsync("/api/roomfacility", content);
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Thêm tiện ích phòng thành công!";
                    return RedirectToAction(nameof(Manage));
                }
                
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, error);
            }
            
            // Reload data for dropdowns
            var roomsResponse = await _apiHelper.GetAsync("/api/room");
            if (roomsResponse.IsSuccessStatusCode)
            {
                var rooms = await roomsResponse.Content.ReadFromJsonAsync<List<RoomModel>>();
                ViewBag.Rooms = rooms ?? new List<RoomModel>();
            }
            
            var facilitiesResponse = await _apiHelper.GetAsync("/api/facility");
            if (facilitiesResponse.IsSuccessStatusCode)
            {
                var facilities = await facilitiesResponse.Content.ReadFromJsonAsync<List<FacilityModel>>();
                ViewBag.Facilities = facilities ?? new List<FacilityModel>();
            }
            
            return View(model);
        }

        // GET: /RoomFacility/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var response = await _apiHelper.GetAsync($"/api/roomfacility/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var roomFacility = await response.Content.ReadFromJsonAsync<RoomFacilityModel>();
            if (roomFacility == null)
                return NotFound();

            ViewData["Title"] = "Chỉnh Sửa Tiện Ích Phòng";
            
            // Load rooms
            var roomsResponse = await _apiHelper.GetAsync("/api/room");
            if (roomsResponse.IsSuccessStatusCode)
            {
                var rooms = await roomsResponse.Content.ReadFromJsonAsync<List<RoomModel>>();
                ViewBag.Rooms = rooms ?? new List<RoomModel>();
            }
            
            // Load facilities
            var facilitiesResponse = await _apiHelper.GetAsync("/api/facility");
            if (facilitiesResponse.IsSuccessStatusCode)
            {
                var facilities = await facilitiesResponse.Content.ReadFromJsonAsync<List<FacilityModel>>();
                ViewBag.Facilities = facilities ?? new List<FacilityModel>();
            }
            
            return View(roomFacility);
        }

        // POST: /RoomFacility/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RoomFacilityModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                var response = await _apiHelper.PutAsync($"/api/roomfacility/{id}", content);
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Cập nhật tiện ích phòng thành công!";
                    return RedirectToAction(nameof(Manage));
                }
                
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, error);
            }
            
            // Reload data for dropdowns
            var roomsResponse = await _apiHelper.GetAsync("/api/room");
            if (roomsResponse.IsSuccessStatusCode)
            {
                var rooms = await roomsResponse.Content.ReadFromJsonAsync<List<RoomModel>>();
                ViewBag.Rooms = rooms ?? new List<RoomModel>();
            }
            
            var facilitiesResponse = await _apiHelper.GetAsync("/api/facility");
            if (facilitiesResponse.IsSuccessStatusCode)
            {
                var facilities = await facilitiesResponse.Content.ReadFromJsonAsync<List<FacilityModel>>();
                ViewBag.Facilities = facilities ?? new List<FacilityModel>();
            }
            
            return View(model);
        }

        // GET: /RoomFacility/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var response = await _apiHelper.GetAsync($"/api/roomfacility/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var roomFacility = await response.Content.ReadFromJsonAsync<RoomFacilityModel>();
            if (roomFacility == null)
                return NotFound();

            ViewData["Title"] = "Chi Tiết Tiện Ích Phòng";
            return View(roomFacility);
        }

        // POST: /RoomFacility/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _apiHelper.DeleteAsync($"/api/roomfacility/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Xóa tiện ích phòng thành công!";
            }
            else
            {
                TempData["Error"] = "Có lỗi xảy ra khi xóa tiện ích phòng!";
            }
            
            return RedirectToAction(nameof(Manage));
        }
    }
} 