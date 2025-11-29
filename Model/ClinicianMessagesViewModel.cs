using System.Collections.Generic;

namespace GrapheneTrace.Model
{
    public class ClinicianMessagesViewModel
    {
        public int ClinicianId { get; set; }

        // Sidebar patient list
        public List<Patient> AssignedPatients { get; set; } = new();

        // Message thread
        public int? SelectedPatientId { get; set; }
        public string? SelectedPatientName { get; set; }
        public List<PatientMessage> Thread { get; set; } = new();

        // New message input
        public string NewMessage { get; set; } = string.Empty;
    }
}
