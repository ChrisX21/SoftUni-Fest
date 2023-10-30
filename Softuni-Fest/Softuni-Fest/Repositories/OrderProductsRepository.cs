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

        public async Task<OrderProduct?> GetOrderItemAsync(string id)
        {
            return await _Context.OrderProducts.FindAsync(id);
        }

        public async Task<OrderProduct?> GetOrderItemAsync(string orderId, string productId)
        {
            return await _Context.OrderProducts
                            .FirstOrDefaultAsync(x => x.OrderId == orderId &&
                                                      x.ProductId == productId);
        }

        public async Task<OrderProduct?> GetOrCreateOrderItemAsync(string orderId, string productId) 
        {
            return
                await GetOrderItemAsync(orderId, productId) ??
                await CreateOrderItemAsync(orderId, productId);
        }

        public async Task<bool> RemoveOrderItemAsync(string orderItemId) 
        {
            OrderProduct? orderItem = await GetOrderItemAsync(orderItemId);
            if (orderItem is null)
                return false;

            _Context.OrderProducts.Remove(orderItem);
            return await SaveAsync();
        }

        public async Task<OrderProduct?> CreateOrderItemAsync(string orderId, string productId) 
        {
            // TODO: check quantity in stock
            OrderProduct orderItem = new() 
            {
                OrderId = orderId,
                ProductId = productId,
                Quantity = 0
            };

            await _Context.OrderProducts.AddAsync(orderItem);

            if (!await SaveAsync())
                return null;

            return orderItem;
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


        public async Task<bool> SaveAsync()
        {
            int saved = await _Context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateOrderItemAsync(OrderProduct orderProduct)
        {
            _Context.Update(orderProduct);
            return await SaveAsync();
        }
    }
}
