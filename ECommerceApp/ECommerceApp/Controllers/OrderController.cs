using ECommerceApp.Data;
using ECommerceApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Security.Claims;

namespace ECommerceApp.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly string _stripeSecretKey;

        public OrderController(ApplicationDbContext context, EmailService emailService, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _emailService = emailService;
            _userManager = userManager;
            _stripeSecretKey = configuration["Stripe:SecretKey"] ?? throw new InvalidOperationException("Stripe SecretKey not found in configuration.");
            StripeConfiguration.ApiKey = _stripeSecretKey;
        }

        public async Task<IActionResult> Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            var totalAmount = (long)(cartItems.Sum(item => item.Product.Price * item.Quantity) * 100);

            var options = new PaymentIntentCreateOptions
            {
                Amount = totalAmount,
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" },
                Metadata = new Dictionary<string, string>
                {
                    { "UserId", userId ?? throw new InvalidOperationException("UserId is null.") }
                }
            };
            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);

            ViewBag.ClientSecret = paymentIntent.ClientSecret;

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Total = cartItems.Sum(item => item.Product.Price * item.Quantity),
                PaymentIntentId = paymentIntent.Id
            };

            foreach (var item in cartItems)
            {
                order.Items.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product.Price
                });
            }

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                string userEmail = user.Email ?? "no-email@example.com";  // Default email if null
                string userName = user.UserName ?? "Customer"; // Default username if null

                await _emailService.SendOrderConfirmationAsync(userEmail, userName, order);
            }

            return View(order);
        }
    }
}
