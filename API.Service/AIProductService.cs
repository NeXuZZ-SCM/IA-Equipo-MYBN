using API.Model;
using API.Repository;
using Microsoft.ML;
using Microsoft.ML.Trainers;

namespace API.Service
{
    public class AIProductService : IAIProductService
    {
        //1. Do remember to replace amazon0302.txt with dataset from https://snap.stanford.edu/data/amazon0302.html
        //2. Replace column names with ProductID and CoPurchaseProductID. It should look like this:
        //   ProductID	CoPurchaseProductID
        //   0	1
        //   0  2
        private static string BaseDataSetRelativePath = @"data";
        private static string TrainingDataRelativePath = $"{BaseDataSetRelativePath}/Amazon0302.txt";
        private static string TrainingDataLocation = GetAbsolutePath(TrainingDataRelativePath);

        private static string BaseModelRelativePath = @"../../../Model";
        private static string ModelRelativePath = $"{BaseModelRelativePath}/model.zip";
        private static string ModelPath = GetAbsolutePath(ModelRelativePath);

        private readonly AIProductRepository _repository;

        public AIProductService(AIProductRepository repository)
        {
            _repository = repository;
        }

        public CopurchasePrediction GetProductInfo(uint idProduct, uint idCoProduct)
        {
            //STEP 1: Create MLContext to be shared across the model creation workflow objects 
            MLContext mlContext = new MLContext();

            //STEP 2: Read the trained data using TextLoader by defining the schema for reading the product co-purchase dataset
            //        Do remember to replace amazon0302.txt with dataset from https://snap.stanford.edu/data/amazon0302.html
            //AIProductRepository aIProductRepository = new AIProductRepository();
            IDataView traindata = _repository.GetProductInfo(mlContext, TrainingDataLocation);

            //STEP 3: Your data is already encoded so all you need to do is specify options for MatrxiFactorizationTrainer with a few extra hyperparameters
            //        LossFunction, Alpa, Lambda and a few others like K and C as shown below and call the trainer. 
            MatrixFactorizationTrainer.Options options = new MatrixFactorizationTrainer.Options();
            options.MatrixColumnIndexColumnName = nameof(ProductEntry.ProductID);
            options.MatrixRowIndexColumnName = nameof(ProductEntry.CoPurchaseProductID);
            options.LabelColumnName = "Label";
            options.LossFunction = MatrixFactorizationTrainer.LossFunctionType.SquareLossOneClass;
            options.Alpha = 0.01;
            options.Lambda = 0.025;
            // For better results use the following parameters
            //options.K = 100;
            //options.C = 0.00001;

            //Step 4: Call the MatrixFactorization trainer by passing options.
            var est = mlContext.Recommendation().Trainers.MatrixFactorization(options);

            //STEP 5: Train the model fitting to the DataSet
            //Please add Amazon0302.txt dataset from https://snap.stanford.edu/data/amazon0302.html to Data folder if FileNotFoundException is thrown.
            ITransformer model = est.Fit(traindata);

            //STEP 6: Create prediction engine and predict the score for Product 63 being co-purchased with Product 3.
            //        The higher the score the higher the probability for this particular productID being co-purchased 
            var predictionengine = mlContext.Model.CreatePredictionEngine<ProductEntry, CopurchasePrediction>(model);
            CopurchasePrediction prediction = predictionengine.Predict(
                new ProductEntry()
                {
                    ProductID = idProduct,
                    CoPurchaseProductID = idCoProduct
                });

            Console.WriteLine("\n For ProductID = 5 and  CoPurchaseProductID = 63 the predicted score is " + Math.Round(prediction.Score, 1));
            Console.WriteLine("=============== End of process, hit any key to finish ===============");
            //Console.ReadKey();

            return prediction;

        }


        public static string GetAbsolutePath(string relativeDatasetPath)
        {
            //FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            FileInfo _dataRoot = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);

            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativeDatasetPath);

            return fullPath;
        }

        public async Task<ProductCoPurchase?> GetById(int id)
        {
            return await _repository.Get(id);
        }

        public async Task<ProductCoPurchase> Crear(ProductCoPurchase productCoPurchase)
        {
            return await _repository.Insert(productCoPurchase);
        }
    }
}
