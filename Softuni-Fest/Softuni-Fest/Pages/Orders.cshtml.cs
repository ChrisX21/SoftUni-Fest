using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Softuni_Fest.Interfaces;
using Softuni_Fest.Models;
using Softuni_Fest.Services;
using Stripe.Checkout;
using System.ComponentModel.DataAnnotations;

namespace Softuni_Fest.Pages
{
    [Authorize(Roles = Roles.Client)]
    public class OrdersModel : PageModel
    {
        public OrdersModel(IOrderRepository orderRepository,
                        IOrderProductsRepository orderProductsRepository,
                        IProductRepository productRepository,
                        StripeService striveService,
                        UserManager<User> userManager,
                        ILogger<OrdersModel> logger)
        {
            _OrderRepository = orderRepository;
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
        }

        public async Task OnGet()
        {
            OrderItems = await GetOrderItemsForCurrentUser();
            if (OrderItems is null)
                return;

            Quantities = new long[OrderItems.Count];
            for (int i = 0; i < OrderItems.Count; i++) 
            {
                Quantities[i] = OrderItems[i].Quantity;
            }
        }

        public async Task<ActionResult> OnPost()
        {
            OrderItems = await GetOrderItemsForCurrentUser();
            if (OrderItems is null || Order is null)
                return Page();

            for (int i = 0; i < Quantities.Length; i++)
                OrderItems[i].Quantity = Quantities[i];

            _Logger.LogInformation("Checking out");
            Session? session = await _StripeService.Checkout(OrderItems);

            if (session is null)
            {
                _Logger.LogError("Couldn't create session");
                return StatusCode(500);
            }

            if (!await _OrderRepository.RemoveOrderAsync(Order)) 
            {
                await _StripeService.ExpireOrderAsync(session.Id);
                _Logger.LogError("Couldn't finish the order");
                return StatusCode(500);
            }

            return Redirect(session.Url);
        }

        public async Task<ActionResult> OnPostRemoveItem(string orderItemId) 
        {
            await _OrderItemRepository.RemoveOrderAsync(orderItemId);
            return Redirect("/Orders");
        }

        public async Task<Product?> GetProductAsync(string productId) 
        {
            return await _ProductRepository.GetProductByIdAsync(productId);
        }

        public Product? GetProduct(string productId) 
        {
            return _ProductRepository.GetProductById(productId);
        }

        public async Task<List<OrderProduct>?> GetOrderItemsForUser(string userId) 
        {
            Order = await _OrderRepository.GetOrderForUserAsync(userId);
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
        [Range(1, int.MaxValue, ErrorMessage = "The {0} should be between {1} {2}")]
        [Display(Name = "Quantity")]
        public long[] Quantities { get; set; }

        public List<OrderProduct>? OrderItems { get; set; } = null;
        public Order? Order { get; set; } = null;

        private IOrderRepository _OrderRepository;
        private IOrderProductsRepository _OrderItemRepository;
        private IProductRepository _ProductRepository;
        private StripeService _StripeService;
        private UserManager<User> _UserManager;
        private ILogger<OrdersModel> _Logger;
    }
}
