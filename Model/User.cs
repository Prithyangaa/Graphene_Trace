<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

=======
>>>>>>> e90a0f6ed46d6e329231b29efb5338278c92ab88
namespace GrapheneTrace.Model
{
    [Table("users")]
    public class User
    {
<<<<<<< HEAD
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("full_name")]
        public string FullName { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("password_hash")]
        public string PasswordHash { get; set; } = string.Empty;

        [Column("role_id")]
        public int RoleId { get; set; }

        public Role Role { get; set; } = null!;

        [Column("created_at")]
=======
        public int UserId { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

>>>>>>> e90a0f6ed46d6e329231b29efb5338278c92ab88
        public DateTime CreatedAt { get; set; }
    }
}

