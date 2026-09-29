using Microsoft.ML;
using Microsoft.ML.Data;

namespace Proyecto_Tesis.ML
{
    public class MLService
    {
        private readonly MLContext _ml;

        public MLService()
        {
            _ml = new MLContext(seed: 42);
        }

        public (ITransformer modelo, BinaryClassificationMetrics? metricas) Entrenar(List<ModelInput> data)
        {
            var dataView = _ml.Data.LoadFromEnumerable(data);

            var pipeline = _ml.Transforms.Concatenate("Features",
                    nameof(ModelInput.Latencia),
                    nameof(ModelInput.Estres),
                    nameof(ModelInput.Ansiedad),
                    nameof(ModelInput.Depresion),
                    nameof(ModelInput.HorasUso),
                    nameof(ModelInput.DiasUso),
                    nameof(ModelInput.TipoUsoReemplazo))
                .Append(_ml.Transforms.NormalizeMinMax("Features"))
                .Append(_ml.BinaryClassification.Trainers.SdcaLogisticRegression());

            var split = _ml.Data.TrainTestSplit(dataView, testFraction: 0.2);

            var modelo = pipeline.Fit(split.TrainSet);

            var testData = _ml.Data.CreateEnumerable<ModelInput>(split.TestSet, false).ToList();

            bool hayAlto = testData.Any(x => x.Riesgo);
            bool hayBajo = testData.Any(x => !x.Riesgo);

            BinaryClassificationMetrics? metricas = null;

            if (hayAlto && hayBajo)
            {
                var predicciones = modelo.Transform(split.TestSet);
                metricas = _ml.BinaryClassification.Evaluate(predicciones);
            }

            return (modelo, metricas);
        }

        public ModelOutput Predecir(ITransformer modelo, ModelInput input)
        {
            var predEngine = _ml.Model.CreatePredictionEngine<ModelInput, ModelOutput>(modelo);
            return predEngine.Predict(input);
        }
    }
}