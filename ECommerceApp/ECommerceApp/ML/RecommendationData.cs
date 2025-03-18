namespace ECommerceApp.ML
{
    public class RecommendationData
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public float Label { get; set; } // 1 for purchased, 0 for not purchased
    }

    public class ProductPrediction
    {
        public float Score { get; set; }
    }
}