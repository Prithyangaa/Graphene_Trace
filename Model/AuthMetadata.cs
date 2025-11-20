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
