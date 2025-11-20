using GrapheneTrace.Data;
using GrapheneTrace.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrapheneTrace.Controllers
{
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;

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
        }
    }
}
