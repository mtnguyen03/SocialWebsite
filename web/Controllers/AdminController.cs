using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialFrontEnd.Models;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace SocialFrontEnd.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var token = HttpContext.Session.GetString("Token");
                
                client.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);
                
                var response = await client.GetAsync("https://localhost:7055/api/Admin/users");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var users = JsonConvert.DeserializeObject<List<UserModel>>(content);
                    return View(users);
                }
                return View(new List<UserModel>());
            }
            catch (Exception ex)
            {
                // Log error
                return View(new List<UserModel>());
            }
        }

        public async Task<IActionResult> UserDetails(string email)
        {
            var client = _httpClientFactory.CreateClient("default");
            var response = await client.GetAsync($"https://localhost:7055/api/Admin/users/{email}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserModel>(content);
                return View(user);
            }
            return NotFound();
        }

        public async Task<IActionResult> Edit(string email)
        {
            var client = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var response = await client.GetAsync($"https://localhost:7055/api/Admin/users/{email}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserModel>(content);
                return View(user);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserModel user)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient();
                var token = HttpContext.Session.GetString("Token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await client.PutAsync("https://localhost:7055/api/Admin/users", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Dashboard));
                }
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string email)
        {
            var client = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var response = await client.DeleteAsync($"https://localhost:7055/api/Admin/users/{email}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Dashboard));
            }

            return NotFound();
        }
    }
}
