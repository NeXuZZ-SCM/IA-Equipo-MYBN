using API.Model;
using API.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using System.Linq;

namespace API.Service
{
    public class AIProductService : IAIProductService
    {
        private readonly AIProductRepository _repository;

        public AIProductService(AIProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<CopurchasePrediction> GetProductInfo(uint idProduct, uint idCoProduct)
        {
            MLContext mlContext = new();

            if(File.Exists("model.zip") is not true)
            {
                await TrainML();
            }

            ITransformer model = mlContext.Model.Load("model.zip", out DataViewSchema modelSchema);
 
            var predictionengine = mlContext.Model.CreatePredictionEngine<ProductEntry, CopurchasePrediction>(model);

            CopurchasePrediction prediction = predictionengine.Predict(
                new ProductEntry()
                {
                    ProductID = idProduct,
                    CoPurchaseProductID = idCoProduct
                });

            return prediction;
        }

        public async Task<ProductCoPurchase?> GetById(int id)
        {
            return await _repository.Get(id);
        }

        public async Task<ProductCoPurchase> Crear(ProductCoPurchase productCoPurchase)
        {
            return await _repository.Insert(productCoPurchase);
        }

        public async Task TrainML()
        {
            MLContext mlContext = new();

            var traindata = await _repository.GetProductInfo(mlContext);

            MatrixFactorizationTrainer.Options options = new MatrixFactorizationTrainer.Options();
            options.MatrixColumnIndexColumnName = nameof(ProductEntry.ProductID);
            options.MatrixRowIndexColumnName = nameof(ProductEntry.CoPurchaseProductID);
            options.LabelColumnName = "Label";
            options.LossFunction = MatrixFactorizationTrainer.LossFunctionType.SquareLossOneClass;
            options.Alpha = 0.01;
            options.Lambda = 0.025;

            var est = mlContext.Recommendation().Trainers.MatrixFactorization(options);

            ITransformer model = est.Fit(traindata);

            mlContext.Model.Save(model, traindata.Schema, "model.zip");
        }

        public async Task<IEnumerable<(float CoProductId, float Score)>> GetRecommendedProducts(int id, int count)
        {
            var mlContext = new MLContext();

            // Cargar el modelo guardado
            ITransformer model = mlContext.Model.Load("model.zip", out DataViewSchema modelSchema);

            // Crear el PredictionEngine
            var predictionEngine = mlContext.Model.CreatePredictionEngine<ProductEntry, CopurchasePrediction>(model);

            // Definir el ID del producto para el cual deseas obtener recomendaciones
            var productId = id; // Cambia esto por el ID del producto de interés

            // Obtener todos los CoProductId distintos desde el conjunto de datos (asumiendo que tienes acceso a los datos originales)
            // Si no tienes acceso a los datos originales, debes tener una lista de todos los posibles CoProductId.
            //var allProducts = new List<float> { 101, 102, 103, 104, 105 }; // Lista de ejemplo, debes sustituirla por los coproductos reales
            var allProducts = await _repository.All();
            var predictions = new List<(float CoProductId, float Score)>();


            foreach (var coProduct in allProducts)
            {
                var prediction = predictionEngine.Predict(new ProductEntry
                {
                    ProductID = Convert.ToUInt32(productId),
                    CoPurchaseProductID = Convert.ToUInt32(coProduct.CoPurchaseProductId)
                });

                predictions.Add((CoProductId: coProduct.CoPurchaseProductId, Score: prediction.Score));
            }

            // Obtener los top 3 coproductos con mayor probabilidad
            IEnumerable<(float CoProductId, float Score)> topProducts = predictions
                .OrderByDescending(p => p.Score).Distinct()
                .Take(count);


            return topProducts;
        }
    }
}
