using Microsoft.EntityFrameworkCore;
using Softuni_Fest.Interfaces;
using Softuni_Fest.Models;

namespace Softuni_Fest.Repositories
{
    public class OrderOrderStatusRepository : IOrderOrderStatusRepository
    {
        public OrderOrderStatusRepository(ApplicationDbContext context)
        {
            _Context = context;
        }

        public async Task AddToStatusAsync(Order order, string orderStatusName)
        {
            OrderStatus? orderStatus = await GetOrderStatusByName(orderStatusName);
            if (orderStatus is null)
                return;

            OrderOrderStatus orderOrderStatus = new()
            {
                OrderId = order.Id,
                OrderStatusId = orderStatus.Id
            };
            await _Context.AddAsync(orderOrderStatus);
            await SaveAsync();
        }

        public async Task<bool> IsInStatusAsync(Order order, string orderStatusName)
        {
            OrderStatus? orderStatus = await GetOrderStatusByName(orderStatusName);
            if (orderStatus is null)
                return false;

            return await _Context.OrderOrderStatuses.AnyAsync(x => x.OrderId == order.Id && x.OrderStatusId == orderStatus.Id);
        }

        public async Task RemoveFromStatusAsync(Order order, string orderStatusName)
        {
            OrderStatus? orderStatus = await GetOrderStatusByName(orderStatusName);
            if (orderStatus is null)
                return;

            OrderOrderStatus? orderOrderStatus = await _Context.OrderOrderStatuses
                                                        .FirstOrDefaultAsync(x => 
                                                                x.OrderId == order.Id && 
                                                                x.OrderStatusId == orderStatus.Id);

            if (orderOrderStatus is null)
                return;

            _Context.Remove(orderOrderStatus);
            await SaveAsync();
        }

        private async Task<bool> SaveAsync() 
        {
            return (await _Context.SaveChangesAsync()) > 0 ? true : false;
        }

        private async Task<OrderStatus?> GetOrderStatusByName(string orderStatusName) 
        {
            return await _Context.OrderStatuses
                            .FirstOrDefaultAsync(x => x.Name == orderStatusName);
        } 

        public async Task<IQueryable<Order>> GetOrdersWithStatus(string orderStatusName)
        {
            OrderStatus? orderStatus = await GetOrderStatusByName(orderStatusName);
            if (orderStatus is null)
                return new List<Order>().AsQueryable();

            IQueryable<OrderOrderStatus> orderOrderStatuses = _Context.OrderOrderStatuses
                                                                .Where(x => x.OrderStatusId == orderStatus.Id);

            return _Context.Orders.Join(orderOrderStatuses,
                                        order => order.Id,
                                        orderOrderStatus => orderOrderStatus.OrderId,
                                        (order, orderOrderStatus) => order);
        }

        private readonly ApplicationDbContext _Context;
    }
}
