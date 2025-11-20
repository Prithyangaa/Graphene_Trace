namespace GrapheneTrace.Model
{
    public class AlertEscalationHistory
    {
        public int EscalationId { get; set; }
        public int AlertId { get; set; }
        public required string FromSeverity { get; set; }
        public required string ToSeverity { get; set; }
        public DateTime EscalatedAt { get; set; }
        public string? Comments { get; set; }
    }
}
