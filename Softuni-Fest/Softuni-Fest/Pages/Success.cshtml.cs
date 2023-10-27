using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Softuni_Fest.Services;

namespace Softuni_Fest.Pages
{
    public class SuccessModel : PageModel
    {
        private readonly ILogger<SuccessModel> _logger;
        
        public SuccessModel(ILogger<SuccessModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

    }
}