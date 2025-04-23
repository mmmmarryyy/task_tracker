using Autofac;
using System.Windows;
using System.Windows.Threading;
using TaskTracker.UI.Stores;
using TaskTracker.UI.ViewModels;
using TaskTracker.UI.Services;

namespace TaskTracker.UI
{
    public partial class App : Application
    {
        private IContainer _container;

        public App()
        {
            this.DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString(), "Unhandled Exception");
            e.Handled = true;
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            try
            {
                _container = AutofacContainerSetup.BuildContainer();
                MessageBox.Show("Container built OK");

                var nav = _container.Resolve<NavigationStore>();
                var loginVm = _container.Resolve<LoginViewModel>();
                nav.Initialize(loginVm);

                var mainVm = _container.Resolve<MainViewModel>();
                MessageBox.Show("Resolved MainViewModel OK");

                var mainWindow = new MainWindow
                {
                    DataContext = mainVm
                };
                this.MainWindow = mainWindow;
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error creating MainWindow");
            }
        }
    }
}


