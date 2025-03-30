using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YemekSepeti.DTO;
using YemekSepeti.Models;

namespace YemekSepeti.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : Controller
    {
        private readonly YemekSepetContext _context;
        private UserManager<IdentityUser<int>> _userManager;
        public NotificationController(YemekSepetContext context, UserManager<IdentityUser<int>> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet("getNotifications/{id}")]
        //[Authorize]
        public async Task<IActionResult> GetUserNotifications(int id)
            {
            var user = _userManager.FindByIdAsync(id.ToString()).Result;
            if(user == null)
            {
                return BadRequest("There is no user like that!");
            }
            List<NotificationDTO> notifications = _context.Notifications.Where(n => n.UserId == user.Id).Select(n => new NotificationDTO(){Message=n.Message,UserId=n.UserId,IsRead=n.IsRead,Id=n.Id,Date=n.Date}).ToList();
            return Ok(notifications);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> SetAsRead(int id)
        {
            Notification exactNotification = _context.Notifications.FirstOrDefault(n => n.Id == id);
            if(exactNotification == null)
            {
                return BadRequest("There is no notification with that id");
            }
            exactNotification.IsRead = true;
            _context.SaveChanges();
            return Ok("Notification set as read successfully");
        }
        [HttpGet("anyUnreadNotification/{id}")] 
        public async Task<IActionResult> AnyUnreadNotification(int id)
        {
            var user = _userManager.FindByIdAsync(id.ToString()).Result;
            if (user == null)
            {
                return BadRequest("There is no user like that!");
            }
            bool anyUnRead = _context.Notifications.Where(n => n.UserId == user.Id).Any(n => n.IsRead == false);

            return Ok(new { anyUnRead = anyUnRead });
        }
    }
}
