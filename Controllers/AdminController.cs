using GrapheneTrace.Data;
using GrapheneTrace.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrapheneTrace.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
<<<<<<< HEAD
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            var model = new AdminDashboardViewModel
            {
                TotalUsers = _context.Users.Count(),
                TotalPatients = _context.Patients.Count(),
                TotalClinicians = _context.Clinicians.Count(),
                TotalAdmins = _context.Users.Count(u => u.RoleId == 1),

                OpenAlerts = _context.Alerts.Count(a => a.Status == "Open"),
                CriticalAlerts = _context.Alerts.Count(a => a.Severity == "Critical"),
                WarningAlerts = _context.Alerts.Count(a => a.Severity == "Medium" || a.Severity == "High"),

                ActiveDevices = _context.SensorReadings
                    .GroupBy(r => r.PatientId)
                    .Count(),

                OfflineDevices = 0,
                SystemStatus = "Operational",
                LastUpdated = DateTime.UtcNow
            };

            return View(model);
        }

        public IActionResult Users()
        {
            var users = _context.Users
                .Include(u => u.Role)
                .OrderBy(u => u.FullName)
=======
        {
            _context = context;
        }

        // =======================
        // ADMIN DASHBOARD
        // =======================
        public IActionResult Index()
        {
            return View();
        }

        // =======================
        // VIEW ALL USERS
        // =======================
        public IActionResult Users()
        {
            var users = _context.Users
                .Include(r => r.Role)
>>>>>>> e90a0f6ed46d6e329231b29efb5338278c92ab88
                .ToList();

            return View(users);
        }

<<<<<<< HEAD
        // -------------------------------------------
        // SYSTEM ALERTS PAGE
        // -------------------------------------------
        public IActionResult SystemAlerts()
        {
            var alerts = _context.Alerts
                .OrderByDescending(a => a.CreatedAt)
                .ToList();

            return View(alerts);
        }

        // -------------------------------------------
        // DEVICES PAGE
        // -------------------------------------------
        public IActionResult Devices()
        {
            var latest = _context.SensorReadings
                .GroupBy(r => r.PatientId)
                .Select(g => g.OrderByDescending(r => r.Timestamp).First())
                .ToList();

            var model = latest.Select(r => new DeviceViewModel
            {
                PatientId = r.PatientId,
                PatientName = _context.Patients.FirstOrDefault(p => p.PatientId == r.PatientId)?.FullName ?? "Unknown",
                LastUpdated = r.Timestamp,
                Pressure = r.Pressure,
                Temperature = r.Temperature
            }).ToList();

            return View(model);
        }

        // -------------------------------------------
        // SYSTEM LOGS
        // -------------------------------------------
        public IActionResult Logs()
        {
            var logs = _context.AuditLogs
                .OrderByDescending(l => l.ActionTimestamp)
                .ToList();

            return View(logs);
        }

        // -------------------------------------------
        // ANALYTICS PAGE (ADDED AS REQUESTED)
        // -------------------------------------------
        public IActionResult Analytics()
        {
            var model = new SystemAnalyticsViewModel
            {
                
                TotalUsers = _context.Users.Count(),
                TotalPatients = _context.Patients.Count(),   // ← MUST BE HERE
                TotalClinicians = _context.Clinicians.Count(),
                TotalAdmins = _context.Users.Count(u => u.RoleId == 1),

                ActiveDevices = _context.SensorReadings.GroupBy(r => r.PatientId).Count(),
                 AlertsToday = _context.Alerts.Count(a => a.CreatedAt.Date == DateTime.UtcNow.Date),
                 SystemUptime = "99.98%",

            };

            return View(model);
        }

        public IActionResult Settings()
{
    // Fetch basic stats
    var vm = new SettingsViewModel
    {
        SystemStatus = "Operational",

        TotalUsers = _context.Users.Count(),
        TotalPatients = _context.Patients.Count(),
        TotalAlerts = _context.Alerts.Count(),

        // Example backup timestamp (If you have real backup logs, replace this)
        LastBackup = DateTime.UtcNow.AddDays(-1),

        // Security info pulled from auth_metadata table
        FailedLoginAttempts = _context.AuthMetadata.Sum(a => a.FailedAttempts),
        LastPasswordChange = _context.AuthMetadata
            .OrderByDescending(a => a.PasswordUpdatedAt)
            .FirstOrDefault()?.PasswordUpdatedAt,

        AutoBackupEnabled = true,
        BackupFrequency = "Daily"
    };

    return View(vm);
}
// -----------------------------
// SYSTEM OVERVIEW PAGE (placeholder)
// -----------------------------
// ==========================
// SYSTEM OVERVIEW PAGE
// ==========================
public IActionResult SystemOverview()
{
    var model = new SystemOverviewViewModel
    {
        TotalUsers = _context.Users.Count(),
        TotalPatients = _context.Patients.Count(),
        TotalClinicians = _context.Clinicians.Count(),

        TotalAlerts = _context.Alerts.Count(),
        OpenAlerts = _context.Alerts.Count(a => a.Status == "Open"),

        LastBackup = DateTime.UtcNow.AddDays(-2), // replace when backup feature implemented
        SystemStatus = "Operational",
        LastUpdated = DateTime.UtcNow
    };

    return View(model);
}

// ==========================
// BACKUP / RESTORE PAGE
// ==========================
public IActionResult BackupRestore()
{
    var model = new BackupRestoreViewModel
    {
        LastBackup = DateTime.UtcNow.AddDays(-2),
        TotalTables = 12,
        TotalRecords = _context.Users.Count()
                        + _context.Patients.Count()
                        + _context.Clinicians.Count()
                        + _context.Alerts.Count()
    };

    return View(model);
}



=======
        // =======================
        // CREATE ADMIN
        // =======================
        [HttpGet]
        public IActionResult CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateAdmin(string fullName, string email, string password)
        {
            var adminRole = _context.Roles.FirstOrDefault(r => r.RoleName == "Admin");

            var user = new User
            {
                FullName = fullName,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                RoleId = adminRole!.RoleId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Users");
        }

        // =======================
        // CREATE CLINICIAN
        // =======================
        [HttpGet]
        public IActionResult CreateClinician()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateClinician(string fullName, string email, string password, string speciality)
        {
            var role = _context.Roles.First(r => r.RoleName == "Clinician");

            var user = new User
            {
                FullName = fullName,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                RoleId = role.RoleId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            var clinician = new Clinician
            {
                UserId = user.UserId,
                Speciality = speciality,
                IsActive = true
            };

            _context.Clinicians.Add(clinician);
            _context.SaveChanges();

            return RedirectToAction("Users");
        }

        // =======================
        // CREATE PATIENT
        // =======================
        [HttpGet]
        public IActionResult CreatePatient()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreatePatient(string fullName, int age, string gender, string contact, string riskLevel)
        {
            var patient = new Patient
            {
                FullName = fullName,
                Age = age,
                Gender = gender,
                ContactNumber = contact,
                RiskLevel = riskLevel,
                CurrentStatus = "Active",
                CreatedAt = DateTime.UtcNow
            };

            _context.Patients.Add(patient);
            _context.SaveChanges();

            return RedirectToAction("Users");
        }
>>>>>>> e90a0f6ed46d6e329231b29efb5338278c92ab88
    }
}


