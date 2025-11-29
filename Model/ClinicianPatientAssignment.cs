<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrapheneTrace.Model
{
    [Table("clinician_patient_assignment")]
    public class ClinicianPatientAssignment
    {
        [Key]
        [Column("assignment_id")]
        public int AssignmentId { get; set; }

        [Column("patient_id")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;

        [Column("clinician_id")]
        public int ClinicianId { get; set; }
        public Clinician Clinician { get; set; } = null!;

        [Column("is_primary")]
        public bool IsPrimary { get; set; }

        [Column("assigned_at")]
        public DateTime AssignedAt { get; set; }
    }
}

=======
namespace GrapheneTrace.Model
{
    public class ClinicianPatientAssignment
    {
        public int AssignmentId { get; set; }
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }

        public int ClinicianId { get; set; }
        public Clinician Clinician { get; set; }

        public bool IsPrimary { get; set; }
        public DateTime AssignedAt { get; set; }
    }
}
>>>>>>> e90a0f6ed46d6e329231b29efb5338278c92ab88
