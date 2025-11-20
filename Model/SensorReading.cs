namespace GrapheneTrace.Model
{
    public class SensorReading
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public double Temperature { get; set; }
        public double HeartRate { get; set; }
        public double OxygenLevel { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
