<<<<<<< HEAD
using System;
using System.ComponentModel.DataAnnotations;

=======
>>>>>>> b283525eb90ca6690cffbf8e55180a5495858727
namespace GrapheneTrace.Model
{
    public class PatientRiskHistory
    {
<<<<<<< HEAD
        [Key]
        public int HistoryId { get; set; }

        public int PatientId { get; set; }

        public string OldRisk { get; set; } = string.Empty;
        public string NewRisk { get; set; } = string.Empty;

        public DateTime ChangedAt { get; set; }
    }
}

=======
        public int HistoryId { get; set; }
        public int PatientId { get; set; }
        public required string OldRisk { get; set; }
        public required string NewRisk { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
>>>>>>> b283525eb90ca6690cffbf8e55180a5495858727
