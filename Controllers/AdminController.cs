using Microsoft.AspNetCore.Mvc;

namespace GrapheneTrace.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            var system = new
            {
                Health = "Good",
                Uptime = "24 hrs",
                ActiveDevices = 128
            };

            var stats = new
            {
                TotalUsers = 142,
                Patients = 87,
                Clinicians = 48,
                Admins = 7
            };

            ViewBag.SystemStatus = system;
            ViewBag.UserStats = stats;

            return View();
        }
    }
}
