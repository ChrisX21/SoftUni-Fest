using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Softuni_Fest.Interfaces;
using Softuni_Fest.Models;

namespace Softuni_Fest.Pages
{
    public class CatalogModel : PageModel
    {
        public CatalogModel(IProductRepository productRepository,
                            IOrderRepository orderRepository,
                            IOrderProductsRepository orderProductsRepository,
                            UserManager<User> userManager,
                            ILogger<CatalogModel> logger)
        {
            _ProductRepository = productRepository;
            _OrderRepository = orderRepository;
            _OrderProductRepository = orderProductsRepository;
            _UserManager = userManager;
            _Logger = logger;
            Products = new List<Product>();
        }
        public async Task OnGet()
        {
            if (User.IsInRole(Roles.Business))
            {
                Products = await GetAllProductsForBusiness();
                return;
            }

            Products = await GetAllProductsForClient();
        }

        public async Task<List<Product>> GetProductsForCurrentUser()
        {
            if (User.IsInRole(Roles.Business))
                return await GetAllProductsForBusiness();

            return await GetAllProductsForClient();
        }

        public async Task OnPostAddItemToCart(string productId)
        {
            string userId = _UserManager.GetUserId(User);
            Order? order = await _OrderRepository.GetOrCreateOrderForUserAsync(userId);
                
            if(order is null)
            {
                _Logger.LogError("Couldn't retrieve or create order");
                return;
            }

            Product? product = await _ProductRepository.GetProductByIdAsync(productId);
            if(product is null)
            {
                _Logger.LogError("Invalid product id");
                return;
            }

            OrderProduct? orderProduct = 
                    await _OrderProductRepository.
                            GetOrCreateOrderItemAsync(order.Id, product.Id);

            if (orderProduct is null) 
            {
                _Logger.LogError("Couldn't retrieve or create cart item");
                return;
            }

            orderProduct.Quantity += ProductQuantity;
            await _OrderProductRepository.SaveAsync();
        }

        public List<Product> Products { get; set; } = null!;
        public async Task<List<Product>> GetAllProductsForBusiness()
        {
            string userId = _UserManager.GetUserId(User);
            List<Product> products = (await _ProductRepository.GetProductsAsyncForVendorId(userId)).ToList();

            return products;
        }
        public async Task<List<Product>> GetAllProductsForClient()
        {
            List<Product> products = (await _ProductRepository.GetProductsAsync()).ToList();
            return products;
        }

        [BindProperty]
        // the user selected quantity of the product
        public int ProductQuantity { get; set; } = 1;


        private readonly IProductRepository _ProductRepository;
        private readonly IOrderRepository _OrderRepository;
        private readonly IOrderProductsRepository _OrderProductRepository;
        private readonly UserManager<User> _UserManager;
        private readonly ILogger<CatalogModel> _Logger;
    }
}
