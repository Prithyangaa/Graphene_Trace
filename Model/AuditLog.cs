<<<<<<< HEAD
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

=======
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
>>>>>>> e90a0f6ed46d6e329231b29efb5338278c92ab88
