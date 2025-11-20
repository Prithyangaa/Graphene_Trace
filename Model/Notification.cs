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
