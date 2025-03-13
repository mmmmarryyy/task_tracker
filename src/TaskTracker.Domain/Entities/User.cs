namespace TaskTracker.Domain.Entities
{
    public class User
    {
        public required int Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
    }
}