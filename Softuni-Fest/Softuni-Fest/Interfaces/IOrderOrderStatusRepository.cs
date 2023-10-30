namespace Softuni_Fest.Interfaces
{
    public interface IOrderOrderStatusRepository
    {
        Task AddToStatusAsync(Order order, string orderStatusName);
        Task<bool> IsInStatusAsync(Order order, string orderStatusName);
        Task RemoveFromStatusAsync(Order order, string orderStatusName);
        Task<IQueryable<Order>> GetOrdersWithStatus(string orderStatusName);
    }
}
