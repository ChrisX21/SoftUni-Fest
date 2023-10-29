using Castle.Core.Smtp;
using System.Net;
using System.Net.Mail;

namespace Softuni_Fest.Services
{
    public class EmailSender : IEmailSender
    {
        public void Send(string from, string to, string subject, string messageText)
        {
            var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("cc7ab3acd43449", "684e8a1d899a1a"),
                EnableSsl = true
            };
            client.Send("otpusnatitemomcheta@gmail.com", "dani.d.game@gmail.com", "Hello world", "testbody");
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
