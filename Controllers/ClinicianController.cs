using GrapheneTrace.Data;
using GrapheneTrace.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;

namespace GrapheneTrace.Controllers
{
    public class ClinicianController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ClinicianController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ---------------------------------------------
        // HELPERS
        // ---------------------------------------------

        private int? GetCurrentClinicianId()
        {
            return HttpContext.Session.GetInt32("ClinicianId");
        }

        private string GetPatientCsvPath(int patientId)
        {
            return Path.Combine(_env.WebRootPath, "Data", $"Patient{patientId}.csv");
        }

        // ---------------------------------------------
        // CSV METRICS PROCESSOR
        // ---------------------------------------------
        private bool TryComputeCsvMetrics(
            int patientId,
            out double peak,
            out double contact,
            out double avg,
            out double variability,
            out double stability,
            out double movement,
            out int hotspots)
        {
            peak = contact = avg = variability = stability = movement = 0;
            hotspots = 0;

            var path = GetPatientCsvPath(patientId);
            if (!System.IO.File.Exists(path)) return false;

            var lines = System.IO.File.ReadAllLines(path);
            if (lines.Length < 32) return false;

            const int size = 32;
            List<double[,]> frames = new List<double[,]>();

            // Parse frames
            for (int i = 0; i + size <= lines.Length; i += size)
            {
                double[,] frame = new double[size, size];

                for (int r = 0; r < size; r++)
                {
                    var cols = lines[i + r].Split(',').Select(double.Parse).ToArray();
                    for (int c = 0; c < size; c++)
                        frame[r, c] = cols[c];
                }

                frames.Add(frame);
            }

            if (frames.Count == 0)
                return false;

            var last = frames.Last();
            List<double> values = new();

            foreach (var v in last)
                values.Add(v);

            peak = values.Max();
            avg = values.Average();
            contact = values.Count(v => v > 30) * 100.0 / values.Count;
            double mean = avg;
            variability = Math.Sqrt(values.Average(v => Math.Pow(v - mean, 2)));
            stability = (peak - values.Min()) / (avg == 0 ? 1 : avg);
            hotspots = values.Count(v => v > 80);

            if (frames.Count > 1)
            {
                var prev = frames[^2];
                double diff = 0;

                for (int r = 0; r < size; r++)
                    for (int c = 0; c < size; c++)
                        diff += Math.Abs(last[r, c] - prev[r, c]);

                movement = diff / (size * size);
            }

            return true;
        }

        // ---------------------------------------------
        // PRIORITY LOGIC
        // ---------------------------------------------
        private (string priority, string css, string label, string summary)
            ComputePriority(Patient p, bool hasHeatmap, double peak, double contact, int hotspots)
        {
            string risk = p.RiskLevel?.ToLower() ?? "";
            string status = p.CurrentStatus?.ToLower() ?? "";

            bool critical =
                risk.Contains("critical") ||
                status.Contains("deteriorating") ||
                (hasHeatmap && (peak >= 180 || hotspots >= 10));

            bool high =
                risk.Contains("high") ||
                (hasHeatmap && peak >= 145);

            bool medium =
                risk.Contains("medium") ||
                (hasHeatmap && peak >= 120);

            if (critical)
                return ("CRITICAL", "gt-priority-critical", "Critical",
                        "Critical pressure detected");

            if (high)
                return ("HIGH", "gt-priority-high", "Attention",
                        "High pressure — needs attention");

            if (medium)
                return ("MEDIUM", "gt-priority-medium", "Moderate",
                        "Moderate pressure — review soon");

            return ("STABLE", "gt-priority-stable", "Stable",
                    "Normal readings");
        }


        // ---------------------------------------------
        // DASHBOARD
        // ---------------------------------------------
        public async Task<IActionResult> Dashboard()
        {
            var clinicianId = GetCurrentClinicianId();
            if (clinicianId == null)
                return View("NoClinicianLinked");

            var clinician = await _context.Clinicians
                .FirstOrDefaultAsync(c => c.ClinicianId == clinicianId.Value);

            if (clinician == null)
                return View("NoClinicianLinked");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == clinician.UserId);

