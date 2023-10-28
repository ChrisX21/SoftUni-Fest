using Microsoft.EntityFrameworkCore;
using Softuni_Fest.Interfaces;

namespace Softuni_Fest.Repository
{
    public class OrderProductsRepository : IOrderProductsRepository
    {
        private readonly ApplicationDbContext _Context;
        public OrderProductsRepository(ApplicationDbContext context) 
        {
            _Context = context;
        }
        public async Task<bool> AddOrderProductAsync(OrderProduct orderProduct)
        {
            _Context.OrderProducts.Add(orderProduct);
            return await SaveAsync();
        }

        public async Task<OrderProduct> GetOrderProductAsync(string id)
        {
            return await _Context.OrderProducts.FindAsync(id);
        }

        public async Task<OrderProduct> GetOrderProductForOrderAndProductAsync(string productId, string orderId)
        {
            return await _Context.OrderProducts
                .Where(x => x.ProductId == productId && x.OrderId == orderId)
                .FirstOrDefaultAsync();
        }

        public async Task<string?> CreateOrderItemAsync(string orderId, string productId) 
        {
            // TODO: check quantity in stock
            OrderProduct orderItem = new() 
            {
                OrderId = orderId,
                ProductId = productId,
                Quantity = 1
            };

            await _Context.OrderProducts.AddAsync(orderItem);

            if (!await SaveAsync())
                return null;

            return orderItem.Id;
        }

        public async Task<List<OrderProduct>> GetOrderItemsForOrderAsync(string orderId) 
        {
            List<OrderProduct> orderProducts = await _Context.OrderProducts.Where(x => x.OrderId == orderId).ToListAsync();
            return orderProducts;
        }

        public async Task<ICollection<OrderProduct>> GetOrderProductsAsync()
        {
            return await _Context.OrderProducts.ToListAsync();
        }

        public async Task<bool> OrderProductExistsAsync(string id)
        {
            return await _Context.OrderProducts.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> OrderProductExistsForOrderAndProductAsync(string productId, string orderId)
        {
            return await _Context.OrderProducts.AnyAsync(x => x.ProductId == productId && x.OrderId == orderId);
        }

        public async Task<bool> RemoveOrderProductAsync(OrderProduct orderProduct)
        {
            _Context.Remove(orderProduct);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            int saved = await _Context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateOrderProductAsync(OrderProduct orderProduct)
        {
            _Context.Update(orderProduct);
            return await SaveAsync();
        }
    }
}
