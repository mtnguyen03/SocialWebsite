using BusinessObject;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;



namespace SocialApi.Controllers
{
    [Route("odata/Notifications")]
    [ApiController]
    public class NotificationController : ODataController
    {
        private readonly INotificationRepository _NotificationRepository;

        public NotificationController(INotificationRepository NotificationRepository)
        {
            _NotificationRepository = NotificationRepository;
        }

        // GET: api/Notifications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> Get()
        {
            var list = await _NotificationRepository.GetNotifications();
            return Ok(list);
        }

        // GET: api/Notifications/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> Get(string id)
        {
            var Notification = await _NotificationRepository.GetNotificationsByUserId(id);

            if (Notification == null)
            {
                return NotFound();
            }

            return Ok(Notification);
        }

     
        // POST: api/Notifications
        [HttpPost]
        public async Task<ActionResult<Notification>> Post([FromBody] Notification Notification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _NotificationRepository.Add(Notification);
            return CreatedAtAction(nameof(Get), new { id = Notification.NotificationID }, Notification);
        }

        // DELETE: api/Notifications/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var notification = await _NotificationRepository.GetNotificationById(id); // Retrieve a single notification by ID
            if (notification == null)
            {
                return NotFound();
            }

            await _NotificationRepository.Delete(id); 
            return NoContent();
        }




    }
}
