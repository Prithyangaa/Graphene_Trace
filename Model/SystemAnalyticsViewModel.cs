namespace GrapheneTrace.Model
{
    public class SystemAnalyticsViewModel
    {
        // ---------------------------
        // MAIN METRICS
        // ---------------------------
        public int TotalUsers { get; set; }
        public int TotalPatients { get; set; }        // ✅ REQUIRED FIELD
        public int TotalClinicians { get; set; }
        public int TotalAdmins { get; set; }

        public int ActiveDevices { get; set; }
        public int AlertsToday { get; set; }
        public string SystemUptime { get; set; } = "0%";

        // ---------------------------
        // CHART DATA
        // ---------------------------
        public List<string> UsageLabels { get; set; } = new();
        public List<int> UsageCounts { get; set; } = new();

        public List<string> PerformanceLabels { get; set; } = new();
        public List<int> PerformanceCounts { get; set; } = new();
    }
}

