using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Softuni_Fest.Interfaces;
using Softuni_Fest.Repository;

namespace Softuni_Fest.Pages
{
    public class createModel : PageModel
    {
        private readonly IProductRepository _ProductRepository;
        private readonly UserManager<User> _UserManager;
        public createModel(IProductRepository productRepository, UserManager<User> userManager)
        {
            _ProductRepository = productRepository;
            _UserManager = userManager;
        }
        public void OnGet()
        {
        }
        [BindProperty]
        public Product Product { get; set; }
        [Authorize(Roles = "Business")]
        public async Task<IActionResult> OnPost()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }

            string userId = _UserManager.GetUserId(User);

            Product.VendorId = userId;

            if (!await _ProductRepository.AddProductAsync(Product))
            {
                return StatusCode(400);
            }
            return StatusCode(200);
        }
    }
}
