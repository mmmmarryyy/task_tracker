using Autofac;
using TaskTracker.Domain.Interfaces;
using TaskTracker.UI;
using Xunit;

namespace TaskTracker.Tests
{
    public class AutofacContainerTests
    {
        [Fact]
        public void Autofac_Resolves_AppRunner_With_All_Dependencies_And_Runs()
        {
            var container = AutofacContainerSetup.BuildContainer();
            using (var scope = container.BeginLifetimeScope())
            {
                var appRunner = scope.Resolve<AppRunner>();
                Xunit.Assert.NotNull(appRunner);

                appRunner.ConfigurationValue = scope.Resolve<string>();
                Xunit.Assert.False(string.IsNullOrEmpty(appRunner.ConfigurationValue));

                appRunner.SetEnvironment(scope.Resolve<IEnvironment>());

                var originalOut = Console.Out;
                using (var sw = new StringWriter())
                {
                    Console.SetOut(sw);

                    appRunner.Run();

                    var output = sw.ToString();
                    Xunit.Assert.Contains("AppRunner running in environment", output);
                    Xunit.Assert.Contains("Configuration Value:", output);
                    Console.SetOut(originalOut);
                }
            }
        }

        [Fact]
        public void Autofac_Resolves_Lifetime_Correctly()
        {
            var container = AutofacContainerSetup.BuildContainer();
            using var scope = container.BeginLifetimeScope();

            var repo1 = scope.Resolve<ITaskRepository>();
            var repo2 = scope.Resolve<ITaskRepository>();
            Xunit.Assert.Same(repo1, repo2);
        }
    }
}
