using API.Model;
using API.Repository;
using Microsoft.ML;
using Microsoft.ML.Trainers;

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
    }
}
