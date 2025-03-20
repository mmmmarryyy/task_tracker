using TaskTracker.Domain.Interfaces;

namespace TaskTracker.Domain.Services
{
    public class SystemDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}
