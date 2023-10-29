using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Softuni_Fest.Interfaces;
using Softuni_Fest.Repository;
using Softuni_Fest.Services;
using Stripe;

namespace Softuni_Fest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddRazorPages();
            builder.Services.AddTransient<SeedData>();
            builder.Services.AddHostedService<SeederService>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderProductsRepository, OrderProductsRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<StripeService>();
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe")["SecretKey"];
            //builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("Mail"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
                app.UseExceptionHandler("/Error");

            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}