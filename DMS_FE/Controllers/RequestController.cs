using Microsoft.AspNetCore.Mvc;
using DMS_FE.Models;
using DMS_FE.Helpers;
using System.Text.Json;
using System.Text;

namespace DMS_FE.Controllers
{
    public class RequestController : Controller
    {
        private readonly ApiHelper _apiHelper;
        private readonly IConfiguration _configuration;

        public RequestController(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiHelper = new ApiHelper(configuration);
        }

        // GET: Request/Manage (Manager view - all requests)
        public async Task<IActionResult> Manage()
        {
            try
            {
                var response = await _apiHelper.GetAsync("api/request");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var requests = JsonSerializer.Deserialize<List<RequestModel>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return View(requests);
                }
                return View(new List<RequestModel>());
            }
            catch (Exception ex)
            {
                return View(new List<RequestModel>());
            }
        }

        // GET: Request/MyRequests (Student view - their own requests)
        public async Task<IActionResult> MyRequests(int? studentId = null)
        {
            try
            {
                var actualStudentId = studentId ?? 1; // Default to 1 if not provided
                var response = await _apiHelper.GetAsync($"api/request/user/{actualStudentId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var requests = JsonSerializer.Deserialize<List<RequestModel>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return View(requests);
                }
                return View(new List<RequestModel>());
            }
            catch (Exception ex)
            {
                return View(new List<RequestModel>());
            }
        }

        // GET: Request/Student (Simple route for students)
        public async Task<IActionResult> Student()
        {
            return await MyRequests(1); // Default student ID
        }

        // GET: Request/Create
        public IActionResult Create(int? studentId = null)
        {
            var model = new RequestCreateViewModel();
            if (studentId.HasValue)
            {
                model.StudentId = studentId.Value;
            }
            else
            {
                // Default student ID for testing - in real app, get from session
                model.StudentId = 1; // This should come from user session
            }
            return View(model);
        }

        // GET: Request/StudentCreate (Simple route for students)
        public IActionResult StudentCreate()
        {
            return Create(1); // Default student ID
        }

        // POST: Request/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RequestCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var requestData = new
                    {
                        StudentId = model.StudentId,
                        Type = model.Type,
                        Description = model.Description
                    };

                    var json = JsonSerializer.Serialize(requestData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await _apiHelper.PostAsync("api/request", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Request created successfully!";
                        return RedirectToAction("Requests", "Student");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to create request. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while creating the request.");
                }
            }
            return View(model);
        }

        // GET: Request/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _apiHelper.GetAsync($"api/request/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var request = JsonSerializer.Deserialize<RequestModel>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return View(request);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        // GET: Request/Approve/{id} (Manager only)
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                var response = await _apiHelper.GetAsync($"api/request/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var request = JsonSerializer.Deserialize<RequestModel>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return View(request);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        // POST: Request/Approve/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id, string status, int managerId)
        {
            try
            {
                // First get the current request to preserve existing data
                var getResponse = await _apiHelper.GetAsync($"api/request/{id}");
                if (getResponse.IsSuccessStatusCode)
                {
                    var responseContent = await getResponse.Content.ReadAsStringAsync();
                    var currentRequest = JsonSerializer.Deserialize<RequestModel>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    var updateData = new
                    {
                        Type = currentRequest.Type,
                        Description = currentRequest.Description,
                        Status = status,
                        ManagerId = managerId
                    };

                    var json = JsonSerializer.Serialize(updateData);
                    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await _apiHelper.PutAsync($"api/request/{id}", httpContent);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = $"Request {status.ToLower()} successfully!";
                        return RedirectToAction(nameof(Manage));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to update request status. Please try again.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the request.");
            }
            return RedirectToAction(nameof(Manage));
        }

        // GET: Request/Status/{status}
        public async Task<IActionResult> Status(string status)
        {
            try
            {
                var response = await _apiHelper.GetAsync($"api/request/status/{status}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var requests = JsonSerializer.Deserialize<List<RequestModel>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    ViewBag.Status = status;
                    return View(requests);
                }
                return View(new List<RequestModel>());
            }
            catch (Exception ex)
            {
                return View(new List<RequestModel>());
            }
        }

        // GET: Request/Statistics (for dashboard)
        public async Task<IActionResult> Statistics()
        {
            try
            {
                var response = await _apiHelper.GetAsync("api/request");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var requests = JsonSerializer.Deserialize<List<RequestModel>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    var statistics = new
                    {
                        TotalRequests = requests?.Count ?? 0,
                        PendingRequests = requests?.Count(r => r.Status?.ToLower() == "pending") ?? 0,
                        ApprovedRequests = requests?.Count(r => r.Status?.ToLower() == "approved") ?? 0,
                        RejectedRequests = requests?.Count(r => r.Status?.ToLower() == "rejected") ?? 0
                    };

                    return Json(statistics);
                }
                return Json(new { TotalRequests = 0, PendingRequests = 0, ApprovedRequests = 0, RejectedRequests = 0 });
            }
            catch (Exception ex)
            {
                return Json(new { TotalRequests = 0, PendingRequests = 0, ApprovedRequests = 0, RejectedRequests = 0 });
            }
        }
    }
} 