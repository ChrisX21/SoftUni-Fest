using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Softuni_Fest.Services;
using Stripe.Checkout;

namespace Softuni_Fest.Pages
{
    public class TestModel : PageModel
    {
        private readonly ILogger<TestModel> _logger;
        private readonly IServiceProvider _ServiceProvider;
        private readonly ApplicationDbContext _Context;
        private readonly StripeService _StripeService;

        public TestModel(ILogger<TestModel> logger,
                        IServiceProvider provider,
                        ApplicationDbContext context)
        {
            _logger = logger;
            _ServiceProvider = provider;
            _Context = context;
            _StripeService = provider.GetRequiredService<StripeService>();
        }

        public void OnGet()
        {

        }

        public async Task<ActionResult> OnPost()
        {
            var server = _ServiceProvider.GetRequiredService<IServer>();
            var serverAddressFeature = server.Features.Get<IServerAddressesFeature>();

            string? serverAddress = null;

            if (serverAddressFeature is not null)
                serverAddress = serverAddressFeature.Addresses.FirstOrDefault();

            if (serverAddress is null)
            {
                _logger.LogError("Server address is null");
                return StatusCode(500);
            }

            _StripeService.Parameters = new StripeServiceParameters()
            {
                SuccessURL = $"{serverAddress}/success",
                CancelURL = $"{serverAddress}/failed"
            };

            User buyer = await _Context.Users.FirstAsync(x => x.UserName == "test@client.com");
            Order order = await _Context.Orders.FirstAsync(x => x.UserId == buyer.Id);

            List<OrderProduct> cartItems = await _Context.OrderProducts.Where(x => x.OrderId == order.Id).ToListAsync();

            Session? session = await _StripeService.Checkout(cartItems, _Context);

            if (session is null)
            {
                _logger.LogError("Couldn't create session");
                return StatusCode(500);
            }

            return Redirect(session.Url);
        }
    }
}