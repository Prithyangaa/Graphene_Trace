using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrapheneTrace.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        // store hashed password in LONGTEXT to be safe for length
        [Column(TypeName = "longtext")]
        public string PasswordHash { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Role { get; set; } = string.Empty;
    }
}
