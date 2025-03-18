using ECommerceApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceApp.ML
{
    public static class RecommendationDataLoader
    {
        public static List<RecommendationData> LoadData(ApplicationDbContext context)
        {
            var purchases = context.OrderItems
                .Include(oi => oi.Order)
                .Select(oi => new { oi.Order.UserId, oi.ProductId })
                .Distinct()
                .ToList();

            var data = purchases.Select(p => new RecommendationData
            {
                UserId = p.UserId,
                ProductId = p.ProductId,
                Label = 1f // Purchased
            }).ToList();

            return data;
        }
    }
}