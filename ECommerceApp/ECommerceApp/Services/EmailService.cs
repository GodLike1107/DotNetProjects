using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using ECommerceApp.Data; // Add this using directive

namespace ECommerceApp.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task SendOrderConfirmationAsync(string toEmail, string userName, Order order)
        {
            var apiKey = _configuration["SendGrid:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("SendGrid API key not found in configuration.");
            }

            var fromEmail = _configuration["SendGrid:FromEmail"] ?? "noreply@ecommerceapp.com";
            var fromName = _configuration["SendGrid:FromName"] ?? "ECommerceApp";

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, fromName);
            var to = new EmailAddress(toEmail, userName);
            var subject = "Order Confirmation - ECommerceApp";
            var plainTextContent = $"Dear {userName},\n\nYour order has been placed successfully!\nOrder ID: {order.Id}\nTotal: {order.Total:C}\nOrder Date: {order.OrderDate}\n\nThank you for shopping with us!";
            var htmlContent = $"<h3>Dear {userName},</h3><p>Your order has been placed successfully!</p><p><strong>Order ID:</strong> {order.Id}</p><p><strong>Total:</strong> {order.Total:C}</p><p><strong>Order Date:</strong> {order.OrderDate}</p><p>Thank you for shopping with us!</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            await client.SendEmailAsync(msg);
        }
    }
}