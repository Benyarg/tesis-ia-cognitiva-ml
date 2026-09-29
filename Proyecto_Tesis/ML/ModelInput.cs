using Microsoft.ML.Data;

namespace Proyecto_Tesis.ML
{
    public class ModelInput
    {
        // VARIABLES
        public float Latencia { get; set; }
        public float Estres { get; set; }
        public float Ansiedad { get; set; }
        public float Depresion { get; set; }

        public float HorasUso { get; set; }
        public float DiasUso { get; set; }

        public float TipoUsoReemplazo { get; set; }

        // ETIQUETA (LO QUE EL MODELO APRENDE)

        [ColumnName("Label")]
        public bool Riesgo { get; set; }
    }
}
