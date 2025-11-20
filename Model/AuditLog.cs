namespace GrapheneTrace.Model
{
    public class AuditLog
    {
        public int AuditId { get; set; }
        public int? UserId { get; set; }
        public required string ActionType { get; set; }
        public required string TableName { get; set; }
        public int? RecordId { get; set; }
        public DateTime ActionTimestamp { get; set; }
        public string? Details { get; set; }
    }
}
