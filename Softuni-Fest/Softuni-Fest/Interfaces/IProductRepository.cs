namespace Softuni_Fest.Interfaces
{
    public interface IProductRepository
    {
        Task<bool> AddProductAsync(Product product);
        Task<bool> RemoveProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> ProductExistsAsync(string id);
        Task<bool> RemoveProductAsync(string id);
        Task<bool> SaveAsync();
        Task<Product?> GetProductByIdAsync(string id);
        Product? GetProductById(string id);
        Task<ICollection<Product>> GetProductsAsync();
        Task<ICollection<Product>> GetProductsAsyncForVendorId(string id);
        Task<ICollection<Product>> GetProductsAsyncForOrderId(string id);
    }
}
