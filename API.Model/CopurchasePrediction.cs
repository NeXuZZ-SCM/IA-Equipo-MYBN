using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Model
{
    public class CopurchasePrediction
    {
        public float Score { get; set; }
    }

    public class CopurchasePredictionRecommended
    {
        public float Id { get; set; }
        public float Score { get; set; }
    }
}
