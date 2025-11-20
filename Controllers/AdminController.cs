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
                .ToList();

            return View(users);
        }

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
    }
}
