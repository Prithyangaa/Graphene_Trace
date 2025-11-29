using System;
using System.Collections.Generic;

namespace GrapheneTrace.Model
{
    public class SystemOverviewViewModel
    {
        // USERS
        public int TotalUsers { get; set; }
        public int TotalPatients { get; set; }
        public int TotalClinicians { get; set; }
        public int TotalAdmins { get; set; }

        // ALERTS
        public int TotalAlerts { get; set; }     // REQUIRED
        public int OpenAlerts { get; set; }
        public int CriticalAlerts { get; set; }
        public int WarningAlerts { get; set; }

        // DEVICES
        public int ActiveDevices { get; set; }
        public int OfflineDevices { get; set; }

        // SYSTEM HEALTH
        public string SystemStatus { get; set; } = "Operational";
        public DateTime LastUpdated { get; set; }

        // PERFORMANCE METRICS
        public double UptimePercentage { get; set; }
        public int ActiveSessions { get; set; }
        public int ActiveUsers { get; set; }
        public double AvgResponseTimeMs { get; set; }
        public double CpuUsage { get; set; }
        public double RamUsage { get; set; }

        // ACTIVITY GRAPH
        public List<string> ActivityLabels { get; set; } = new();
        public List<int> ActivityCounts { get; set; } = new();

        // BACKUP
        public DateTime LastBackup { get; set; }
    }
}

