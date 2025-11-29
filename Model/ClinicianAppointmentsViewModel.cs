using System;
using System.Collections.Generic;

namespace GrapheneTrace.Model
{
    public class ClinicianAppointmentsViewModel
    {
        public int ClinicianId { get; set; }

        // List of appointments
        public List<Appointment> TodayAppointments { get; set; } = new();
        public List<Appointment> UpcomingAppointments { get; set; } = new();

        // For creation
        public List<Patient> AssignedPatients { get; set; } = new();

        public Appointment NewAppointment { get; set; } = new Appointment();
    }
}
