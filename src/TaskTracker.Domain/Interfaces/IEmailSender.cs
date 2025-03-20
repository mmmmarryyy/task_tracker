namespace TaskTracker.Domain.Interfaces
{
    public interface IEmailSender
    {
        void SendEmail(string to, string subject, string body);
    }
}
