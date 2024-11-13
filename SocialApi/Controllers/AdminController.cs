using BusinessObject;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SocialApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try 
            {
                var users = await _adminRepository.GetAllUsers();
                if(users == null) return NotFound();
                return Ok(users);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("users/{email}")]
        public async Task<IActionResult> GetUser(string email)
        {
            var user = await _adminRepository.GetUserByEmail(email);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPut("users")]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            var result = await _adminRepository.UpdateUser(user);
            if (!result) return BadRequest();
            return Ok();
        }

        [HttpDelete("users/{email}")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var result = await _adminRepository.DeleteUser(email);
            if (!result) return BadRequest();
            return Ok();
        }
    }
}
