using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrapheneTrace.Model
{
    [Table("appointments")]
    public class Appointment
    {
        [Key]
        [Column("appointment_id")]
        public int AppointmentId { get; set; }

        [Column("patient_id")]
        public int PatientId { get; set; }

        [Column("clinician_id")]
        public int ClinicianId { get; set; }

        [Column("appointment_time")]
        public DateTime AppointmentTime { get; set; }

        [Column("status")]
        public string Status { get; set; } = "Scheduled"; // Scheduled, Completed, Cancelled

        [Column("notes")]
        public string? Notes { get; set; }

        public Patient? Patient { get; set; }
        public Clinician? Clinician { get; set; }
    }
}
