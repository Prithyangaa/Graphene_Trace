<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrapheneTrace.Model
{
    [Table("auth_metadata")]
    public class AuthMetadata
    {
        [Key]
        [Column("meta_id")]
        public int MetaId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("last_login")]
        public DateTime? LastLogin { get; set; }

        [Column("failed_attempts")]
        public int FailedAttempts { get; set; }

        [Column("password_updated_at")]
        public DateTime? PasswordUpdatedAt { get; set; }
    }
}

=======
namespace GrapheneTrace.Model
{
    public class AuthMetadata
    {
        public int MetaId { get; set; }
        public int UserId { get; set; }
        public DateTime? LastLogin { get; set; }
        public int FailedAttempts { get; set; }
        public DateTime? PasswordUpdatedAt { get; set; }
    }
}
>>>>>>> b283525eb90ca6690cffbf8e55180a5495858727
