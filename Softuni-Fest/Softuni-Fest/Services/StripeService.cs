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
		public StripeService(IOptions<StripeSettings> settings)
		{
			_StripeSettings = settings.Value;
			_SessionService = new SessionService();
            //_Context = context;
		}

        public async Task<Session?> Checkout(List<OrderProduct> cartItems, ApplicationDbContext context)
        {

            List<SessionLineItemOptions> lineItems = new List<SessionLineItemOptions>();

            foreach (var cartItem in cartItems)
            {
                Product? product = await context.Products.FirstOrDefaultAsync(x => x.Id == cartItem.ProductId);
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
                SuccessUrl = Parameters.SuccessURL,
                CancelUrl = Parameters.CancelURL,
                PaymentMethodTypes = new List<string>()
                {
                    "card"
                },
                LineItems = lineItems,
                Mode = "payment"
            };

            return await _SessionService.CreateAsync(options);
        }

        public StripeServiceParameters Parameters { get; set; }

        private readonly StripeSettings _StripeSettings;
		private readonly SessionService _SessionService;
	}
}
