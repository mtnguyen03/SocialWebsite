using Microsoft.AspNetCore.Mvc;
using SocialFrontEnd.Models;
using Newtonsoft.Json;

namespace SocialFrontEnd.Controllers
{
    public class ChatController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ChatController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            Users = new List<UserModel>();
        }

        public List<UserModel> Users { get; set; }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("default");
            var request = new HttpRequestMessage(HttpMethod.Get, "https://socialapi20241113093259.azurewebsites.net/api/Accounts/GetUser");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                Users = JsonConvert.DeserializeObject<List<UserModel>>(responseData);
                return View(Users); 
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Error fetching user data");
            }
        }
    }
}
