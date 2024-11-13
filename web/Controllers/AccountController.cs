using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BusinessObject;
using SocialFrontEnd.Helper;
using Newtonsoft.Json;
using System.Text;
using BusinessObject.Authen;
using SocialFrontEnd.Services.MailService;
using SocialFrontEnd.Services.OtpService;

namespace SocialFrontEnd.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOtpService _otpService;
        private readonly ISendGmailService _sendGmailService;

        // Constructor
        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IHttpClientFactory httpClientFactory,
            IOtpService otpService,
            ISendGmailService sendGmailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpClientFactory = httpClientFactory;
            _otpService = otpService ?? throw new ArgumentNullException(nameof(otpService));
            _sendGmailService = sendGmailService ?? throw new ArgumentNullException(nameof(sendGmailService));
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
                        SetSession(token, user.UserName, user.Email,user.Id);
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


        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult ResetPassword(string email)
        {
            var model = new ResetPasswordModel { Email = email };
            return View(model);
        }

        public IActionResult CheckOtp(string email)
        {
            var model = new CheckOtpModel { Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CheckOtp(CheckOtpModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var isOtpValid = _otpService.ValidateOtp(model.Email, model.Otp);
            if (!isOtpValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid OTP. Please try again.");
                return View(model); 
            }
            return Redirect($"/Account/ResetPassword?email={model.Email}");
          
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); 
            }

         
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User with this email does not exist.");
                return View(model); 
            }

          
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (resetToken == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to generate reset token.");
                return View(model); 
            }


            var result = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);
            if (!result.Succeeded) 
            {
                ModelState.AddModelError(string.Empty, "Failed to reset the password. Please try again.");
                return Redirect($"/Account/ResetPassword?email={model.Email}");
            }

            // Use TempData to store the success message
            TempData["SuccessMessage"] = "Password reset successful. Please log in with your new password.";

            return RedirectToAction("Login", "Account");
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

             
                var signUpModel = new SignUpModel
                {
                    FullName = fullName,
                    Email = email,
                    Password = password,
                    ConfirmPassword = confirmPassword
                };

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7055/api/Accounts/SignUp");

                
                    var response = await client.PostAsJsonAsync("SignUp", signUpModel);

                    if (response.IsSuccessStatusCode)
                    {
              
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

        private void SetSession(string token, string username, string userEmail, string Id)
        {
            HttpContext.Session.SetString("Id", Id);
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

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User with this email does not exist.");
                return View(model);
            }

            _otpService.GenerateOtp(model.Email, out var otp);

            MailContent content = new MailContent
            {
                To = model.Email,
                Subject = "OTP - Social",
                Body = $"<h2>Mail from Social</h2>" +
                $"Your OTP is: {otp}"
            };

            _sendGmailService.SendMail(content);

            return Redirect($"/Account/CheckOtp?email={model.Email}");
        }

    }

}
