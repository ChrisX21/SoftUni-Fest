using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Softuni_Fest.Interfaces;
using Softuni_Fest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Softuni_Fest.Pages
{
    public class CatalogModel : PageModel
    {
        private readonly IProductRepository _ProductRepository;
        private readonly IOrderRepository _OrderRepository;
        private readonly IOrderProductsRepository _OrderProductRepository;
        private readonly IUserRepository _UserRepository;
        private readonly UserManager<User> _UserManager;
        private readonly ILogger<CatalogModel> _Logger;
        public CatalogModel(IProductRepository productRepository,
                            IOrderRepository orderRepository,
                            IOrderProductsRepository orderProductsRepository,
                            UserManager<User> userManager,
                            ILogger<CatalogModel> logger,
                            IUserRepository userRepository)
        {
            _ProductRepository = productRepository;
            _OrderRepository = orderRepository;
            _OrderProductRepository = orderProductsRepository;
            _UserManager = userManager;
            _Logger = logger;
            Products = new List<Product>();
            Users = new List<User>();
            _UserRepository = userRepository;
        }
        public async Task OnGet()
        {
            await GetRecommendedUsers("test");
            foreach (var item in Users)
            {
                Console.WriteLine(item.Email);
            }

            await GetRecommendedProducts();
            foreach (var item in Products)
            {
                Console.WriteLine(item.ProductName);
            }
            if (User.IsInRole(Roles.Business))
            {
                Products = await GetAllProductsForBusiness();
                return;
            }
            Products = await GetAllProductsForClient();
        }
        public List<Product> Products { get; private set; } = null!;
        public List<User> Users { get; private set; } = null!;

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

            if (order is null)
            {
                _Logger.LogError("Couldn't retrieve or create order");
                return;
            }

            Product? product = await _ProductRepository.GetProductByIdAsync(productId);
            if (product is null)
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

        public async Task GetRecommendedUsers(string username)
        {
            Users = (await _UserRepository.GetRecommendedUser(username)).ToList();
        }
        public async Task GetRecommendedProducts()
        {
            Products = new List<Product>();
            foreach (User user in Users)
            {
                List<Product> productsForVendor = (await _ProductRepository.GetProductsAsyncForVendorId(user.Id)).ToList();
                Products.AddRange(productsForVendor);
            }
        }
        [BindProperty]
        public int ProductQuantity { get; set; } = 1;
    }
}
