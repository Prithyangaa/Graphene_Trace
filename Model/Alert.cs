using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrapheneTrace.Model
{
    [Table("alerts")]
    public class Alert
    {
        [Key]
        [Column("alert_id")]
        public int AlertId { get; set; }

        [Column("patient_id")]
        public int PatientId { get; set; }

        [Column("alert_type")]
        public string AlertType { get; set; } = string.Empty;

        [Column("severity")]
        public string Severity { get; set; } = string.Empty;

        [Column("alert_description")]
        public string? AlertDescription { get; set; }

        [Column("status")]
        public string Status { get; set; } = "Open";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("resolved_at")]
        public DateTime? ResolvedAt { get; set; }
    }
}