            var assignments = await _context.ClinicianPatientAssignments
                .Include(a => a.Patient)
                .Where(a => a.ClinicianId == clinicianId.Value)
                .ToListAsync();

            List<ClinicianDashboardPatientTileViewModel> tiles = new();

            foreach (var a in assignments)
            {
                var p = a.Patient;
                if (p == null) continue;

                double peak, contact, avg, varb, stab, mov;
                int hotspots;

                bool hasHeatmap = TryComputeCsvMetrics(
                    p.PatientId,
                    out peak,
                    out contact,
                    out avg,
                    out varb,
                    out stab,
                    out mov,
                    out hotspots
                );

                var (priority, css, label, summary) =
                    ComputePriority(p, hasHeatmap, peak, contact, hotspots);

                tiles.Add(new ClinicianDashboardPatientTileViewModel
                {
                    PatientId = p.PatientId,
                    PatientName = p.FullName ?? "Patient",
                    Age = p.Age,
                    RiskLevel = p.RiskLevel,
                    CurrentStatus = p.CurrentStatus,
                    PressureIndex = peak,
                    HasHeatmapData = hasHeatmap,
                    HotspotCount = hotspots,
                    Priority = priority,
                    PriorityCssClass = css,
                    PriorityLabel = label,
                    SummaryMessage = summary,
                    LastReadingUtc = DateTime.UtcNow.AddMinutes(-5)
                });
            }

            tiles = tiles
                .OrderBy(t => t.Priority == "CRITICAL" ? 0 :
                              t.Priority == "HIGH" ? 1 :
                              t.Priority == "MEDIUM" ? 2 : 3)
                .ToList();

            var vm = new ClinicianDashboardViewModel
            {
                ClinicianId = clinician.ClinicianId,
                FullName = user?.FullName ?? "Clinician",
                Email = user?.Email ?? "",
                Speciality = clinician.Speciality,
                TotalPatients = tiles.Count,
                PatientsRequiringAttention = tiles.Count(t => t.Priority != "STABLE"),
                ActiveAlerts = 0,
                PriorityPatients = tiles,
                RecentMessages = new()
            };
            vm.TodayAppointmentsCount = await _context.Appointments
                .CountAsync(a =>
                    a.ClinicianId == clinician.ClinicianId &&
                    a.AppointmentTime.Date == DateTime.UtcNow.Date
                );

