using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrapheneTrace.Model
{
    [Table("audit_log")]
    public class AuditLog
    {
        [Key]
        [Column("audit_id")]
        public int AuditId { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [Column("action_type")]
        public string ActionType { get; set; } = string.Empty;

        [Column("table_name")]
        public string TableName { get; set; } = string.Empty;

        [Column("record_id")]
        public int? RecordId { get; set; }

        [Column("action_timestamp")]
        public DateTime ActionTimestamp { get; set; }

        [Column("details")]
        public string? Details { get; set; }
    }
}

