using System;
using System.Linq;
using GrapheneTrace.Data;
using GrapheneTrace.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrapheneTrace.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET: /Auth/Login
        // =========================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // =========================
        // POST: /Auth/Login
        // =========================
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Email and password are required.");
                return View();
            }

            var user = _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View();
            }

            // =========================
            // SIMPLE PASSWORD CHECK (PLAINTEXT)
            // =========================
            if (!string.Equals(user.PasswordHash.Trim(), password.Trim(), StringComparison.Ordinal))
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View();
            }

            // =========================
            // STORE SESSION (BASE USER INFO)
            // =========================
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserFullName", user.FullName);
            HttpContext.Session.SetInt32("RoleId", user.RoleId);

            // Ensure clean session
            HttpContext.Session.Remove("ClinicianId");
            HttpContext.Session.Remove("PatientId");

            // =========================
            // LINK PATIENT ACCOUNT
            // (KEEP YOUR LOGIC EXACTLY AS IT IS)
            // =========================
            int? patientIdFromEmail = ResolvePatientIdFromEmail(user.Email);
            if (patientIdFromEmail.HasValue)
                HttpContext.Session.SetInt32("PatientId", patientIdFromEmail.Value);

            // Or if patient is linked via DB foreign key
            var patientRecord = _context.Patients.FirstOrDefault(p => p.UserId == user.UserId);
            if (patientRecord != null)
                HttpContext.Session.SetInt32("PatientId", patientRecord.PatientId);

            // =========================
            // LINK CLINICIAN ACCOUNT
            // =========================
            if (user.RoleId == 2) // Clinician role
            {
                var clinician = _context.Clinicians.FirstOrDefault(c => c.UserId == user.UserId);

                if (clinician != null)
                {
                    HttpContext.Session.SetInt32("ClinicianId", clinician.ClinicianId);
                }
                else
                {
                    // This prevents errors when clinician user exists but clinician table does not match
                    ModelState.AddModelError("", "Clinician profile not found.");
                    return View();
                }
            }

            // =========================
            // ROLE-BASED REDIRECT
            // =========================
            if (user.RoleId == 1)
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            if (user.RoleId == 2)
            {
                return RedirectToAction("Dashboard", "Clinician");
            }

            // Default → Patient Dashboard
            return RedirectToAction("Dashboard", "Patient");
        }

        // =========================
        // GET: /Auth/Logout
        // =========================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // =========================
        // Helper: Extract numeric ID from patient email
        // patient5@gmail.com → 5
        // =========================
        private int? ResolvePatientIdFromEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var atIndex = email.IndexOf('@');
            if (atIndex <= 0)
                return null;

            var local = email.Substring(0, atIndex); // "patient5"

            if (!local.StartsWith("patient", StringComparison.OrdinalIgnoreCase))
                return null;

            var digits = new string(local
                .SkipWhile(c => !char.IsDigit(c))
                .TakeWhile(char.IsDigit)
                .ToArray());

            if (int.TryParse(digits, out int id))
                return id;

            return null;
        }
    }
}
