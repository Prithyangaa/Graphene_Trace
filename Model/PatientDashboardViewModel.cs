using System;
using System.Collections.Generic;

namespace GrapheneTrace.Model
{
    public class PatientDashboardViewModel
    {
        // Identity
        public int PatientId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? ContactNumber { get; set; }
        public string? RiskLevel { get; set; }
        public string? CurrentStatus { get; set; }
        public DateTime CreatedAt { get; set; }

        // Account
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;

        // Clinician
        public string? ClinicianName { get; set; }
        public string? ClinicianSpeciality { get; set; }

        // Heatmap metrics (simple summary for tiles)
        public double PeakPressureIndex { get; set; }
        public double ContactAreaPercent { get; set; }
        public double AveragePressure { get; set; }
        public string PressureStatus { get; set; } = "Stable";
        public string PressureStatusLabel { get; set; } = "";
        public string PressureStatusSummary { get; set; } = "";

        public double PressureVariability { get; set; }
        public double StabilityIndex { get; set; }
        public double MovementIndex { get; set; }
        public int HotspotCount { get; set; }
        public string HeatmapImageBase64 { get; set; } = "";

        public bool HasHeatmapData { get; set; }
        public bool HasPastHeatmapData { get; set; }

        // Lists
        public List<Alert> RecentAlerts { get; set; } = new();
        public List<PatientMessage> RecentMessages { get; set; } = new();
    }
}
