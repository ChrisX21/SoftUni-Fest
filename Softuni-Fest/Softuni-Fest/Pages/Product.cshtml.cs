using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Softuni_Fest.Interfaces;
using Softuni_Fest.Models;
using System.ComponentModel.DataAnnotations;

namespace Softuni_Fest.Pages
{
    public class ProductModel : PageModel
    {
        public ProductModel(IProductRepository productRepository,
                            IOrderRepository orderRepository,
                            IOrderProductsRepository orderProductsRepository,
                            UserManager<User> userManager,
                            ILogger<ProductModel> logger) 
        {
            _ProductRepository = productRepository;
            _OrderRepository = orderRepository;
            _OrderProductRepository = orderProductsRepository;
            _UserManager = userManager;
            _Logger = logger;
        }

        public async Task OnGet(string productId)
        {
            Product = await _ProductRepository.GetProductByIdAsync(productId);
        }

        public async Task<IActionResult> OnPost(string productId) 
        {
            if (!User.IsInRole(Roles.Client))
                return Redirect("/Identity/Account/Login");
            string userId = _UserManager.GetUserId(User);
            Order? order = await _OrderRepository.GetOrCreateOrderForUserAsync(userId);

            if (order is null)
            {
                _Logger.LogError("Couldn't retrieve or create order");
                return LocalRedirect("/Catalog");
            }

            Product? product = await _ProductRepository.GetProductByIdAsync(productId);
            if (product is null)
            {
                _Logger.LogError("Invalid product id");
                return LocalRedirect("/Catalog");
            }

            OrderProduct? orderProduct =
                    await _OrderProductRepository.
                            GetOrCreateOrderItemAsync(order.Id, productId);

            if (orderProduct is null)
            {
                _Logger.LogError("Couldn't retrieve or create cart item");
                return LocalRedirect("/Catalog");
            }

            orderProduct.Quantity += Input.Quantity;
            await _OrderProductRepository.SaveAsync();
            return LocalRedirect("/Catalog");
        }

        [BindProperty]
        public InputData Input { get; set; } 

        public class InputData
        {
            [Range(1, 20, ErrorMessage = "{0} should be between {1} and {2}")]
            [Display(Name = "Quantity")]
            public int Quantity { get; set; }
        }

        public Product? Product { get; set; }

        private IProductRepository _ProductRepository;
        private IOrderRepository _OrderRepository;
        private IOrderProductsRepository _OrderProductRepository;
        private UserManager<User> _UserManager;
        private ILogger<ProductModel> _Logger;
    }
}
