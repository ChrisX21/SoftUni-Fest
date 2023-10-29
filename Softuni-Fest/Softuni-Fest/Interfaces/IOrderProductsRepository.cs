namespace Softuni_Fest.Interfaces
{
    public interface IOrderProductsRepository
    {
        Task<OrderProduct?> GetOrCreateOrderItemAsync(string orderId, string productId);
        Task<OrderProduct?> CreateOrderItemAsync(string orderId, string productId);
        Task<OrderProduct?> GetOrderItemAsync(string orderId, string productId);
        Task<bool> RemoveOrderAsync(string orderItemId);

        Task<List<OrderProduct>> GetOrderItemsForOrderAsync(string orderId);
        Task<bool> AddOrderProductAsync(OrderProduct orderProduct);
        Task<bool> RemoveOrderProductAsync(OrderProduct orderProduct);
        Task<bool> UpdateOrderProductAsync(OrderProduct orderProduct);
        Task<bool> OrderProductExistsAsync(string id);
        Task<bool> SaveAsync();
        Task<OrderProduct?> GetOrderProductAsync(string id);
        Task<ICollection<OrderProduct>> GetOrderProductsAsync();
        Task<bool> OrderProductExistsForOrderAndProductAsync(string productId, string orderId);
        Task<OrderProduct> GetOrderProductForOrderAndProductAsync(string productId, string orderId);
    }
}
