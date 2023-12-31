﻿using Microsoft.EntityFrameworkCore;
using Softuni_Fest.Interfaces;

namespace Softuni_Fest.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _Context;
        public OrderRepository(ApplicationDbContext context)
        {
            _Context = context;
        }

        public async Task<bool> AddOrderAsync(Order order)
        {
            await _Context.AddAsync(order);
            return await SaveAsync();
        }

        public async Task<Order> GetOrderAsync(string id)
        {
            return await _Context.Orders.FindAsync(id);
        }

        public async Task<Order?> CreateOrderAsync(string userId) 
        {
            Order order = new()
            {
                UserId = userId
            };

            await _Context.AddAsync(order);
            if (!await SaveAsync())
                return null;

            return order;
        }

        public async Task<Order?> GetOrCreateOrderForUserAsync(string userId)
        {
            return 
                await GetOrderForUserAsync(userId) ??
                await CreateOrderAsync(userId);
        }

        public async Task<Order?> GetOrderForUserAsync(string userId) 
        {
            Order? order = await _Context.Orders.FirstOrDefaultAsync(x => x.UserId == userId);
            return order;
        }

        public async Task<ICollection<Order>> GetOrdersAsync()
        {
            return await _Context.Orders.ToListAsync();
        }

        public Task<ICollection<Order>> GetOrdersForUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> OrderExistsAsync(string id)
        {
            return await _Context.Orders.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> RemoveOrderAsync(Order order)
        {
            _Context.Remove(order);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            int saved = await _Context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            _Context.Update(order);
            return await SaveAsync();
        }
    }
}
