using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Softuni_Fest.Interfaces;

namespace Softuni_Fest.Pages
{
    public class CatalogModel : PageModel
    {
        private readonly IProductRepository _ProductRepository;
        private readonly IUserRepository _UserRepository;
        private readonly UserManager<User> _UserManager;
        public CatalogModel(IProductRepository productRepository,
            IUserRepository userRepository,
            UserManager<User> userManager)
        {
            _ProductRepository = productRepository;
            _UserRepository = userRepository;
            _UserManager = userManager;
            Products = new List<Product>();
            Users = new List<User>();
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
            if (User.IsInRole("Business"))
            {
                Products = await GetAllProductsForBusiness();
            }
            else
            {
                Products = await GetAllProductsForClient();
            }
        }
        public List<Product> Products { get; private set; } = null!;
        public List<User> Users { get; private set; } = null!;

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
    }
}
