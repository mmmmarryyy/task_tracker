using Microsoft.Extensions.DependencyInjection;
using TaskTracker.Domain.Interfaces;
using TaskTracker.Domain.Services;
using TaskTracker.UI;
using Xunit;

namespace TaskTracker.Tests
{
    public class NotificationServiceTests
    {
        [Fact]
        public void MicrosoftDI_NotificationService_Notifies_Correctly()
        {
            var serviceProvider = MicrosoftDIContainerSetup.BuildServiceProvider();
            var notificationService = serviceProvider.GetService<INotificationService>();
            Xunit.Assert.NotNull(notificationService);

            notificationService.Notify("Unit test notification");
        }
    }
}
