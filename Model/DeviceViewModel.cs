namespace GrapheneTrace.Model
{
    public class DeviceViewModel
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; } = "";

        public DateTime LastUpdated { get; set; }

        public float Pressure { get; set; }
        public float Temperature { get; set; }

        public bool IsActive => (DateTime.UtcNow - LastUpdated).TotalMinutes < 5;

        public string LastValue =>
            $"P: {Pressure} , T: {Temperature}";
    }
}

