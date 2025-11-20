using Microsoft.EntityFrameworkCore;
using GrapheneTrace.Model;

namespace GrapheneTrace.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Clinician> Clinicians { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<ClinicianPatientAssignment> ClinicianPatientAssignments { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<AlertEscalationHistory> AlertEscalationHistories { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<PatientRiskHistory> PatientRiskHistories { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<SensorReading> SensorReadings { get; set; }
        public DbSet<AuthMetadata> AuthMetadata { get; set; }
    }

}
