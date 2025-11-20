namespace GrapheneTrace.Model
{
    public class Clinician
    {
        public int ClinicianId { get; set; }
        public int UserId { get; set; }
        public string? Speciality { get; set; }
        public bool IsActive { get; set; }
    }
}
