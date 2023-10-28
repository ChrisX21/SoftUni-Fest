using Castle.Core.Smtp;
using System.Net;
using System.Net.Mail;

namespace Softuni_Fest.Services
{
    public class EmailSender : IEmailSender
    {
        public void Send(string from, string to, string subject, string messageText)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("otpusnatitemomcheta@gmail.com", "snusoclock");
            smtpClient.EnableSsl = true;

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(from);
            mailMessage.To.Add(new MailAddress(to));
            mailMessage.Subject = subject;
            mailMessage.Body = messageText;

            smtpClient.Send(mailMessage);
        }

        public void Send(MailMessage message)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Send(message);
        }

        public void Send(IEnumerable<MailMessage> messages)
        {
            SmtpClient smtpClient = new SmtpClient();
            foreach (MailMessage message in messages)
            {
                smtpClient.Send(message);
            }
        }
    }
}
