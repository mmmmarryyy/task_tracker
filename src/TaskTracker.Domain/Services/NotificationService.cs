using TaskTracker.Domain.Interfaces;

namespace TaskTracker.Domain.Services
{
    public class NotificationService : INotificationService
    {
        private IDateTimeProvider _dateTimeProvider;

        private readonly ILogger _logger;
        public NotificationService(ILogger logger)
        {
            _logger = logger;
        }

        public IEmailSender EmailSender { get; set; }

        public void SetDateTimeProvider(IDateTimeProvider provider)
        {
            _dateTimeProvider = provider;
        }

        public void Notify(string message)
        {
            var timestamp = _dateTimeProvider?.Now.ToString() ?? DateTime.Now.ToString();
            _logger.Log($"Notification at {timestamp}: {message}");
            EmailSender?.SendEmail("user@example.com", "Notification", message);
        }
    }
}
