<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrapheneTrace.Model
{
    [Table("notifications")]
    public class Notification
    {
        [Key]
        [Column("notification_id")]
        public int NotificationId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("alert_id")]
        public int AlertId { get; set; }

        [Column("message")]
        public string? Message { get; set; }

        [Column("delivered")]
        public bool Delivered { get; set; }

        [Column("delivered_at")]
        public DateTime? DeliveredAt { get; set; }
    }
}

=======
namespace GrapheneTrace.Model
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public int AlertId { get; set; }
        public string? Message { get; set; }
        public bool Delivered { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }
}
>>>>>>> b283525eb90ca6690cffbf8e55180a5495858727
