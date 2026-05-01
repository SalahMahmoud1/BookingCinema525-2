using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace Ecommerce525.Utilities
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("salahmahmoud8831@gmail.com", "jgrz ajvd krvv obuu")
            };
            var mail = new MailMessage(from: "salahmahmoud8831@gmail.com", to: email, subject, htmlMessage)
            {
                IsBodyHtml = true,
            }; 
            return client.SendMailAsync(mail);
        }
    
    }
}
