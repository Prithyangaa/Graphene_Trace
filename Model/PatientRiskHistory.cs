<<<<<<< HEAD
using System;
using System.ComponentModel.DataAnnotations;

=======
>>>>>>> e90a0f6ed46d6e329231b29efb5338278c92ab88
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
>>>>>>> e90a0f6ed46d6e329231b29efb5338278c92ab88
