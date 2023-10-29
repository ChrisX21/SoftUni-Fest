using Castle.Core.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Softuni_Fest.Pages
{
    public class TestPageModel : PageModel
    {
        private readonly IEmailSender _emailSender;

        public TestPageModel(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        public void OnGet()
        {
            string from = "otpusnatitemomcheta@gmail.com";

            string to = "dani27game@gmail.com";

            string subject = "Hello from YourApp";

            string messageText = "This is a test email from YourApp.";

            _emailSender.Send(from, to, subject, messageText);
        }
    }
}
