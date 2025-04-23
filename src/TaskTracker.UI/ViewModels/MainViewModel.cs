using System;
using System.Windows.Input;
using TaskTracker.UI.Stores;
using TaskTracker.UI.Services;
using TaskTracker.Domain.Interfaces;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;
using System.Collections.ObjectModel;
using System.Windows;
using System.Threading.Tasks;

namespace TaskTracker.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore _nav;
        private readonly Func<LoginViewModel> _loginFactory;
        private readonly Func<TaskListViewModel> _tasksFactory;
        private readonly AuthenticationStore _authStore;
        public ViewModelBase CurrentViewModel => _nav.CurrentViewModel;

        public ICommand GoBackCommand { get; }
        public ICommand GoForwardCommand { get; }
        public ICommand NavigateLoginCommand { get; }
        public ICommand NavigateTasksCommand { get; }

        public bool CanGoBack => _nav.CanGoBack;
        public bool CanGoForward => _nav.CanGoForward;

        public MainViewModel(
            NavigationStore nav,
            Func<LoginViewModel> loginFactory,
            Func<TaskListViewModel> tasksFactory,
            AuthenticationStore authStore)
        {
            _nav = nav;
            _loginFactory = loginFactory;
            _tasksFactory = tasksFactory;
            _authStore = authStore;

            GoBackCommand = new RelayCommand(_ => _nav.GoBack(), _ => _nav.CanGoBack);
            GoForwardCommand = new RelayCommand(_ => _nav.GoForward(), _ => _nav.CanGoForward);

            _nav.StateChanged += () =>
            {
                OnPropertyChanged(nameof(CurrentViewModel));
                OnPropertyChanged(nameof(CanGoBack));
                OnPropertyChanged(nameof(CanGoForward));

                (GoBackCommand as RelayCommand)!.RaiseCanExecuteChanged();
                (GoForwardCommand as RelayCommand)!.RaiseCanExecuteChanged();
            };

            NavigateLoginCommand = new RelayCommand(_ => _nav.Navigate(_loginFactory()));
            NavigateTasksCommand = new RelayCommand(_ =>
            {
                if (_authStore.IsLoggedIn)
                {
                    _nav.Navigate(_tasksFactory());
                }
                else
                {
                    MessageBox.Show(
                        "Для доступа к функционалу пройдите авторизацию",
                        "Внимание",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            });
        }
    }
}