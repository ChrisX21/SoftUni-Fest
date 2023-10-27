using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Softuni_Fest.Interfaces;

namespace Softuni_Fest.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _Context;
        public ProductRepository(ApplicationDbContext context)
        {
            _Context = context;
        }

        public async Task<bool> AddProductAsync(Product product)
        {
            _Context.Add(product);
            return await SaveAsync();
        }

        public async Task<Product> GetProductAsync(string id)
        {
            return await _Context.Products.FindAsync(id);
        }

        public async Task<ICollection<Product>> GetProductsAsync()
        {
            return await _Context.Products.ToListAsync();
        }

        public async Task<ICollection<Product>> GetProductsAsyncForOrderId(string id)
        {
            return await _Context.Products
                .Where(x => x.OrderProducts
                .Any(a => a.OrderId == id))
                .Include(x => x.Vendor)
                .ToListAsync();
        }

        public async Task<ICollection<Product>> GetProductsAsyncForVendorId(string id)
        {
            return await _Context.Products.Where(x => x.VendorId == id).ToListAsync();
        }

        public async Task<bool> ProductExistsAsync(string id)
        {
            return await _Context.Products.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> RemoveProductAsync(Product product)
        {
            _Context.Remove(product);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            int saved = await _Context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            _Context.Update(product);
            return await SaveAsync();
        }
    }
}
