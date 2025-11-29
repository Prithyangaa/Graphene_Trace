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

