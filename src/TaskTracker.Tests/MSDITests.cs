using Xunit;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using TaskTracker.Domain.Interfaces;
using TaskTracker.Domain.Services;
using TaskTracker.UI;
using Microsoft.Extensions.Configuration;


namespace TaskTracker.Tests
{
    public class MSDITests
    {
        [Fact]
        public void MsDi_Resolves_Lifetimes_From_Config()
        {
            var serviceProvider = MicrosoftDIContainerSetup.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var logger1 = scope.ServiceProvider.GetService<ILogger>();
            var logger2 = scope.ServiceProvider.GetService<ILogger>();
            Xunit.Assert.Same(logger1, logger2);

            var repo1 = scope.ServiceProvider.GetService<ITaskRepository>();
            var repo2 = scope.ServiceProvider.GetService<ITaskRepository>();
            Xunit.Assert.Same(repo1, repo2);

            var notify1 = scope.ServiceProvider.GetService<INotificationService>();
            var notify2 = scope.ServiceProvider.GetService<INotificationService>();
            Xunit.Assert.NotSame(notify1, notify2);
        }

        [Fact]
        public void MsDi_Config_Parses_Correctly()
        {
            var serviceProvider = MicrosoftDIContainerSetup.BuildServiceProvider();
            var config = serviceProvider.GetService<IConfiguration>();
            Xunit.Assert.NotNull(config);
            Xunit.Assert.Equal("LoggerService", config.GetSection("Singleton").Get<string[]>()[0]);
        }
    }
}
