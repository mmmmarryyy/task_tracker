using TaskTracker.Domain.Interfaces;

namespace TaskTracker.UI
{
    public interface IEnvironment
    {
        string Name { get; }
    }

    public class ProductionEnvironment : IEnvironment
    {
        public string Name => "Production";
    }

    public class AppRunner
    {
        private readonly IAuthService _authService;
        private readonly ITaskService _taskService;
        private IEnvironment _environment;

        public AppRunner(IAuthService authService, ITaskService taskService)
        {
            _authService = authService;
            _taskService = taskService;
        }

        public string ConfigurationValue { get; set; }

        public void SetEnvironment(IEnvironment env)
        {
            _environment = env;
        }

        public void Run()
        {
            Console.WriteLine("AppRunner running in environment: " + _environment?.Name);
            Console.WriteLine("Configuration Value: " + ConfigurationValue);
        }
    }
}
