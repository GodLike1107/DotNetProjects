using Microsoft.ML;
using Microsoft.ML.Trainers;
using System;
using System.Collections.Generic;
using System.IO;

namespace ECommerceApp.ML
{
    public static class RecommendationModelTrainer
    {
        public static void TrainAndSaveModel(List<RecommendationData> data, string modelPath)
        {
            var mlContext = new MLContext();

            // Load data into ML.NET
            var dataView = mlContext.Data.LoadFromEnumerable(data);

            // Define the pipeline
            var pipeline = mlContext.Transforms.Conversion.MapValueToKey("UserIdEncoded", nameof(RecommendationData.UserId))
                .Append(mlContext.Transforms.Conversion.MapValueToKey("ProductIdEncoded", nameof(RecommendationData.ProductId)))
                .Append(mlContext.Recommendation().Trainers.MatrixFactorization(
                    labelColumnName: nameof(RecommendationData.Label),
                    matrixColumnIndexColumnName: "UserIdEncoded",
                    matrixRowIndexColumnName: "ProductIdEncoded",
                    numberOfIterations: 20,
                    approximationRank: 100));

            // Train the model
            var model = pipeline.Fit(dataView);

            // Save the model
            using (var fileStream = new FileStream(modelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                mlContext.Model.Save(model, dataView.Schema, fileStream);
            }
        }

        public static ITransformer LoadModel(MLContext mlContext, string modelPath)
        {
            using (var fileStream = new FileStream(modelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return mlContext.Model.Load(fileStream, out var modelInputSchema);
            }
        }
    }
}