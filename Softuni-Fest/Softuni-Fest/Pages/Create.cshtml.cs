using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Softuni_Fest.Interfaces;
using Softuni_Fest.Models;
using Softuni_Fest.Repository;
using System.ComponentModel.DataAnnotations;

namespace Softuni_Fest.Pages
{
    // TODO: do
    [Authorize(Roles = Roles.Business)]
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
        //public async Task<IActionResult> OnPost(Product product)
        //{
        //    //await _context.Products.AddAsync(product);
        //    //await _context.SaveChangesAsync();
        //    return RedirectToPage("Catalog");
        //}


        [BindProperty]
        public Product Product { get; set; }

        public class ProductInputModel 
        {
            [Required]
            [StringLength(100)]
            [Display(Name = "Name")]
            public string ProductName { get; set; } = null!;

            [StringLength(100)]
            [Display(Name = "Description")]
            public string? ProductDescription { get; set; }


            [Required]
            [DataType(DataType.Currency)]
            [Display(Name = "Price")]
            public long Price { get; set; }

            [Required]
            [Display(Name = "Quantity In Stock")]
            public int QuantityInStock { get; set; }
        }


        public async Task<IActionResult> OnPost()
        {
            string userId = _UserManager.GetUserId(User);

            Product.VendorId = userId;

            if (!await _ProductRepository.AddProductAsync(Product))
                return RedirectToPage("/Catalog");

            return RedirectToPage("/Catalog");
        }
        //[BindProperty]
        //public string _ProductId { get; set; }
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
