using GrapheneTrace.Model;
using Microsoft.AspNetCore.Mvc;

namespace GrapheneTrace.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult Dashboard()
        {
            var patient = new Patient
            {
                FullName = "Jane Doe",
                Gender = "Female",
                DOB = new DateTime(1998, 4, 15),
                Email = "jane.doe@example.com",
                Phone = "+1 555-123-4567",
                Address = "123 Wellness Ave, Health City",
                Notifications = new List<string>
                {
                    "New message from Dr. Lee",
                    "Upcoming appointment reminder"
                },
                
            };
            return View(patient);
        }

        public IActionResult LiveHeatMap()
        {
            return View();
        }

        public IActionResult PastHeatMap()  
        {
            return View("PastHeatMap");
        }
    }
}
