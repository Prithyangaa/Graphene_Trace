namespace GrapheneTrace.Model
{
    public class SettingsViewModel
    {
        // SYSTEM INFORMATION
        public string SystemStatus { get; set; } = "Operational";
        public DateTime LastBackup { get; set; }
        public int TotalUsers { get; set; }
        public int TotalPatients { get; set; }
        public int TotalAlerts { get; set; }

        // SECURITY SETTINGS
        public int FailedLoginAttempts { get; set; }
        public DateTime? LastPasswordChange { get; set; }

        // BACKUP SETTINGS
        public bool AutoBackupEnabled { get; set; }
        public string BackupFrequency { get; set; } = "Daily";
    }
}

