using System;
using System.ComponentModel.DataAnnotations;

namespace GrapheneTrace.Model
{
    public class PatientRiskHistory
    {
        [Key]
        public int HistoryId { get; set; }

        public int PatientId { get; set; }

        public string OldRisk { get; set; } = string.Empty;
        public string NewRisk { get; set; } = string.Empty;

        public DateTime ChangedAt { get; set; }
    }
}

