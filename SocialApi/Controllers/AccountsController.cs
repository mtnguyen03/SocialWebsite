using BookApi.Repositories;
using BusinessObject.Authen;
using Microsoft.AspNetCore.Mvc;

namespace SocialApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IAccountRepository arepository;

        public AccountsController(IAccountRepository repository)
        {
            arepository = repository;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            var result = await arepository.SignUp(model);
            if (result.Succeeded)
            {
                return Ok(result.Succeeded);
            }

            return Content("SignUp Failed.");
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            var result = await arepository.SignIn(model);
            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }
            return Ok(result);
        }
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            var result = await arepository.GetUsers();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        
    }
}
