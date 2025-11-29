using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrapheneTrace.Model
{
    [Table("patients")]
    public class Patient
    {
        [Key]
        [Column("patient_id")]
        public int PatientId { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }   // 🔹 NEW

        [Column("full_name")]
        public string? FullName { get; set; }

        [Column("age")]
        public int? Age { get; set; }

        [Column("gender")]
        public string? Gender { get; set; }

        [Column("contact_number")]
        public string? ContactNumber { get; set; }

        [Column("risk_level")]
        public string? RiskLevel { get; set; }

        [Column("current_status")]
        public string? CurrentStatus { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
