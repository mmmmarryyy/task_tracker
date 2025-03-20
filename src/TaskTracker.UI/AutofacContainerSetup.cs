using System.Reflection;
using Autofac;

namespace TaskTracker.UI
{
    public static class AutofacContainerSetup
    {
        public static Autofac.IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            var dataAccessAssembly = Assembly.Load("TaskTracker.DataAccess");

            var idatabaseType = dataAccessAssembly.GetType("TaskTracker.DataAccess.Storage.InMemoryDatabase");
            builder.RegisterType(idatabaseType)
                   .AsImplementedInterfaces()
                   .SingleInstance();

            builder.RegisterAssemblyTypes(dataAccessAssembly)
                   .Where(t => t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly.Load("TaskTracker.Domain"))
                   .Where(t => t.Name.EndsWith("Service") && t.Name != "NotificationService")
                   .AsImplementedInterfaces()
                   .InstancePerDependency();

            builder.RegisterType<AppRunner>().AsSelf().InstancePerDependency();

            builder.RegisterType<ProductionEnvironment>().As<IEnvironment>().SingleInstance();

            builder.RegisterInstance("Configuration from Autofac config").As<string>();

            return builder.Build();
        }
    }
}
