using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrapheneTrace.Model
{
    [Table("patient_messages")]
    public class PatientMessage
    {
        [Key]
        [Column("message_id")]
        public int MessageId { get; set; }

        [Column("patient_id")]
        public int PatientId { get; set; }

        [Column("clinician_id")]
        public int? ClinicianId { get; set; }

        [Column("from_clinician")]
        public bool FromClinician { get; set; }

        [Column("content")]
        public string Content { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        // navigation (optional)
        public Patient? Patient { get; set; }
        public Clinician? Clinician { get; set; }
    }
}
