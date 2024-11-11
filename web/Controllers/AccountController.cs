using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using SocialFrontEnd.Helper;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Http;
using Azure;
using BusinessObject.Authen;

namespace SocialFrontEnd.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpClientFactory _httpClientFactory;

        // Constructor
        public AccountController(UserManager<User> userManager
            , SignInManager<User> signInManager
            ,IHttpClientFactory httpClientFactory

           )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpClientFactory = httpClientFactory;
        }

      
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var loginData = new { email, password };
            var jsonData = JsonConvert.SerializeObject(loginData);

            var client = _httpClientFactory.CreateClient("default");

            var response = await client.PostAsync("https://localhost:7055/api/Accounts/SignIn",
                new StringContent(jsonData, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();

                try
                {
                    var decodedEmail = JwtHelper.GetEmailFromJwt(token);
                    var user = await _userManager.FindByEmailAsync(decodedEmail);

                    if (user != null)
                    {
                        SetSession(token, user.UserName, user.Email);
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "User not found.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error decoding JWT: {ex.Message}");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login failed. Please check your credentials.");
            }

            return View();
        }

        // GET: /Authen/SignUp
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(string fullName, string email, string password, string confirmPassword)
        {
            if (ModelState.IsValid)
            {
                if (password != confirmPassword)
                {
                    ModelState.AddModelError(string.Empty, "Passwords do not match.");
                    return View();
                }

                // Create SignUpModel
                var signUpModel = new SignUpModel
                {
                    FullName = fullName,
                    Email = email,
                    Password = password,
                    ConfirmPassword = confirmPassword
                };

                // Call the API to handle the registration
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7055/api/Accounts/SignUp");

                    // API call
                    var response = await client.PostAsJsonAsync("SignUp", signUpModel);

                    if (response.IsSuccessStatusCode)
                    {
                        // Handle success, you may want to redirect
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Sign-up failed. Please try again.");
                    }
                }
            }
            return View();
        }



        private void SetSession(string token, string username, string userEmail)
        {
            HttpContext.Session.SetString("JwtToken", token);
            HttpContext.Session.SetString("Username", username);
            HttpContext.Session.SetString("Email", userEmail);
        }
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

    }


}
