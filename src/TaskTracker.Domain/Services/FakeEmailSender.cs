using TaskTracker.Domain.Interfaces;

namespace TaskTracker.Domain.Services
{
    public class FakeEmailSender : IEmailSender
    {
        public void SendEmail(string to, string subject, string body)
        {
            Console.WriteLine($"[FakeEmailSender] Email sent to {to} with subject '{subject}'");
        }
    }
}
