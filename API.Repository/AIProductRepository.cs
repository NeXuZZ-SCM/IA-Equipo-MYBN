using API.Model;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Repository
{
    public class AIProductRepository
    {

        public IDataView GetProductInfo(MLContext mlContext, string trainingDataLocation)
        {
            return mlContext.Data.LoadFromTextFile(path: trainingDataLocation,
                                                      columns: new[]
                                                      {
                                                                    new TextLoader.Column("Label", DataKind.Single, 0),
                                                          new TextLoader.Column(name:nameof(ProductEntry.ProductID), dataKind:DataKind.UInt32, source: new [] { new TextLoader.Range(0) }, keyCount: new KeyCount(262111)),
                                                          new TextLoader.Column(name:nameof(ProductEntry.CoPurchaseProductID), dataKind:DataKind.UInt32, source: new [] { new TextLoader.Range(1) }, keyCount: new KeyCount(262111))
                                                      },
            hasHeader: true,
                                                      separatorChar: '\t');
        }
    }
}
