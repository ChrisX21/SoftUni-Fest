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
        
        [BindProperty]
        public ProductInputModel Input { get; set; }

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
            [Range(0.01, 1000000.0, ErrorMessage = "The {0} should be between {1} and {2}")]
            [DataType(DataType.Currency)]
            [Display(Name = "Price")]
            public double Price { get; set; }
        }


        public async Task<IActionResult> OnPost()
        {
            string userId = _UserManager.GetUserId(User);

            Product product = new()
            {
                ProductName = Input.ProductName,
                ProductDescription = Input.ProductDescription,
                ProductPrice = (long)(Input.Price * 100d),
                VendorId = userId
            };

            if (!await _ProductRepository.AddProductAsync(product))
                return RedirectToPage("/Catalog");

            return RedirectToPage("/Catalog");
        }

        public async Task<IActionResult> OnDelete()
        {
            return RedirectToPage("/Catalog");
        }
    }
}
