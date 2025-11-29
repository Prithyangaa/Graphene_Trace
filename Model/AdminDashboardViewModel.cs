using System;

namespace GrapheneTrace.Model
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalPatients { get; set; }
        public int TotalClinicians { get; set; }
        public int TotalAdmins { get; set; }

        public int OpenAlerts { get; set; }
        public int CriticalAlerts { get; set; }
        public int WarningAlerts { get; set; }

        // Since you have no device table or device tracking:
        public int ActiveDevices { get; set; } = 0;
        public int OfflineDevices { get; set; } = 0;

        public string SystemStatus { get; set; } = "Operational";
        public DateTime LastUpdated { get; set; }
    }
}

