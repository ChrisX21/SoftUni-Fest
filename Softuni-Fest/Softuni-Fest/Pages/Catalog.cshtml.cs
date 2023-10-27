using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Softuni_Fest.Interfaces;

namespace Softuni_Fest.Pages
{
    [Authorize(Roles = "Business")]
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
        }
        public async Task OnGet()
        {
            if (User.IsInRole("Business"))
            {
                Products = await GetAllProductsForBusiness();
            }
            else if (User.IsInRole("Client"))
            {
                Products = await GetAllProductsForClient();
            }
        }
        public List<Product> Products { get; private set; } = null!;
        public async Task<List<Product>> GetAllProductsForBusiness()
        {
            string userId = _UserManager.GetUserId(User);
            List<Product> products = (await _ProductRepository.GetProductsAsyncForVendorId(userId)).ToList();

            return products;
        }
        public async Task<List<Product>> GetAllProductsForClient()
        {
            string userId = _UserManager.GetUserId(User);
            List<Product> products = (await _ProductRepository.GetProductsAsync()).ToList();

            return products;
        }
    }
}
