using GrapheneTrace.Data;
using GrapheneTrace.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
=======
>>>>>>> b283525eb90ca6690cffbf8e55180a5495858727

namespace GrapheneTrace.Controllers
{
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;
<<<<<<< HEAD
        private readonly IWebHostEnvironment _env;

        public PatientController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // Helper: get current patient id from session
        private int? GetCurrentPatientId()
        {
            return HttpContext.Session.GetInt32("PatientId");
        }

        private string GetPatientCsvPath(int patientId)
        {
            // e.g. wwwroot/Data/Patient1.csv
            return Path.Combine(_env.WebRootPath, "Data", $"Patient{patientId}.csv");
        }

        private bool TryComputeCsvMetrics(
            int patientId,
            out double peak,
            out double contact,
            out double avg,
            out double variability,
            out double stability,
            out double movement,
            out int hotspots,
            out string heatmapImageBase64)
        {
            peak = contact = avg = variability = stability = movement = 0;
            hotspots = 0;
            heatmapImageBase64 = "";

            var path = GetPatientCsvPath(patientId);
            if (!System.IO.File.Exists(path))
                return false;

            var lines = System.IO.File.ReadAllLines(path);
            if (lines.Length < 32) return false;

            int size = 32;
            List<double[,]> frames = new();

            for (int i = 0; i + size <= lines.Length; i += size)
            {
                double[,] f = new double[size, size];
                for (int r = 0; r < size; r++)
                {
                    var cols = lines[i + r].Split(',').Select(double.Parse).ToArray();
                    for (int c = 0; c < cols.Length && c < size; c++)
                        f[r, c] = cols[c];
                }
                frames.Add(f);
            }

            if (frames.Count == 0) return false;

            var frame = frames.Last();

            List<double> values = new();
            foreach (var v in frame) values.Add(v);

            peak = values.Max();
            avg = values.Average();
            contact = values.Count(v => v > 30) * 100.0 / values.Count;

            // Variability (std dev)
            double mean = avg;
            variability = Math.Sqrt(values.Average(v => Math.Pow(v - mean, 2)));

            // Stability
            double minValue = values.Min();
            stability = (peak - minValue) / (avg == 0 ? 1 : avg);

            // Hotspots (> 80)
            hotspots = values.Count(v => v > 80);

            // Movement index (diff between last two frames)
            if (frames.Count > 1)
            {
                var prev = frames[^2];
                double totalDiff = 0;
                for (int r = 0; r < size; r++)
                    for (int c = 0; c < size; c++)
                        totalDiff += Math.Abs(frame[r, c] - prev[r, c]);

                movement = totalDiff / (size * size);
            }

            return true;
        }

        private (string priority, string label, string summary)
            ComputePriority(double peak, double contact, int hotspots, string? riskLevel, string? currentStatus)
        {
            string risk = riskLevel?.ToLower() ?? "";
            string status = currentStatus?.ToLower() ?? "";

            bool critical =
                risk.Contains("critical") ||
                status.Contains("deteriorating") ||
                peak >= 180 ||
                hotspots >= 10;

            bool high =
                risk.Contains("high") ||
                peak >= 145;

            bool medium =
                risk.Contains("medium") ||
                peak >= 120;

            if (critical)
                return ("Critical", "Critical", "Critical pressure detected");

            if (high)
                return ("High", "Attention", "High pressure — needs clinician attention");

            if (medium)
                return ("Medium", "Moderate", "Moderate pressure — review recommended");

            return ("Stable", "Stable", "Normal readings");
        }


