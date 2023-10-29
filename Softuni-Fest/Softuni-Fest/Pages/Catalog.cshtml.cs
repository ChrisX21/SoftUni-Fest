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
                            UserManager<User> userManager,
                            ILogger<CatalogModel> logger,
                            IUserRepository userRepository)
        {
            _ProductRepository = productRepository;
            _UserManager = userManager;
            _UserRepository = userRepository;
            _Logger = logger;
            Products = new List<Product>();
            Users = new List<User>();
        }

        public async Task OnGet()
        {
            if (User.IsInRole(Roles.Business))
            {
                Products = await GetAllProductsForBusiness();
                return;
            }

            if (!string.IsNullOrEmpty(SearchTerm)) 
            {
                List<User> recommendedVendors = await _UserRepository.GetUsersFromSearchTerm(SearchTerm);
                Products = (await GetAllProductsForClient()).Where(x => recommendedVendors.Any(s => s.Id == x.VendorId)).ToList();
                return;
            }

            Products = await GetAllProductsForClient();
        }

        public List<User> Users { get; private set; } = null!;

        public async Task<IActionResult> OnPostDelete(string productId)
        {
            await _ProductRepository.RemoveProductAsync(productId);
            return RedirectToPage("/Catalog");
        }

        public async Task<List<Product>> GetProductsForCurrentUser()
        {
            if (User.IsInRole(Roles.Business))
                return await GetAllProductsForBusiness();

            return await GetAllProductsForClient();
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

        public async Task<User> GetVendorById(string vendorId)
        {
            return await _UserRepository.GetUserAsync(vendorId);
        }

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }
        public List<Product> Products { get; set; } = null!;

        private readonly IProductRepository _ProductRepository;
        private readonly UserManager<User> _UserManager;
        private readonly IUserRepository _UserRepository;
        private readonly ILogger<CatalogModel> _Logger;
    }
}
