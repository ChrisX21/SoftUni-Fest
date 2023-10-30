namespace Softuni_Fest.Interfaces
{
    public interface IOrderProductsRepository
    {
        Task<OrderProduct?> GetOrCreateOrderItemAsync(string orderId, string productId);
        Task<OrderProduct?> CreateOrderItemAsync(string orderId, string productId);
        Task<OrderProduct?> GetOrderItemAsync(string orderId, string productId);
        Task<bool> RemoveOrderItemAsync(string orderItemId);
        Task<OrderProduct?> GetOrderItemAsync(string id);

        Task<List<OrderProduct>> GetOrderItemsForOrderAsync(string orderId);
        Task<bool> AddOrderProductAsync(OrderProduct orderProduct);
        Task<bool> UpdateOrderItemAsync(OrderProduct orderProduct);
        Task<bool> OrderProductExistsAsync(string id);
        Task<bool> SaveAsync();
        Task<ICollection<OrderProduct>> GetOrderProductsAsync();
    }
}