            return View(vm);
        }

        // ---------------------------------------------
        // PATIENT OVERVIEW
        // ---------------------------------------------
        public async Task<IActionResult> PatientOverview(int id)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PatientId == id);
            return View(patient);
        }

        // ---------------------------------------------
        // PATIENT LIST
        // ---------------------------------------------
        public async Task<IActionResult> PatientList()
        {
            var clinicianId = GetCurrentClinicianId();
            var assignments = await _context.ClinicianPatientAssignments
                .Include(a => a.Patient)
                .Where(a => a.ClinicianId == clinicianId)
                .ToListAsync();

            return View(assignments.Select(a => a.Patient).ToList());
        }

        // ---------------------------------------------
        // MESSAGES
        // ---------------------------------------------
        public async Task<IActionResult> Messages(int? patientId)
        {
            var clinicianId = GetCurrentClinicianId();
            if (clinicianId == null) return View("NoClinicianLinked");

            // Load assigned patients for sidebar
            var assigned = await _context.ClinicianPatientAssignments
                .Include(a => a.Patient)
                .Where(a => a.ClinicianId == clinicianId)
                .OrderBy(a => a.Patient.FullName)
                .ToListAsync();

            var vm = new ClinicianMessagesViewModel
            {
                ClinicianId = clinicianId.Value,
                AssignedPatients = assigned.Select(a => a.Patient).ToList()
            };

            // If patient is selected, load thread
            if (patientId.HasValue)
            {
                vm.SelectedPatientId = patientId.Value;

                var patient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.PatientId == patientId.Value);

                vm.SelectedPatientName = patient?.FullName ?? "Patient";

                vm.Thread = await _context.PatientMessages
                    .Where(m => m.PatientId == patientId.Value)
                    .OrderBy(m => m.CreatedAt)
                    .ToListAsync();
            }

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> SendMessageToPatient(int patientId, string content)
        {
            var clinicianId = GetCurrentClinicianId();
            if (clinicianId == null) return View("NoClinicianLinked");

            if (string.IsNullOrWhiteSpace(content))
                return RedirectToAction("Messages", new { patientId });

            var msg = new PatientMessage
            {
                PatientId = patientId,
                ClinicianId = clinicianId.Value,
                FromClinician = true,
                Content = content.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _context.PatientMessages.Add(msg);
            await _context.SaveChangesAsync();

            return RedirectToAction("Messages", new { patientId });
        }


        // ---------------------------------------------
        // APPOINTMENTS
        // ---------------------------------------------
        public async Task<IActionResult> Appointments()
        {
            var clinicianId = GetCurrentClinicianId();
            if (clinicianId == null) return View("NoClinicianLinked");

            DateTime today = DateTime.UtcNow.Date;

            // Load assigned patients
            var assigned = await _context.ClinicianPatientAssignments
                .Include(a => a.Patient)
                .Where(a => a.ClinicianId == clinicianId.Value)
                .ToListAsync();

            var todayList = await _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.ClinicianId == clinicianId.Value &&
                            a.AppointmentTime.Date == today)
                .OrderBy(a => a.AppointmentTime)
                .ToListAsync();

            var upcomingList = await _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.ClinicianId == clinicianId.Value &&
                            a.AppointmentTime.Date > today)
                .OrderBy(a => a.AppointmentTime)
                .ToListAsync();

            var vm = new ClinicianAppointmentsViewModel
            {
                ClinicianId = clinicianId.Value,
                TodayAppointments = todayList,
                UpcomingAppointments = upcomingList,
                AssignedPatients = assigned.Select(a => a.Patient).ToList()
            };

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAppointment(int patientId, DateTime appointmentTime, string? notes)
        {
            var clinicianId = GetCurrentClinicianId();
            if (clinicianId == null) return View("NoClinicianLinked");

            var appointment = new Appointment
            {
                PatientId = patientId,
                ClinicianId = clinicianId.Value,
                AppointmentTime = appointmentTime,
                Notes = notes ?? "",
                Status = "Scheduled"
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            // Also create notification for patient
            var alert = new Alert
            {
                PatientId = patientId,
                AlertType = "Appointment Scheduled",
                AlertDescription = $"New appointment on {appointmentTime.ToLocalTime():MMM dd, HH:mm}",
                Severity = "info",
                CreatedAt = DateTime.UtcNow
            };

            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync();

            return RedirectToAction("Appointments");
        }



        // ---------------------------------------------
        // SETTINGS
        // ---------------------------------------------
        public IActionResult Settings()
        {
            return View();
        }

        // ---------------------------------------------
        // HEATMAP PAGE
        // ---------------------------------------------
        public async Task<IActionResult> Heatmap(int patientId)
        {
            var path = GetPatientCsvPath(patientId);
            if (!System.IO.File.Exists(path))
                return View("NoHeatmapData");

            var vm = new ClinicianHeatmapViewModel
            {
                PatientId = patientId,
                PatientName = "Patient",
                CsvUrl = Url.Content($"~/Data/Patient{patientId}.csv"),
                TotalFrames = System.IO.File.ReadAllLines(path).Length / 32
            };

            return View(vm);
        }
    }
}
