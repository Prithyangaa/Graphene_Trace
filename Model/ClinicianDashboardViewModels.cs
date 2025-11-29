using System;
using System.Collections.Generic;

namespace GrapheneTrace.Model
{
    public class ClinicianDashboardPatientTileViewModel
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int? Age { get; set; }
        public string? RiskLevel { get; set; }
        public string? CurrentStatus { get; set; }

        // Pressure / sensor summary
        public double PressureIndex { get; set; }        // peak pressure index
        public bool HasHeatmapData { get; set; }
        public int HotspotCount { get; set; }

        // Priority & UI helpers
        public string Priority { get; set; } = "STABLE"; // CRITICAL/HIGH/MEDIUM/STABLE
        public string PriorityCssClass { get; set; } = "gt-priority-stable";
        public string PriorityLabel { get; set; } = "STABLE";
        public string SummaryMessage { get; set; } = string.Empty;

        // For "Last reading" text (you can wire to real timestamps later)
        public DateTime? LastReadingUtc { get; set; }
        public string LastReadingDisplay =>
            LastReadingUtc.HasValue ? LastReadingUtc.Value.ToLocalTime().ToString("HH:mm") : "N/A";
    }

    public class ClinicianDashboardViewModel
    {
        // Clinician identity
        public int ClinicianId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Speciality { get; set; }
        public string Email { get; set; } = string.Empty;

        // Summary stats
        public int TotalPatients { get; set; }
        public int PatientsRequiringAttention { get; set; }
        public int ActiveAlerts { get; set; }

        // Tiles
        public List<ClinicianDashboardPatientTileViewModel> PriorityPatients { get; set; } = new();


        // Messages (latest across all patients)
        public List<PatientMessage> RecentMessages { get; set; } = new();
        public int TodayAppointmentsCount { get; set; }

    }
}
