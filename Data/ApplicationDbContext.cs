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
<<<<<<< HEAD

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Patient> Patients { get; set; } = null!;
        public DbSet<Clinician> Clinicians { get; set; } = null!;
        public DbSet<ClinicianPatientAssignment> ClinicianPatientAssignments { get; set; } = null!;
        public DbSet<Alert> Alerts { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<PatientMessage> PatientMessages { get; set; } = null!;   // 🔹 NEW
        public DbSet<SensorReading> SensorReadings { get; set; } = null!;
        public DbSet<AlertEscalationHistory> AlertEscalationHistories { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<AuthMetadata> AuthMetadata { get; set; }

        public DbSet<Appointment> Appointments { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<ClinicianPatientAssignment>()
                .HasOne(a => a.Clinician)
                .WithMany()
                .HasForeignKey(a => a.ClinicianId);

            modelBuilder.Entity<ClinicianPatientAssignment>()
                .HasOne(a => a.Patient)
                .WithMany()
                .HasForeignKey(a => a.PatientId);

            modelBuilder.Entity<Alert>()
                .HasOne<Patient>()
                .WithMany()
                .HasForeignKey(a => a.PatientId);

            modelBuilder.Entity<Notification>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(n => n.UserId);

            modelBuilder.Entity<Notification>()
                .HasOne<Alert>()
                .WithMany()
                .HasForeignKey(n => n.AlertId);

            modelBuilder.Entity<AlertEscalationHistory>()
                .HasOne<Alert>()
                .WithMany()
                .HasForeignKey(e => e.AlertId);

            modelBuilder.Entity<AuthMetadata>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<SensorReading>()
                .HasOne<Patient>()
                .WithMany()
                .HasForeignKey(s => s.PatientId);
        }
=======

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
>>>>>>> e90a0f6ed46d6e329231b29efb5338278c92ab88
    }

}

