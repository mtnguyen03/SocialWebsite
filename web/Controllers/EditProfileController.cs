using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using BusinessObject;
using Newtonsoft.Json;
using SocialFrontEnd.Models;
using System.Diagnostics;
using System.Text;

namespace SocialFrontEnd.Controllers
{
    public class EditProfileController : Controller
    {
        private readonly HttpClient _httpClient;
        public class ProfileResponse
        {
            [JsonProperty("value")]
            public List<User> Value { get; set; }
        }

        public EditProfileController()
        {
            _httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
        }
        public async Task<IActionResult> IndexAsync()
        {
            string id = HttpContext.Session.GetString("Id");
            if (id == null)
            {
                return RedirectToAction("Login", "Account");
            }
            //string str = "https://socialapi20241113093259.azurewebsites.net/odata/Profiles?filter=Id eq '" + id + "'";
            //string str = "https://socialapi20241113093259.azurewebsites.net/odata/Profiles('" + id + "')";
            string str = "https://socialapi20241113093259.azurewebsites.net/api/Profiles/" + id;
            HttpResponseMessage res = await _httpClient.GetAsync(str);
            if (!res.IsSuccessStatusCode)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            string rData = await res.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<User>(rData);
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(User user)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            string id = HttpContext.Session.GetString("Id");
            if (id == null)
            {
                return RedirectToAction("Login", "Account");
            }
            //   string str = "https://socialapi20241113093259.azurewebsites.net/odata/Profiles('" + id + "')";
            string str = "https://socialapi20241113093259.azurewebsites.net/api/Profiles/" + id;
            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage res = await _httpClient.PutAsync(str, data);
            if (!res.IsSuccessStatusCode)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            return RedirectToAction("Index");
        }
        public IActionResult MyProfile()
        {
            return View();
        }


    }
}
s