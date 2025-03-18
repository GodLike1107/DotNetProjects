using ECommerceApp.Data;
using ECommerceApp.ML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using System.Security.Claims;

namespace ECommerceApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly MLContext _mlContext;
        private readonly ITransformer _model;
        private readonly string _modelPath = "MLModel.zip";

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
            _mlContext = new MLContext();
            _model = RecommendationModelTrainer.LoadModel(_mlContext, _modelPath);
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            ViewBag.RecommendedProducts = await GetRecommendedProducts();
            return View(products);
        }

        private async Task<List<Product>> GetRecommendedProducts()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // For non-authenticated users, return popular products
                return await _context.Products.OrderBy(p => p.Id).Take(3).ToListAsync();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var allProducts = await _context.Products.ToListAsync();

            // Use ML.NET to predict scores for this user
            var predictions = new List<(Product Product, float Score)>();
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<RecommendationData, ProductPrediction>(_model);

            foreach (var product in allProducts)
            {
                var prediction = predictionEngine.Predict(new RecommendationData
                {
                    UserId = userId,
                    ProductId = product.Id
                });
                predictions.Add((product, prediction.Score));
            }

            // Return top 3 products by predicted score
            return predictions.OrderByDescending(p => p.Score)
                .Take(3)
                .Select(p => p.Product)
                .ToList();
        }
    }
}