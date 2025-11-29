using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrapheneTrace.Model
{
    [Table("devices")]
    public class Device
    {
        [Key]
        [Column("device_id")]
        public int DeviceId { get; set; }

        [Column("patient_id")]
        public int PatientId { get; set; }

        [Column("status")]
        public string Status { get; set; } = "Active";

        [Column("last_seen")]
        public DateTime LastSeen { get; set; }
    }
}

