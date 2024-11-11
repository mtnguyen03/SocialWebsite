using Microsoft.AspNetCore.Mvc;

namespace SocialFrontEnd.Controllers
{
    public class EditProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MyProfile()
        {
            return View();
        }
    }
}
