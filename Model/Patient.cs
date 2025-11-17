using System;
using System.Collections.Generic;

namespace GrapheneTrace.Model
{
    public class Patient
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<string> Notifications { get; set; } = new List<string>();
        public string CurrentHeartRateMap { get; set; } = "/images/current-map.png";
        public string PastHeartRateMap { get; set; } = "/images/past-map.png";
    }
}
