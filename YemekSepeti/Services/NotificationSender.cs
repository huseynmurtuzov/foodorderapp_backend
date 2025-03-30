using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using YemekSepeti.Models;

namespace YemekSepeti.Services;

public class NotificationSender:INotificationSender
{
    private readonly YemekSepetContext _context;
    private UserManager<IdentityUser<int>> _userManager;
    public NotificationSender(YemekSepetContext context, UserManager<IdentityUser<int>> userManager)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task SendNotification(string message, int userId)
    {
        var user = _userManager.FindByIdAsync(userId.ToString()).Result;
        if (user != null)
        {
            Notification notification = new Notification()
            {
                UserId = userId,
                Message = message,
                User = user,
                Date = DateTime.Now,
                IsRead = false
            };
            _context.Notifications.Add(notification);
            _context.SaveChanges();
        }
    }
}
