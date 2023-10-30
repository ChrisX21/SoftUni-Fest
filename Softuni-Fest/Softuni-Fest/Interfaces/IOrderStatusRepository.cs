using Softuni_Fest.Models;

namespace Softuni_Fest.Source.Interfaces
{
    public interface IOrderStatusRepository
    {
        Task<bool> CreatOrderStatusAsync(OrderStatus orderStatus);
        Task<bool> OrderStatusExists(string orderStatusName);
        Task<bool> SaveAsync();
        Task<bool> AnyAsync();

        List<OrderStatus> OrderStatuses { get; }
    }
}
