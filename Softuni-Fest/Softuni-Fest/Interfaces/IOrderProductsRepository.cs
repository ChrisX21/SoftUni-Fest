namespace Softuni_Fest.Interfaces
{
    public interface IOrderProductsRepository
    {
        Task<bool> AddOrderProductAsync(OrderProduct orderProduct);
        Task<bool> RemoveOrderProductAsync(OrderProduct orderProduct);
        Task<bool> UpdateOrderProductAsync(OrderProduct orderProduct);
        Task<bool> OrderProductExistsAsync(string id);
        Task<bool> SaveAsync();
        Task<OrderProduct> GetOrderProductAsync(string id);
        Task<ICollection<OrderProduct>> GetOrderProductsAsync();
        Task<bool> OrderProductExistsForOrderAndProductAsync(string productId, string orderId);
        Task<OrderProduct> GetOrderProductForOrderAndProductAsync(string productId, string orderId);
    }
}
