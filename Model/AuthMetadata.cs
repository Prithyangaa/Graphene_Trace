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

