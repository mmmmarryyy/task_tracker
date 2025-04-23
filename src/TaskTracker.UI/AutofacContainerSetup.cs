using System.Reflection;
using Autofac;
using Microsoft.EntityFrameworkCore;
using TaskTracker.UI.Stores;
using TaskTracker.UI.ViewModels;
using TaskTracker.UI.Services;
using System.Windows;

namespace TaskTracker.UI
{
    public static class AutofacContainerSetup
    {
        public static Autofac.IContainer BuildContainer()
        {
            Console.WriteLine("Inside AutofacContainerSetup.BuildContainer()");
            var builder = new ContainerBuilder();

            var dataAccessAssembly = Assembly.Load("TaskTracker.DataAccess");

            var efContextType = dataAccessAssembly.GetType("TaskTracker.DataAccess.EFTaskTrackerContext");
            var efDatabaseType = dataAccessAssembly.GetType("TaskTracker.DataAccess.Storage.EFDatabase");
            var inMemoryDbType = dataAccessAssembly.GetType("TaskTracker.DataAccess.Storage.InMemoryDatabase");

            if (efContextType == null || efDatabaseType == null || inMemoryDbType == null)
            {
                throw new Exception("�� ������� ��������� ����������� ����.");
            }

            //string connectionString = "Data Source=WRS-KIY-005\\SQLEXPRESS;Initial Catalog=TaskTrackerDB;Integrated Security=True;Encrypt=False"; // TODO: move connection string from code to settings
            string connectionString = "";
            Type chosenDatabaseType;

            try
            {
                var optionsBuilderType = typeof(DbContextOptionsBuilder<>).MakeGenericType(efContextType);
                dynamic optionsBuilder = Activator.CreateInstance(optionsBuilderType);

                var sqlServerExtensionsType = typeof(Microsoft.EntityFrameworkCore.SqlServerDbContextOptionsExtensions);
                var useSqlServerMethod = sqlServerExtensionsType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .FirstOrDefault(m =>
                    {
                        if (m.Name != "UseSqlServer")
                        {
                            return false;
                        }
                        var parameters = m.GetParameters();
                        return parameters.Length == 3 &&
                               parameters[0].ParameterType.IsAssignableFrom(optionsBuilderType) &&
                               parameters[1].ParameterType == typeof(string);
                    });
                if (useSqlServerMethod == null)
                {
                    throw new Exception("�� ������� ����� ����� UseSqlServer � ���������� ����������.");
                }
                useSqlServerMethod.Invoke(null, new object[] { optionsBuilder, connectionString, null });

                dynamic efContextInstance = Activator.CreateInstance(efContextType, optionsBuilder.Options);
                builder.RegisterInstance((object)efContextInstance)
                   .AsSelf()
                   .ExternallyOwned();

                if (efContextInstance.Database.CanConnect())
                {
                    //MessageBox.Show("����������� � MS SQL Server �������. ����� ������������ EFDatabase.");
                    Console.WriteLine("����������� � MS SQL Server �������. ����� ������������ EFDatabase.");

                    chosenDatabaseType = efDatabaseType;
                }
                else
                {
                    //MessageBox.Show("���������� ������������ � MS SQL Server. ���������� InMemoryDatabase.");
                    Console.WriteLine("���������� ������������ � MS SQL Server. ���������� InMemoryDatabase.");
                    chosenDatabaseType = inMemoryDbType;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"������ ��� �������� �����������: {ex.Message}");
                Console.WriteLine($"������ ��� �������� �����������: {ex.Message}");
                //MessageBox.Show("���������� InMemoryDatabase.");
                Console.WriteLine("���������� InMemoryDatabase.");
                chosenDatabaseType = inMemoryDbType;
            }

            builder.RegisterType(chosenDatabaseType)
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

            builder.RegisterType<AuthenticationStore>().SingleInstance();
            builder.RegisterType<NavigationStore>().SingleInstance();

            builder.RegisterType<LoginViewModel>().InstancePerDependency();
            builder.RegisterType<TaskListViewModel>().InstancePerDependency();
            builder.RegisterType<TaskEditViewModel>().InstancePerDependency();
            builder.RegisterType<MainViewModel>().InstancePerDependency();
            builder.RegisterType<MainViewModel>().InstancePerDependency();
            builder.RegisterType<RelayCommand>();

            return builder.Build();
        }
    }
}

