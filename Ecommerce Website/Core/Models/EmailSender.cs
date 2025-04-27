using System.Net.Mail;
using System.Net;


namespace Ecommerce_Website.Core.Models
{
    public class EmailSender : Microsoft.AspNetCore.Identity.UI.Services.IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var fromEmail = "em55599555@gmail.com";
            var fromPasswerd = "pxxq znor xrxa jxfw";

            var message = new MailMessage();
            message.From = new MailAddress(fromEmail);
            message.To.Add(new MailAddress(email));
            message.Subject = subject;
            message.Body =$"<html><body>{htmlMessage} </body></html>";
            message.IsBodyHtml = true;


            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail , fromPasswerd),
                EnableSsl = true,
            };
            await smtpClient.SendMailAsync(message);
        }
    }

}
