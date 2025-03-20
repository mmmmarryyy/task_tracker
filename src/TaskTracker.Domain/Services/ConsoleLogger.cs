using TaskTracker.Domain.Interfaces;

namespace TaskTracker.Domain.Services
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine("[Log] " + message);
        }
    }
}
