using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class SendEmailService
    {
        public async Task SendAsync(MailMessage message)
        {
            try
            {
                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "serviceofed@gmail.com",
                        Password = "!qaz2wsx"
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                }

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task SendAsync(string destination, string subject, string body)
        {
            try
            {
                var msg = new MailMessage();

                msg.To.Add(new MailAddress(destination));
                msg.From = new MailAddress("serviceofed@gmail.com");
                msg.IsBodyHtml = true;
                msg.Subject = subject;
                msg.Body = body;
                msg.IsBodyHtml = true;
                await SendAsync(msg);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
