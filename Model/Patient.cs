namespace GrapheneTrace.Model
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string? FullName { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? ContactNumber { get; set; }
        public string? RiskLevel { get; set; }
        public string? CurrentStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
