using Microsoft.ML.Data;

namespace Proyecto_Tesis.ML
{
    public class ModelOutput
    {
        [ColumnName("PredictedLabel")]
        public bool Prediccion { get; set; }

        public float Probability { get; set; }
        public float Score { get; set; }
    }
}
