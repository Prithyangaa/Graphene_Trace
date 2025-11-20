namespace GrapheneTrace.Model
{
    public class Alert
    {
        public int AlertId { get; set; }
        public int PatientId { get; set; }
        public required string AlertType { get; set; }
        public required string Severity { get; set; }
        public string? AlertDescription { get; set; }
        public required string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public required DateTime ResolvedAt { get; set; }
    }
}
