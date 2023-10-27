using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Data;

namespace Softuni_Fest
{
	public class SeedData
	{
		public SeedData(RoleManager<IdentityRole> roleManager)
		{
			_RoleManager = roleManager;
		}

		public async Task SeedRolesAsync() 
		{
			string[] roles = new string[] { "Business", "Client" };
			foreach (string role in roles) 
				await CreateRoleAsync(new IdentityRole(role));
		}

		public async Task CreateRoleAsync(IdentityRole role) 
		{
			if (!await _RoleManager.RoleExistsAsync(role.Name))
				await _RoleManager.CreateAsync(role);
		}

		private readonly RoleManager<IdentityRole> _RoleManager;
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
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		private readonly IServiceProvider _ServiceProvider;
	}
}
