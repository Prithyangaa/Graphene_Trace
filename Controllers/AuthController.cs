using GrapheneTrace.Model;
using GrapheneTrace.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace GrapheneTrace.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ------------------------------------
        // GET: Login Page
        // ------------------------------------
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // ------------------------------------
        // POST: Login Form Submission
        // ------------------------------------
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Email and password are required.";
                return View();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                ViewBag.Error = "Invalid email or password.";
                return View();
            }

            if (!VerifyPassword(password, user.PasswordHash))
            {
                ViewBag.Error = "Invalid email or password.";
                return View();
            }

            // Store user in session
            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("UserRole", user.Role.RoleName);

            // Redirect based on role
            return user.Role.RoleName switch
            {
                "Admin" => RedirectToAction("Dashboard", "Admin"),
                "Clinician" => RedirectToAction("Dashboard", "Clinician"),
                "Patient" => RedirectToAction("Dashboard", "Patient"),
                _ => RedirectToAction("Login")
            };
        }


        // ------------------------------------
        // GET: Registration Page
        // ------------------------------------
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        // ------------------------------------
        // POST: Registration Handling
        // ------------------------------------
        [HttpPost]
        public async Task<IActionResult> Register(
            string fullName,
            string email,
            string password,
            string role,
            string? speciality,
            int? age,
            string? gender,
            string? contactNumber
        )
        {
            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "All required fields must be filled.";
                return View();
            }

            if (await _context.Users.AnyAsync(u => u.Email == email))
            {
                ViewBag.Error = "Email already exists.";
                return View();
            }

            // Fetch role from DB
            var selectedRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == role);

            if (selectedRole == null)
            {
                ViewBag.Error = "Invalid role selection.";
                return View();
            }

            // Create base user
            var user = new User
            {
                FullName = fullName,
                Email = email,
                PasswordHash = HashPassword(password),
                RoleId = selectedRole.RoleId,
                Role = selectedRole,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Create extended entities depending on selected role
            if (role == "Patient")
            {
                var patient = new Patient
                {
                    FullName = fullName,
                    Age = age,
                    Gender = gender,
                    ContactNumber = contactNumber,
                    RiskLevel = "Low",
                    CurrentStatus = "Active",
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Patients.AddAsync(patient);
            }
            else if (role == "Clinician")
            {
                var clinician = new Clinician
                {
                    UserId = user.UserId,
                    Speciality = speciality,
                    IsActive = true
                };

                await _context.Clinicians.AddAsync(clinician);
            }

            await _context.SaveChangesAsync();

            ViewBag.Success = "Account created successfully! Please login.";
            return RedirectToAction("Login");
        }



        // ------------------------------------
        // LOGOUT
        // ------------------------------------
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // ------------------------------------------------
        // Password Hashing Utilities (SHA256)
        // ------------------------------------------------
        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            return HashPassword(password) == storedHash;
        }
    }
}