        // ============================
        // MAIN DASHBOARD
        // ============================
        public async Task<IActionResult> Dashboard()
        {
            var patientId = GetCurrentPatientId();
            if (patientId == null)
                return View("NoPatientLinked");

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PatientId == patientId.Value);
            if (patient == null)
                return View("NoPatientLinked");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == patient.UserId);
            var assignment = await _context.ClinicianPatientAssignments
                .Include(a => a.Clinician)
                .FirstOrDefaultAsync(a => a.PatientId == patient.PatientId && a.IsPrimary);

            string? clinicianName = null;
            string? clinicianSpec = null;

            if (assignment != null)
            {
                var clinicianUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserId == assignment.Clinician.UserId);

                clinicianName = clinicianUser?.FullName;
                clinicianSpec = assignment.Clinician.Speciality;
            }

            var alerts = await _context.Alerts
                .Where(a => a.PatientId == patient.PatientId)
                .OrderByDescending(a => a.CreatedAt)
                .Take(3)
                .ToListAsync();

            var messages = await _context.PatientMessages
                .Where(m => m.PatientId == patient.PatientId)
                .OrderByDescending(m => m.CreatedAt)
                .Take(3)
                .ToListAsync();

            double peak, contact, avg, variability, stability, movement;
            int hotspots;
            string heatmapImg;

            bool hasHeatmap = TryComputeCsvMetrics(
                patient.PatientId,
                out peak,
                out contact,
                out avg,
                out variability,
                out stability,
                out movement,
                out hotspots,
                out heatmapImg
            );
            var (priority, priorityLabel, prioritySummary) =
                ComputePriority(
                    peak,
                    contact,
                    hotspots,
                    patient.RiskLevel,
                    patient.CurrentStatus
                );
            var vm = new PatientDashboardViewModel
            {
                PatientId = patient.PatientId,
                FullName = patient.FullName ?? "",
                Age = patient.Age,
                Gender = patient.Gender,
                ContactNumber = patient.ContactNumber,
                RiskLevel = patient.RiskLevel,
                CurrentStatus = patient.CurrentStatus,
                CreatedAt = patient.CreatedAt,
                UserId = user?.UserId ?? 0,
                Email = user?.Email ?? "",
                ClinicianName = clinicianName,
                ClinicianSpeciality = clinicianSpec,
                PeakPressureIndex = peak,
                ContactAreaPercent = contact,
                AveragePressure = avg,
                PressureStatus = priority,
                PressureStatusLabel = priorityLabel,
                PressureStatusSummary = prioritySummary,
                RecentAlerts = alerts,
                RecentMessages = messages,
                HasHeatmapData = hasHeatmap,
                HasPastHeatmapData = hasHeatmap,
                PressureVariability = variability,
                StabilityIndex = stability,
                MovementIndex = movement,
                HotspotCount = hotspots,
                HeatmapImageBase64 = heatmapImg,
            };

            return View(vm);
        }

        // ============================
        // LIVE HEATMAP (full page)
        // ============================
        public IActionResult LiveHeatmap()
        {
            var patientId = GetCurrentPatientId();
            if (patientId == null) return View("NoPatientLinked");

            var csvPath = GetPatientCsvPath(patientId.Value);
            if (!System.IO.File.Exists(csvPath))
                return View("NoPatientLinked");

            ViewBag.PatientId = patientId.Value;
            ViewBag.CsvUrl = Url.Content($"~/Data/Patient{patientId.Value}.csv");
            return View();
        }

        // ============================
        // PAST HEATMAP WITH SLIDER
        // ============================
        public IActionResult PastHeatmap()
        {
            var patientId = GetCurrentPatientId();
            if (patientId == null) return View("NoPatientLinked");

            var csvPath = GetPatientCsvPath(patientId.Value);
            if (!System.IO.File.Exists(csvPath))
                return View("NoPatientLinked");

            ViewBag.PatientId = patientId.Value;
            ViewBag.CsvUrl = Url.Content($"~/Data/Patient{patientId.Value}.csv");
            return View();
        }

        // ============================
        // ALERTS / NOTIFICATIONS
        // ============================
        public async Task<IActionResult> Alerts()
        {
            var patientId = GetCurrentPatientId();
            if (patientId == null) return View("NoPatientLinked");

            var alerts = await _context.Alerts
                .Where(a => a.PatientId == patientId.Value)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return View(alerts);
        }

        // ============================
        // CLINICIAN MESSAGES
        // ============================
        [HttpGet]
        public async Task<IActionResult> Messages()
        {
            var patientId = GetCurrentPatientId();
            if (patientId == null) return View("NoPatientLinked");

            var messages = await _context.PatientMessages
                .Where(m => m.PatientId == patientId.Value)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();

            ViewBag.PatientId = patientId.Value;
            return View(messages);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(string content)
        {
            var patientId = GetCurrentPatientId();
            if (patientId == null) return View("NoPatientLinked");

            if (string.IsNullOrWhiteSpace(content))
                return RedirectToAction(nameof(Messages));

            // primary clinician (if any)
            var assignment = await _context.ClinicianPatientAssignments
                .FirstOrDefaultAsync(a => a.PatientId == patientId.Value && a.IsPrimary);

            var msg = new PatientMessage
            {
                PatientId = patientId.Value,
                ClinicianId = assignment?.ClinicianId,
                FromClinician = false,
                Content = content.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _context.PatientMessages.Add(msg);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Messages));
        }

        // ============================
        // PROFILE
        // ============================
        public async Task<IActionResult> Profile()
        {
            var patientId = GetCurrentPatientId();
            if (patientId == null) return View("NoPatientLinked");

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PatientId == patientId.Value);
            if (patient == null) return View("NoPatientLinked");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == patient.UserId);
            var assignment = await _context.ClinicianPatientAssignments
                .Include(a => a.Clinician)
                .FirstOrDefaultAsync(a => a.PatientId == patient.PatientId && a.IsPrimary);

            string? clinicianName = null;
            string? clinicianSpec = null;

            if (assignment != null)
            {
                var clinicianUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserId == assignment.Clinician.UserId);

                clinicianName = clinicianUser?.FullName;
                clinicianSpec = assignment.Clinician.Speciality;
            }

            var totalAlerts = await _context.Alerts.CountAsync(a => a.PatientId == patient.PatientId);
            var daysMonitored = (DateTime.UtcNow.Date - patient.CreatedAt.Date).Days + 1;

            double peak, contact, avg, variability, stability, movement;
            int hotspots;
            string heatmapImg;
            TryComputeCsvMetrics(patient.PatientId,
                out peak,
                out contact,
                out avg,
                out variability,
                out stability,
                out movement,
                out hotspots,
                out heatmapImg
            );
            ViewBag.TotalAlerts = totalAlerts;
            ViewBag.DaysMonitored = daysMonitored;
            ViewBag.AvgDailyPressure = avg;
            ViewBag.Email = user?.Email ?? "";
            ViewBag.ClinicianName = clinicianName;
            ViewBag.ClinicianSpeciality = clinicianSpec;

            return View(patient);
        }

        // ============================
        // SETTINGS
        // ============================
        public IActionResult Settings()
        {
            return View();
=======

        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =======================
        // PATIENT DASHBOARD
        // =======================
        public IActionResult Index(int id)
        {
            var patient = _context.Patients.Find(id);

            return View(patient);
        }

        // =======================
        // ALERT VIEW
        // =======================
        public IActionResult Alerts(int patientId)
        {
            var alerts = _context.Alerts
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.CreatedAt)
                .ToList();

            return View(alerts);
        }

        // =======================
        // ULCER DATA
        // =======================
        public IActionResult UlcerData(int patientId)
        {
            var data = _context.SensorReadings
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.Timestamp)
                .ToList();

            return View(data);
        }

        // =======================
        // PROFILE
        // =======================
        public IActionResult Profile(int id)
        {
            var patient = _context.Patients.Find(id);
            return View(patient);
        }

        [HttpPost]
        public IActionResult UpdateProfile(int id, string contact, string status)
        {
            var patient = _context.Patients.Find(id);
            if (patient == null) return NotFound();

            patient.ContactNumber = contact;
            patient.CurrentStatus = status;

            _context.SaveChanges();

            return RedirectToAction("Profile", new { id });
>>>>>>> b283525eb90ca6690cffbf8e55180a5495858727
        }
    }
}
