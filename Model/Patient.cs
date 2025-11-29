<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

=======
>>>>>>> e90a0f6ed46d6e329231b29efb5338278c92ab88
namespace GrapheneTrace.Model
{
    [Table("patients")]
    public class Patient
    {
<<<<<<< HEAD
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
=======
        public int PatientId { get; set; }
        public string? FullName { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? ContactNumber { get; set; }
        public string? RiskLevel { get; set; }
        public string? CurrentStatus { get; set; }
>>>>>>> e90a0f6ed46d6e329231b29efb5338278c92ab88
        public DateTime CreatedAt { get; set; }
    }
}
