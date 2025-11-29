<<<<<<< HEAD
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace GrapheneTrace.Model
{
    [Table("sensor_readings")]
    public class SensorReading
    {
        [Key]
        [Column("reading_id")]
        public int ReadingId { get; set; }

        [Column("patient_id")]
        public int PatientId { get; set; }

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        [Column("value")]
        public string? Value { get; set; }

        // Extract pressure from JSON
        [NotMapped]
        public float Pressure
        {
            get
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(Value)) return 0;
                    using var doc = JsonDocument.Parse(Value);
                    return doc.RootElement.GetProperty("pressure").GetSingle();
                }
                catch { return 0; }
            }
        }

        // Extract temperature from JSON
        [NotMapped]
        public float Temperature
        {
            get
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(Value)) return 0;
                    using var doc = JsonDocument.Parse(Value);
                    return doc.RootElement.GetProperty("temperature").GetSingle();
                }
                catch { return 0; }
            }
        }
    }
}

=======
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
>>>>>>> e90a0f6ed46d6e329231b29efb5338278c92ab88
