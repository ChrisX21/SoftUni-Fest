using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Softuni_Fest.Services;

namespace Softuni_Fest.Pages
{
    public class FailureModel : PageModel
    {
        private readonly ILogger<FailureModel> _logger;
        
        public FailureModel(ILogger<FailureModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

    }
}