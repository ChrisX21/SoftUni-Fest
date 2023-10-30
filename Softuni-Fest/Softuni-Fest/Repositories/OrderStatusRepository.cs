using Microsoft.EntityFrameworkCore;
using Softuni_Fest.Models;
using Softuni_Fest.Source.Interfaces;

namespace Softuni_Fest.Source.Repository
{
    public class OrderStatusRepository : IOrderStatusRepository
    {
        public OrderStatusRepository(ApplicationDbContext context) 
        {
            _Context = context;
        }

        public async Task<bool> CreatOrderStatusAsync(OrderStatus orderStatus)
        {
            await _Context.AddAsync(orderStatus);
            return await SaveAsync();
        }

        public Task<bool> OrderStatusExists(string orderStatusName)
        {
            return _Context.OrderStatuses.AnyAsync(x => x.Name == orderStatusName);
        }

        public async Task<bool> SaveAsync() 
        {
            return (await _Context.SaveChangesAsync()) > 0 ? true : false;
        }
        public List<OrderStatus> OrderStatuses 
        { 
            get => _Context.OrderStatuses.ToList();
        }

        public Task<bool> AnyAsync() 
        {
            return _Context.OrderStatuses.AnyAsync();
        }

        private ApplicationDbContext _Context;

    }
}
