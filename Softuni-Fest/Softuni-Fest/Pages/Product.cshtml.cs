using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Softuni_Fest.Interfaces;
using Softuni_Fest.Models;
using Softuni_Fest.Services;
using System.ComponentModel.DataAnnotations;

namespace Softuni_Fest.Pages
{
    public class ProductModel : PageModel
    {
        public ProductModel(IProductRepository productRepository,
                            OrderService orderService,
                            IOrderProductsRepository orderProductsRepository,
                            UserManager<User> userManager,
                            ILogger<ProductModel> logger) 
        {
            _ProductRepository = productRepository;
            _OrderService = orderService;
            _OrderProductRepository = orderProductsRepository;
            _UserManager = userManager;
            _Logger = logger;
        }

        public async Task OnGet(string productId)
        {
            Product = await _ProductRepository.GetProductByIdAsync(productId);
        }

        public async Task<IActionResult> OnPostAddItemToCart(string productId) 
        {
            if (!User.IsInRole(Roles.Client))
                return Redirect("/Identity/Account/Login");
            string userId = _UserManager.GetUserId(User);
            Order? order = await _OrderService.GetOrCreatePendingOrderAsync(userId);

            if (order is null)
            {
                _Logger.LogError("Couldn't retrieve or create order");
                return LocalRedirect("/catalog");
            }

            Product? product = await _ProductRepository.GetProductByIdAsync(productId);
            if (product is null)
            {
                _Logger.LogError("Invalid product id");
                return LocalRedirect("/catalog");
            }

            OrderProduct? orderProduct =
                    await _OrderProductRepository.
                            GetOrCreateOrderItemAsync(order.Id, productId);

            if (orderProduct is null)
            {
                _Logger.LogError("Couldn't retrieve or create cart item");
                return LocalRedirect("/catalog");
            }

            orderProduct.Quantity += Input.Quantity;
            await _OrderProductRepository.SaveAsync();
            return LocalRedirect("/catalog");
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
        private OrderService _OrderService;
        private IOrderProductsRepository _OrderProductRepository;
        private UserManager<User> _UserManager;
        private ILogger<ProductModel> _Logger;
    }
}
