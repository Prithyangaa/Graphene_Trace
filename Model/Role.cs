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
>>>>>>> e90a0f6ed46d6e329231b29efb5338278c92ab88
