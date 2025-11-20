using GrapheneTrace.Data;
using GrapheneTrace.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrapheneTrace.Controllers
{
    public class ClinicianController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClinicianController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =======================
        // DASHBOARD
        // =======================
        public IActionResult Index()
        {
            return View();
        }

        // =======================
        // VIEW ASSIGNED PATIENTS
        // =======================
        public IActionResult MyPatients(int clinicianId)
        {
            var patients = _context.ClinicianPatientAssignments
                .Include(a => a.Patient)
                .Where(a => a.ClinicianId == clinicianId)
                .ToList();

            return View(patients);
        }

        // =======================
        // PATIENT DETAILS
        // =======================
        public IActionResult PatientDetails(int id)
        {
            var patient = _context.Patients
                .FirstOrDefault(p => p.PatientId == id);

            if (patient == null)
                return NotFound();

            return View(patient);
        }

        // =======================
        // UPDATE PATIENT STATUS
        // =======================
        [HttpPost]
        public IActionResult UpdateStatus(int id, string newStatus)
        {
            var patient = _context.Patients.Find(id);
            if (patient == null) return NotFound();

            patient.CurrentStatus = newStatus;
            _context.SaveChanges();

            return RedirectToAction("PatientDetails", new { id });
        }
    }
}
