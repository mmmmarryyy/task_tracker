namespace TaskTracker.Domain.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}
