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
    public class TaskListViewModel : ViewModelBase
    {
        public ObservableCollection<Domain.Entities.Task> Tasks { get; }
        public ICommand AddTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }
        public ICommand EditTaskCommand { get; }
        public ICommand ShowDetailsCommand { get; }

        private readonly ITaskService _taskService;
        private readonly NavigationStore _navigation;
        private readonly AuthenticationStore _authStore;

        public TaskListViewModel(
            ITaskService taskService,
            NavigationStore navigation,
            AuthenticationStore authStore
        ) {
            _taskService = taskService;
            _navigation = navigation;
            _authStore = authStore;

            var username = authStore.CurrentUser?.Username;
            if (string.IsNullOrEmpty(username))
            {
                throw new InvalidOperationException("User must be logged in");
            }

            var list = _taskService.GetTasksByUsername(username);
            Tasks = new ObservableCollection<Domain.Entities.Task>(list);

            AddTaskCommand = new RelayCommand(_ =>
                _navigation.Navigate(
                    new TaskEditViewModel(taskService, navigation, authStore)));

            DeleteTaskCommand = new RelayCommand(id =>
            {
                _taskService.DeleteTask((int)id);
                var toRemove = Tasks.FirstOrDefault(t => t.Id == (int)id);
                if (toRemove != null) Tasks.Remove(toRemove);
            });

            EditTaskCommand = new RelayCommand(id =>
            {
                var existing = _taskService.GetTaskById((int)id);
                _navigation.Navigate(
                    new TaskEditViewModel(taskService, navigation, authStore, existing));
            });

            ShowDetailsCommand = new RelayCommand(id =>
            {
                var t = Tasks.First(x => x.Id == (int)id);
                MessageBox.Show(t.Description, $"Задача {t.Title}");
            });
        }

        public void ReloadTasks()
        {
            Tasks.Clear();
            var updated = _taskService.GetTasksByUsername(_authStore.CurrentUser!.Username);
            foreach (var task in updated)
                Tasks.Add(task);
        }
    }
}