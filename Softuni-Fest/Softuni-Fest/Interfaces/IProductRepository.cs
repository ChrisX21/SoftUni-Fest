namespace Softuni_Fest.Interfaces
{
    public interface IProductRepository
    {
        Task<bool> AddProductAsync(Product product);
        Task<bool> RemoveProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> ProductExistsAsync(string id);
        Task<bool> SaveAsync();
        Task<Product> GetProductAsync(string id);
        Task<ICollection<Order>> GetProductsAsync();
        Task<ICollection<Order>> GetProductsAsyncForVendorId(string id);
        Task<ICollection<Order>> GetProductsAsyncForOrderId(string id);
    }
}
