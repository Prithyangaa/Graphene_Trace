using Microsoft.AspNetCore.Mvc;
using GrapheneTrace.Data;
using GrapheneTrace.Models;
using System.Linq;
using BCrypt.Net;

namespace GrapheneTrace.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ---------------- REGISTER ----------------
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // check if user already exists
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "User already exists with that email.");
                    return View(model);
                }

                // Hash password
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

                // Create new user
                var user = new User
                {
                    Email = model.Email,
                    PasswordHash = hashedPassword,
                    Role = model.Role
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                TempData["Success"] = "Registration successful. Please login.";
                return RedirectToAction("Login");
            }
            return View(model);
        }

        // ---------------- LOGIN ----------------
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                    return View(model);
                }

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);
                if (!isPasswordValid)
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                    return View(model);
                }

                // Simulate Role-Based Access Redirect
                if (user.Role == "Admin")
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                else if (user.Role == "Clinician")
                    return RedirectToAction("Dashboard", "Clinician");
            }
            return View(model);
        }
    }
}

