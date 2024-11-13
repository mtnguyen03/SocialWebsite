using BusinessObject;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Mvc;



namespace SocialApi.Controllers
{
    [Route("api/Profiles")]
    [ApiController]
    public class EditProfileController : ControllerBase
    {
        private readonly IProfileRepository _profileRepository;

        public EditProfileController(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        // GET: api/Profiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            var list = await _profileRepository.GetUsers();
            return Ok(list);
        }

        // GET: api/Profiles/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(string id)
        {
            var user = await _profileRepository.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Profiles/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exist = await _profileRepository.GetUserById(id);
            if (exist == null)
            {
                return NotFound();
            }
             user.Id = id;
            await _profileRepository.Update(user);
            return Ok();
        }

        // POST: api/Profiles
        [HttpPost]
        public async Task<ActionResult<User>> Post([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _profileRepository.Add(user);
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        // DELETE: api/Profiles/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var exist = await _profileRepository.GetUserById(id);
            if (exist == null)
            {
                return NotFound();
            }

            await _profileRepository.Delete(exist);
            return NoContent();
        }



    }
}
