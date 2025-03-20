namespace TaskTracker.Domain.Interfaces
{
    public interface INotificationService
    {
        void Notify(string message);
        void SetDateTimeProvider(IDateTimeProvider provider);
    }
}
