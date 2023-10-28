using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Softuni_Fest.Pages
{
    public class ProductModel : PageModel
    {
        [BindProperty]
        public int quantity { get; set; }

        public void OnGet()
        {
        }
    }
}
