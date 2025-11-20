namespace GrapheneTrace.Model
{
    public class PatientRiskHistory
    {
        public int HistoryId { get; set; }
        public int PatientId { get; set; }
        public required string OldRisk { get; set; }
        public required string NewRisk { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
