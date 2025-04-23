using System;
using System.Windows.Input;
using TaskTracker.Domain.Interfaces;
using TaskTracker.Domain.Enums;
using TaskTracker.Domain.Entities;
using TaskTracker.UI.Stores;
using TaskTracker.UI.Services;
using System.Collections.ObjectModel;


namespace TaskTracker.UI.ViewModels
{
    public class TaskEditViewModel : ViewModelBase
    {
        private readonly ITaskService _taskService;
        private readonly NavigationStore _navigation;
        private readonly AuthenticationStore _authStore;
        private readonly bool _isEdit;

        public int? Id { get; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; } = DateTime.Now;
        public TaskPriority Priority { get; set; }
        public static Array TaskPriorityValues => Enum.GetValues(typeof(TaskPriority));
        public Domain.Enums.TaskStatus Status { get; set; } = Domain.Enums.TaskStatus.New;
        public static Array TaskStatusValues => Enum.GetValues(typeof(Domain.Enums.TaskStatus));


        public ICommand SaveCommand { get; }

        public TaskEditViewModel(
            ITaskService taskService,
            NavigationStore navigation,
            AuthenticationStore authStore
        ) { 
            _taskService = taskService;
            _navigation = navigation;
            _authStore = authStore;
            SaveCommand = new RelayCommand(_ => CreateTask());
        }
        
        public TaskEditViewModel(
            ITaskService taskService,
            NavigationStore navigation,
            AuthenticationStore authStore,
            Domain.Entities.Task existing
        ) : this(taskService, navigation, authStore)
        {
            _isEdit = true;
            Id = existing.Id;
            Title = existing.Title;
            Description = existing.Description;
            Deadline = existing.Deadline;
            Priority = existing.Priority;
            Status = existing.Status;
            SaveCommand = new RelayCommand(_ => UpdateTask());
        }

        private void CreateTask()
        {
            _taskService.CreateTask(
                Title,
                Description,
                _authStore.CurrentUser.Username,
                Deadline,
                Priority
            );

            _navigation.Navigate(new TaskListViewModel(_taskService, _navigation, _authStore));

            if (_navigation.CurrentViewModel is TaskListViewModel taskList)
            {
                taskList.ReloadTasks();
            }
        }

        private void UpdateTask()
        {
            _taskService.UpdateTaskTitle(Id!.Value, Title);
            _taskService.UpdateTaskDescription(Id.Value, Description);
            _taskService.UpdateTaskDeadline(Id.Value, Deadline);
            _taskService.UpdateTaskPriority(Id.Value, Priority);
            _taskService.UpdateTaskStatus(Id!.Value, Status);

            _navigation.Navigate(new TaskListViewModel(_taskService, _navigation, _authStore));

            if (_navigation.CurrentViewModel is TaskListViewModel taskList)
            {
                taskList.ReloadTasks();
            }
        }
    }
}