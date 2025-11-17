using Microsoft.AspNetCore.Mvc;

namespace YourProject.Controllers
{
    public class ClinicianController : Controller
    {
        // GET: /Clinician/Dashboard
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
