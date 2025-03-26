using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using YemekSepeti.Services;

public class EmailSender : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        using (var client = new SmtpClient("smtp.gmail.com", 587))
        {
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("murtuzorderverify@gmail.com", "tbmm fskk lecy omcy");

            var mailMessage = new MailMessage
            {
                From = new MailAddress("murtuzorderverify@gmail.com"),
                Subject = subject,
                Body = message,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            try
            {
                await client.SendMailAsync(mailMessage);
                Console.WriteLine("✅ Email başarıyla gönderildi!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Email gönderme başarısız: {ex.Message}");
            }
        }
    }
}
