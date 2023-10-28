using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace Softuni_Fest.Services
{
    public struct StripeServiceParameters
    {
        public StripeServiceParameters() 
        {
            SuccessURL = "/";
            CancelURL = "/";
        }

        public string SuccessURL;
        public string CancelURL;
    }

    public class StripeService
    {
        public StripeService(IOptions<StripeSettings> settings,
                            ApplicationDbContext context,
                            IServiceProvider serviceProvider)
        {
            _StripeSettings = settings.Value;
            _SessionService = new SessionService();
            _Context = context;

            // get the server address
            var server = serviceProvider.GetRequiredService<IServer>();
            var serverAddressFeature = server.Features.Get<IServerAddressesFeature>();

            _ServerAddress = null;

            if (serverAddressFeature is not null)
                _ServerAddress = serverAddressFeature.Addresses.FirstOrDefault();
        }

        public async Task<Session?> Checkout(List<OrderProduct>? cartItems)
        {
            if (cartItems is null)
                return null;

            if (_ServerAddress is null)
                return null;

            List<SessionLineItemOptions> lineItems = new List<SessionLineItemOptions>();

            foreach (var cartItem in cartItems)
            {
                Product? product = await _Context.Products.FirstOrDefaultAsync(x => x.Id == cartItem.ProductId);
                if (product is null)
                    return null;

                lineItems.Add(new()
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        UnitAmount = product.ProductPrice,
                        Currency = "BGN",
                        ProductData = new SessionLineItemPriceDataProductDataOptions()
                        {
                            Name = product.ProductName
                        }
                    },
                    Quantity = cartItem.Quantity
                });
            }

            SessionCreateOptions options = new SessionCreateOptions()
            {
                SuccessUrl = $"{_ServerAddress}{Parameters.SuccessURL}",
                CancelUrl = $"{_ServerAddress}{Parameters.CancelURL}",
                PaymentMethodTypes = new List<string>()
                {
                    "card"
                },
                LineItems = lineItems,
                Mode = "payment"
            };

            return await _SessionService.CreateAsync(options);
        }

        public async Task ExpireOrderAsync(string sessionId)
        {
            await _SessionService.ExpireAsync(sessionId);
        }

        public StripeServiceParameters Parameters { get; set; }

        private readonly ApplicationDbContext _Context;
        private readonly StripeSettings _StripeSettings;
        private readonly string? _ServerAddress;
		private readonly SessionService _SessionService;
	}
}
