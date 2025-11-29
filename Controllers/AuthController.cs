<<<<<<< HEAD
using System;
using System.Linq;
using GrapheneTrace.Data;
using GrapheneTrace.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
=======
using GrapheneTrace.Model;
using GrapheneTrace.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
>>>>>>> e90a0f6ed46d6e329231b29efb5338278c92ab88

namespace GrapheneTrace.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

<<<<<<< HEAD
        // =========================
        // GET: /Auth/Login
        // =========================
=======
        // ------------------------------------
        // GET: Login Page
        // ------------------------------------
>>>>>>> e90a0f6ed46d6e329231b29efb5338278c92ab88
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

<<<<<<< HEAD
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
=======
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
>>>>>>> e90a0f6ed46d6e329231b29efb5338278c92ab88
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

<<<<<<< HEAD
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
=======
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
>>>>>>> e90a0f6ed46d6e329231b29efb5338278c92ab88
