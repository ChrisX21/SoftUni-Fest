using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Softuni_Fest.Pages
{
    public class CatalogModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        
        public CatalogModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            
        }
    }
}
