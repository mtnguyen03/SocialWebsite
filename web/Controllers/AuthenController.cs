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

namespace SocialFrontEnd.Controllers
{
    public class AuthenController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;


        // Constructor
        public AuthenController(UserManager<User> userManager
            , SignInManager<User> signInManager
           )
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }

        // GET: /Authen/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Authen/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var loginData = new { email, password };  // Ensure correct usage of variables
            var jsonData = JsonConvert.SerializeObject(loginData);

            using (var client = new HttpClient())
            {
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
                            HttpContext.Session.SetString("JwtToken", token);
                            HttpContext.Session.SetString("Username", user.UserName);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "User not found.");
                            return View();
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, $"Error decoding JWT: {ex.Message}");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Login failed. Please check your credentials.");
                    return View();
                }
            }
        }

        // GET: /Authen/SignUp
        public IActionResult SignUp()
        {
            return View();
        }

        // POST: /Authen/SignUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(string email, string password, string confirmPassword)
        {
            if (ModelState.IsValid)
            {
                if (password != confirmPassword)
                {
                    ModelState.AddModelError(string.Empty, "Passwords do not match.");
                    return View();
                }

                var user = new User { UserName = email, Email = email };
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");  // Redirect after successful registration
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View();
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Authen");
        }

    }


}
