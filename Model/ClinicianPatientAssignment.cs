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
