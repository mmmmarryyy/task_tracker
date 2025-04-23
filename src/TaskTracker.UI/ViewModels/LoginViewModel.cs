using System;
using System.Windows.Input;
using TaskTracker.UI.Stores;
using TaskTracker.UI.Services;
using TaskTracker.Domain.Interfaces;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;
using System.Collections.ObjectModel;
using System.Windows;


namespace TaskTracker.UI.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthService _authService;
        private readonly NavigationStore _navigation;
        private readonly Func<TaskListViewModel> _taskListVmFactory;
        private readonly AuthenticationStore _authStore;

        public bool IsLoggedIn => _authStore.IsLoggedIn;

        public ICommand RegisterCommand { get; }
        public ICommand LoginCommand { get; }
        public ICommand LogoutCommand { get; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; private set; }

        public LoginViewModel(IAuthService authService,
                              NavigationStore navigation,
                              Func<TaskListViewModel> taskListVmFactory,
                              AuthenticationStore authStore)
        {
            _authService = authService;
            _navigation = navigation;
            _taskListVmFactory = taskListVmFactory;
            _authStore = authStore;

            if (IsLoggedIn)
            {
                Username = _authStore.CurrentUser.Username;
            }

            RegisterCommand = new RelayCommand(_ => DoRegister());
            LoginCommand = new RelayCommand(_ => DoLogin());
            LogoutCommand = new RelayCommand(_ => Logout());
        }

        private void DoRegister()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Username) || Username.Length > 20 ||
                    string.IsNullOrWhiteSpace(Password) || Password.Length > 20)
                {
                    ErrorMessage = "»м€ и пароль: 1Ц20 символов.";
                    OnPropertyChanged(nameof(ErrorMessage));
                    return;
                }
                _authService.Register(Username, Password);
                DoLogin();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        private void DoLogin()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Username) || Username.Length > 20 ||
                    string.IsNullOrWhiteSpace(Password) || Password.Length > 20)
                {
                    ErrorMessage = "»м€ и пароль: 1Ц20 символов.";
                    OnPropertyChanged(nameof(ErrorMessage));
                    return;
                }
                var user = _authService.Login(Username, Password);
                _authStore.SetCurrentUser(user);
                
                _navigation.Navigate(_taskListVmFactory());
                OnPropertyChanged(nameof(IsLoggedIn));
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        private void Logout()
        {
            _authStore.Logout();
            Username = string.Empty;
            Password = string.Empty;
            OnPropertyChanged(nameof(Username));
            OnPropertyChanged(nameof(Password));
            OnPropertyChanged(nameof(IsLoggedIn));

            _navigation.Navigate(this); // TODO: think that should break history of navigation
        }
    }
}