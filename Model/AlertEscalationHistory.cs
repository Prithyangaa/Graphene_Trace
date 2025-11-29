<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrapheneTrace.Model
{
    [Table("alert_escalation_history")]
    public class AlertEscalationHistory
    {
        [Key]
        [Column("escalation_id")]
        public int EscalationId { get; set; }

        [Column("alert_id")]
        public int AlertId { get; set; }

        [Column("from_severity")]
        public string? FromSeverity { get; set; }

        [Column("to_severity")]
        public string? ToSeverity { get; set; }

        [Column("escalated_at")]
        public DateTime EscalatedAt { get; set; }

        [Column("comments")]
        public string? Comments { get; set; }
    }
}

=======
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
>>>>>>> b283525eb90ca6690cffbf8e55180a5495858727
