namespace Proyecto_Tesis.Models
{
    public class InteractionLog
    {
        public int Id { get; set; }
        public string UserToken { get; set; } = string.Empty;

        public string ActionType { get; set; } = string.Empty;
        public double LatencyMs { get; set; }

        public int InputSize { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
