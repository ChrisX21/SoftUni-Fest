using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Softuni_Fest
{
	public partial class ApplicationDbContext : IdentityDbContext<User>
	{
		public ApplicationDbContext() 
		{
		}

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
			: base(options)
		{
		}

		public override DbSet<User> Users { get; set; } = null!;
	}
}
