using Microsoft.AspNetCore.Mvc;
using GrapheneTrace.Data;
using GrapheneTrace.Models;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;

namespace GrapheneTrace.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
            if (user == null || !VerifyPassword(model.Password, user.PasswordHash))
            {
                ViewBag.Error = "Invalid credentials";
                return View(model);
            }

            // Redirect based on role
            return user.Role switch
            {
                "Admin" => RedirectToAction("AdminDashboard", "Home"),
                "Clinician" => RedirectToAction("ClinicianDashboard", "Home"),
                "Patient" => RedirectToAction("PatientDashboard", "Home"),
                _ => RedirectToAction("Index", "Home")
            };
        }

        // GET: Register
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var existingUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);
            if (existingUser != null)
            {
                ViewBag.Error = "Email already exists";
                return View(model);
            }

            var hashed = HashPassword(model.Password);
            var user = new User { Email = model.Email, PasswordHash = hashed, Role = model.Role };
            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        // Password hashing helpers
        private string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return $"{Convert.ToBase64String(salt)}.{hashed}";
        }

        private bool VerifyPassword(string password, string stored)
        {
            var parts = stored.Split('.');
            var salt = Convert.FromBase64String(parts[0]);
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hash == parts[1];
        }
    }
}
