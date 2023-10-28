using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Softuni_Fest.Pages
{
    public class createModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public Product product { get; set; }
        public createModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPost(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToPage("Catalog");
        }
    }
}
