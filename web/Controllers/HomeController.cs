using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using SocialFrontEnd.Models;
using Microsoft.AspNetCore.Identity;
using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using DataAccess.Helpers;

namespace SocialFrontEnd.Controllers
{
    [Authorize(AppRole.User)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger,
            UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
          
            var username = HttpContext.Session.GetString("Username");
            var email = HttpContext.Session.GetString("Email");
            ViewBag.Username = username;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
