using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TaskTracker.DataAccess.Repositories;
using TaskTracker.Domain.Interfaces;
using TaskTracker.Domain.Services;

namespace TaskTracker.UI
{
    public static class MicrosoftDIContainerSetup
    {
        public static ServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("di-config.json", optional: false)
                .Build();
            services.AddSingleton<IConfiguration>(configuration);

            var singletons = configuration.GetSection("Singleton").Get<string[]>();
            var scoped = configuration.GetSection("Scoped").Get<string[]>();
            var transients = configuration.GetSection("Transient").Get<string[]>();

            foreach (var service in singletons)
            {
                if (service == "LoggerService")
                    services.AddSingleton<ILogger, ConsoleLogger>();
                if (service == "DateTimeProvider")
                    services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
            }

            foreach (var service in transients)
            {
                if (service == "NotificationService")
                {
                    services.AddTransient<INotificationService>(provider =>
                    {
                        var logger = provider.GetRequiredService<ILogger>();
                        var notificationService = new NotificationService(logger);
                        notificationService.EmailSender = new FakeEmailSender();
                        notificationService.SetDateTimeProvider(provider.GetRequiredService<IDateTimeProvider>());
                        return notificationService;
                    });
                }
            }

            return services.BuildServiceProvider();
        }
    }
}
