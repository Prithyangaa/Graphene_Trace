<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrapheneTrace.Model
{
    [Table("clinicians")]
    public class Clinician
    {
        [Key]
        [Column("clinician_id")]
        public int ClinicianId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Column("speciality")]
        public string? Speciality { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("phone")]
        public string? Phone { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("department")]
        public string? Department { get; set; }

        [Column("bio")]
        public string? Bio { get; set; }

    }
}

=======
namespace GrapheneTrace.Model
{
    public class Clinician
    {
        public int ClinicianId { get; set; }
        public int UserId { get; set; }
        public string? Speciality { get; set; }
        public bool IsActive { get; set; }
    }
}
>>>>>>> b283525eb90ca6690cffbf8e55180a5495858727
