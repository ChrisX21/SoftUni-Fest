using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Softuni_Fest
{
	public class SeedData
	{
		public SeedData(ApplicationDbContext context,
						RoleManager<IdentityRole> roleManager,
						UserManager<User> userManager,
                        IUserStore<User> userStore)
		{
			_RoleManager = roleManager;
			_UserManager = userManager;
			_Context = context;
			_UserStore = userStore;
		}

		public async Task SeedRolesAsync() 
		{
			string[] roles = new string[] { "Business", "Client" };
			foreach (string role in roles) 
				await CreateRoleAsync(new IdentityRole(role));
		}

		public async Task SeedUserAsync() 
		{
			if (_Context.Users.Any())
				return;

			//IdentityRole businessRole = await _Context.Roles.FirstAsync(x => x.Name == "Business");
			//IdentityRole clientRole = await _Context.Roles.FirstAsync(x => x.Name == "Client");

			// create business user
			User businessUser = new();

            await _UserStore.SetUserNameAsync(businessUser, _BusinessEmail, CancellationToken.None);
            await ((IUserEmailStore<User>)_UserStore).SetEmailAsync(businessUser, _BusinessEmail, CancellationToken.None);
			businessUser.EmailConfirmed = true;
            var businessResult = await _UserManager.CreateAsync(businessUser, _Password);
			await _UserManager.AddToRoleAsync(businessUser, "Business");

            // creat client user
            User clientUser = new();

            await _UserStore.SetUserNameAsync(clientUser, _ClientEmail, CancellationToken.None);
            await ((IUserEmailStore<User>)_UserStore).SetEmailAsync(clientUser, _ClientEmail, CancellationToken.None);
            clientUser.EmailConfirmed = true;
            var clentResult = await _UserManager.CreateAsync(clientUser, _Password);
            await _UserManager.AddToRoleAsync(clientUser, "Client");
        }

        public async Task SeedProductsAsync()
		{
			if (_Context.Products.Any())
				return;

			User user = await _Context.Users.FirstAsync(x => x.UserName == _BusinessEmail);

			Product product1 = new Product() 
			{
				ProductName = _ProductName,
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

            User user = await _Context.Users.FirstAsync(x => x.UserName == _ClientEmail);

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

			Product product = await _Context.Products.FirstAsync(x => x.ProductName == _ProductName);
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
		private readonly UserManager<User> _UserManager;
		private readonly ApplicationDbContext _Context;
		private readonly IUserStore<User> _UserStore;
		private const string _Password = "test@T1";
		private const string _BusinessEmail = "test@business.com";
		private const string _ClientEmail = "test@client.com";
		private const string _ProductName = "Test1";
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
				await seeder.SeedUserAsync();
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
