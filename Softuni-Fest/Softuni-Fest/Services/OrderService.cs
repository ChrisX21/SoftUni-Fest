using Microsoft.EntityFrameworkCore;
using Softuni_Fest.Interfaces;
using Softuni_Fest.Models;

namespace Softuni_Fest.Services
{
    public class OrderService
    {
        public OrderService(IOrderRepository orderRepository,
                            IOrderOrderStatusRepository orderOrderStatusRepository)
        {
            _OrderRepository = orderRepository;
            _OrderOrderStatusRepository = orderOrderStatusRepository;
        }

        public async Task<Order?> CreateOrderAsync(string userId, string orderStatus) 
        {
            Order? order = await _OrderRepository.CreateOrderAsync(userId);
            if (order is null)
                return null;

            await _OrderOrderStatusRepository.AddToStatusAsync(order, orderStatus);
            return order;
        }

        public async Task<Order?> GetPendingOrderForUserAsync(string userId) 
        {
            IQueryable<Order> pendingOrders = await _OrderOrderStatusRepository.GetOrdersWithStatus(OrderStatus.Pending);
            Order? order = await pendingOrders.FirstOrDefaultAsync(x => x.UserId == userId);
            return order;
        }

        public async Task<List<Order>> GetCompletedOrdersForUserAsync(string userId) 
        {
            IQueryable<Order> completedOrders = await _OrderOrderStatusRepository.GetOrdersWithStatus(OrderStatus.Completed);
            return await completedOrders.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<Order?> GetOrCreatePendingOrderAsync(string userId) 
        {
            Order? order = await GetPendingOrderForUserAsync(userId);
            if (order is not null)
                return order;

            order = await CreateOrderAsync(userId, OrderStatus.Pending);
            return order;
        }

        public async Task<bool> CompleteOrderAsync(Order order)
        {
            if (await _OrderOrderStatusRepository.IsInStatusAsync(order, OrderStatus.Completed))
                return false;

            await _OrderOrderStatusRepository.RemoveFromStatusAsync(order, OrderStatus.Pending);
            await _OrderOrderStatusRepository.AddToStatusAsync(order, OrderStatus.Completed);
            return await _OrderOrderStatusRepository.IsInStatusAsync(order, OrderStatus.Completed);
        }

        public IQueryable<Order> Orders => _OrderRepository.Orders;

        private IOrderRepository _OrderRepository;
        private IOrderOrderStatusRepository _OrderOrderStatusRepository;
    }
}
