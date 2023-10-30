namespace Softuni_Fest.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrCreateOrderForUserAsync(string userId);
        Task<Order?> CreateOrderAsync(string userId);
        Task<Order?> GetOrderForUserAsync(string userId);
        Task<bool> AddOrderAsync(Order order);
        Task<bool> RemoveOrderAsync(Order order);
        Task<bool> UpdateOrderAsync(Order order);
        Task<bool> OrderExistsAsync(string id);
        Task<bool> SaveAsync();
        Task<Order> GetOrderAsync(string id);
        Task<ICollection<Order>> GetOrdersAsync();
        Task<ICollection<Order>> GetOrdersForUserAsync(string userId);

        IQueryable<Order> Orders { get; }
    }
}
