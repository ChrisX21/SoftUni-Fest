using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Softuni_Fest.Interfaces;
using Softuni_Fest.Models;
using Softuni_Fest.Services;
using Stripe.Checkout;
using Microsoft.Extensions.Options;

namespace Softuni_Fest.Pages
{
    [Authorize(Roles = Roles.Client)]
    public class OrdersModel : PageModel
    {
        public OrdersModel(OrderService orderService,
                        IOrderProductsRepository orderProductsRepository,
                        IProductRepository productRepository,
                        StripeService striveService,
                        IOptions<StripeSettings> options,
                        UserManager<User> userManager,
                        ILogger<OrdersModel> logger)
        {
            _OrderService = orderService;
            _OrderItemRepository = orderProductsRepository;
            _ProductRepository = productRepository;
            
            // setup the stripe service
            _StripeService = striveService;
            _StripeService.Parameters = new() 
            {
                SuccessURL = $"/success",
                CancelURL = $"/failed"
            };

            _UserManager = userManager;
            _Logger = logger;
            _StripeSettings = options.Value;
        }

        public async Task OnGet()
        {
            OrderItems = await GetOrderItemsForCurrentUser();
            if (OrderItems is null)
                return;

            TotalPrice = CalculateTotalPrice(OrderItems);
        }

        public async Task<ActionResult> OnPostCheckout()
        {
            OrderItems = await GetOrderItemsForCurrentUser();
            if (OrderItems is null || Order is null)
                return Page();

            _Logger.LogInformation("Checking out");
            Session? session = await _StripeService.Checkout(OrderItems);

            if (session is null)
            {
                _Logger.LogError("Couldn't create session");
                return StatusCode(500);
            }

            if (!await _OrderService.CompleteOrderAsync(Order))
            {
                await _StripeService.ExpireOrderAsync(session.Id);
                _Logger.LogError("Couldn't finish the order");
                return StatusCode(500);
            }

            return Redirect(session.Url);
        }

        public async Task<ActionResult> OnPostRemoveItem(string orderItemId) 
        {
            await _OrderItemRepository.RemoveOrderItemAsync(orderItemId);
            return Redirect("/cart");
        }

        public async Task<ActionResult> OnPostUpdateItem(string orderItemId, long quantity) 
        {
            OrderProduct? orderItem = await _OrderItemRepository.GetOrderItemAsync(orderItemId);
            if (orderItem is null) 
            {
                _Logger.LogError("Order item is null");
                return StatusCode(500);
            }

            orderItem.Quantity = quantity;
            await _OrderItemRepository.UpdateOrderItemAsync(orderItem);
            return StatusCode(200);
        }

        public async Task<PartialViewResult> OnGetPricePartial() 
        {
			List<OrderProduct>? orderItems = await GetOrderItemsForCurrentUser();
            if (orderItems is null)
                return Partial("_PricePartial", 0.0);

            TotalPrice = CalculateTotalPrice(orderItems);
			return Partial("_PricePartial", TotalPrice);
		}


		public async Task<Product?> GetProductAsync(string productId) 
        {
            return await _ProductRepository.GetProductByIdAsync(productId);
        }

        public async Task<List<OrderProduct>?> GetOrderItemsForUser(string userId) 
        {
            Order = await _OrderService.GetPendingOrderForUserAsync(userId);
            if (Order is null)
            {
                _Logger.LogWarning("User doesn't have any orders");
                return null;
            }

            return await _OrderItemRepository.GetOrderItemsForOrderAsync(Order.Id);
        }

        public async Task<List<OrderProduct>?> GetOrderItemsForCurrentUser() 
        {
            string userId = _UserManager.GetUserId(User);
            return await GetOrderItemsForUser(userId);
        }

        [BindProperty]
        public List<OrderProduct>? OrderItems { get; set; }
        public decimal TotalPrice { get; set; }

        public Order? Order { get; set; } = null;
        public int[] Quantities = new int[] 
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
            11, 12, 13, 14, 15, 16, 17, 18, 19, 20
        };

        private decimal CalculateTotalPrice(List<OrderProduct> orderItems)
        {
            return orderItems.Sum((x) =>
            {
                Product? product = _ProductRepository.GetProductById(x.ProductId);
                if (product is null)
                    return 0;

                return product.ProductPrice * x.Quantity;
            }) / 100m;
        }

        private OrderService _OrderService;
        private IOrderProductsRepository _OrderItemRepository;
        private IProductRepository _ProductRepository;
        private StripeService _StripeService;
        private StripeSettings _StripeSettings;
        private UserManager<User> _UserManager;
        private ILogger<OrdersModel> _Logger;
    }
}
