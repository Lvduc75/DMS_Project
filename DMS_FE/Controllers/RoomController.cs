using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DMS_FE.Helpers;
using DMS_FE.Models;

namespace DMS_FE.Controllers
{
    public class RoomController : Controller
    {
        private readonly ApiHelper _apiHelper;
        public RoomController(ApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<IActionResult> Manage(int? dormitoryId)
        {
            var response = await _apiHelper.GetAsync($"/api/Room{(dormitoryId.HasValue ? "?dormitoryId=" + dormitoryId : "")}");
            var json = await response.Content.ReadAsStringAsync();
            var rooms = JsonSerializer.Deserialize<List<RoomModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Lấy danh sách dormitory cho filter
            var dormRes = await _apiHelper.GetAsync("/api/Dormitory");
            var dormJson = await dormRes.Content.ReadAsStringAsync();
            var dorms = JsonSerializer.Deserialize<List<DormitoryModel>>(dormJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var vm = new RoomManageViewModel
            {
                Rooms = rooms,
                Dormitories = dorms,
                SelectedDormitoryId = dormitoryId
            };
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var dormRes = await _apiHelper.GetAsync("/api/Dormitory");
            var dormJson = await dormRes.Content.ReadAsStringAsync();
            var dorms = System.Text.Json.JsonSerializer.Deserialize<List<DormitorySimpleViewModel>>(dormJson, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var vm = new RoomCreateViewModel
            {
                Dormitories = dorms
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoomCreateViewModel model)
        {
            var content = new System.Net.Http.StringContent(System.Text.Json.JsonSerializer.Serialize(model), System.Text.Encoding.UTF8, "application/json");
            await _apiHelper.PostAsync("/api/Room", content);
            return RedirectToAction("Manage");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _apiHelper.GetAsync("/api/Room");
            var json = await response.Content.ReadAsStringAsync();
            var rooms = System.Text.Json.JsonSerializer.Deserialize<List<RoomEditViewModel>>(json, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var room = rooms.FirstOrDefault(r => r.Id == id);
            if (room == null) return NotFound();
            var dormRes = await _apiHelper.GetAsync("/api/Dormitory");
            var dormJson = await dormRes.Content.ReadAsStringAsync();
            var dorms = System.Text.Json.JsonSerializer.Deserialize<List<DormitorySimpleViewModel>>(dormJson, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            room.Dormitories = dorms;
            return View(room);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoomModel model)
        {
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            await _apiHelper.PutAsync($"/api/Room/{model.Id}", content);
            return RedirectToAction("Manage");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiHelper.GetAsync("/api/Room");
            var json = await response.Content.ReadAsStringAsync();
            var rooms = JsonSerializer.Deserialize<List<RoomModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var room = rooms.FirstOrDefault(r => r.Id == id);
            if (room == null) return NotFound();
            return View(room);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int Id)
        {
            var response = await _apiHelper.DeleteAsync($"/api/Room/{Id}");
            if (!response.IsSuccessStatusCode)
            {
                var msg = await response.Content.ReadAsStringAsync();
                TempData["DeleteError"] = msg;
                return RedirectToAction("Delete", new { id = Id });
            }
            return RedirectToAction("Manage");
        }
    }
} 