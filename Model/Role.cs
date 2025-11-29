<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrapheneTrace.Model
{
    [Table("roles")]
    public class Role
    {
        [Key]
        [Column("role_id")]
        public int RoleId { get; set; }

        [Column("role_name")]
        public string RoleName { get; set; } = string.Empty;
    }
}

=======
namespace GrapheneTrace.Model
{
    public class Role
    {
        public int RoleId { get; set; }
        public required string RoleName { get; set; }
    }
}
>>>>>>> b283525eb90ca6690cffbf8e55180a5495858727
