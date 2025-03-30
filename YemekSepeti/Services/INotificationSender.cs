namespace YemekSepeti.Services
{
    public interface INotificationSender
    {
        Task SendNotification(string message, int userId);
    }
}
