using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Softuni_Fest.Interfaces;
using Softuni_Fest.Repository;

namespace Softuni_Fest.Pages
{
    // TODO: do
    [Authorize(Roles = "Business")]
    public class CreateModel : PageModel
    {
        private readonly IProductRepository _ProductRepository;
        private readonly UserManager<User> _UserManager;
        public CreateModel(IProductRepository productRepository, UserManager<User> userManager)
        {
            _ProductRepository = productRepository;
            _UserManager = userManager;
        }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPost(Product product)
        {
            //await _context.Products.AddAsync(product);
            //await _context.SaveChangesAsync();
            return RedirectToPage("Catalog");
        }
        [BindProperty]
        public Product Product { get; set; }
        public async Task<IActionResult> OnPost()
        {
            string userId = _UserManager.GetUserId(User);

            Product.VendorId = userId;

            if (!await _ProductRepository.AddProductAsync(Product))
            {
                return Redirect("/Index");
            }
            return Page();
        }
        //[BindProperty]
        public string _ProductId { get; set; }
        public async Task<IActionResult> OnDelete()
        {
            if(!await _ProductRepository.RemoveProductAsync(Product))
            {
                return Redirect("/Index");
            }
            return Page();
        }
        public async Task<IActionResult> OnUpdate()
        {
            //Product product = await _ProductRepository.GetProductAsync(_ProductId);
            //product = Product;
            //if (!await _ProductRepository.UpdateProductAsync(product))
            //{
            //    return Redirect("/Index");
            //}
            return Page();
        }
    }
}
