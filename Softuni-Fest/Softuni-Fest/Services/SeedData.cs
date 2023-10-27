using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Data;
using static System.Net.Mime.MediaTypeNames;

namespace Softuni_Fest
{
	public class SeedData
	{
		public SeedData(ApplicationDbContext context,
						RoleManager<IdentityRole> roleManager)
		{
			_RoleManager = roleManager;
			_Context = context;
		}

		public async Task SeedRolesAsync() 
		{
			string[] roles = new string[] { "Business", "Client" };
			foreach (string role in roles) 
				await CreateRoleAsync(new IdentityRole(role));
		}

		public async Task SeedProductsAsync()
		{
			if (_Context.Products.Any())
				return;

			User user = await _Context.Users.FirstAsync(x => x.UserName == "test2@gmail.com");

			Product product1 = new Product() 
			{
				ProductName = "Test1",
				ProductDescription = "This is a test product",
				ProductPrice = 1000,
				VendorId = user.Id
			};

			await _Context.Products.AddAsync(product1);
			await _Context.SaveChangesAsync();
		}

		public async Task SeedOrderAsync() 
		{
			if (_Context.Orders.Any())
				return;

            User user = await _Context.Users.FirstAsync(x => x.UserName == "test2@gmail.com");

			Order order = new Order() 
			{
				UserId = user.Id
			};

            await _Context.Orders.AddAsync(order);
            await _Context.SaveChangesAsync();
        }

		public async Task SeedCartItemAsync() 
		{
			if (_Context.OrderProducts.Any())
				return;

			Product product = await _Context.Products.FirstAsync(x => x.ProductName == "Test1");
			Order order = await _Context.Orders.FirstAsync();

			OrderProduct orderProduct = new OrderProduct() 
			{
				ProductId = product.Id,
				OrderId = order.Id,
				Quantity = 2
			};

			await _Context.OrderProducts.AddAsync(orderProduct);
			await _Context.SaveChangesAsync();
		}

		public async Task CreateRoleAsync(IdentityRole role) 
		{
			if (!await _RoleManager.RoleExistsAsync(role.Name))
				await _RoleManager.CreateAsync(role);
		}

		private readonly RoleManager<IdentityRole> _RoleManager;
		private readonly ApplicationDbContext _Context;
	}

	public class SeederService : IHostedService
	{
		public SeederService(IServiceProvider serviceProvider) 
		{
			_ServiceProvider = serviceProvider;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			using (var scope = _ServiceProvider.CreateScope())
			{
				var seeder = scope.ServiceProvider.GetRequiredService<SeedData>();
				await seeder.SeedRolesAsync();
				await seeder.SeedProductsAsync();
				await seeder.SeedOrderAsync();
				await seeder.SeedCartItemAsync();
            }
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		private readonly IServiceProvider _ServiceProvider;
	}
}
