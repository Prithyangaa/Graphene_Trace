<<<<<<< HEAD
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

=======
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
>>>>>>> b283525eb90ca6690cffbf8e55180a5495858727
