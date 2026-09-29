namespace Proyecto_Tesis.Models
{
    public class ResultadoML
    {
        public int Id { get; set; }
        public float Accuracy { get; set; }
        public float Precision { get; set; }
        public float Recall { get; set; }
        public float F1Score { get; set; }

        public DateTime Fecha { get; set; }
    }
}
