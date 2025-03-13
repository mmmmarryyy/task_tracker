using TaskTracker.Domain.Enums;
using TaskTracker.Domain.Interfaces;

namespace TaskTracker.Domain.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITaskRepository _taskRepository;

        public TaskService()
        {
            // TODO: check that this repositories are not null
            _userRepository = ServiceLocator.UserRepository;
            _taskRepository = ServiceLocator.TaskRepository;
        }

        public int CreateTask(string title, string description, string username, DateTime deadline, TaskPriority priority)
        {
            var user = _userRepository.GetByUsername(username);
            if (user == null)
            {
                throw new Exception($"Can't find user {username}");
            }

            return _taskRepository.Add(title, description, user.Id, deadline, priority);
        }

        public void UpdateTaskTitle(int taskId, string title)
        {
            var task = _taskRepository.GetById(taskId);
            if (task != null)
            {
                task.Title = title;
                _taskRepository.Update(task);
            } 
            else
            {
                throw new Exception($"Can't find task with ID = {taskId}");
            }
        }

        public void UpdateTaskDescription(int taskId, string description)
        {
            var task = _taskRepository.GetById(taskId);
            if (task != null)
            {
                task.Description = description;
                _taskRepository.Update(task);
            }
            else
            {
                throw new Exception($"Can't find task with ID = {taskId}");
            }
        }

        public void UpdateTaskDeadline(int taskId, DateTime deadline)
        {
            var task = _taskRepository.GetById(taskId);
            if (task != null)
            {
                task.Deadline = deadline;
                _taskRepository.Update(task);
            }
            else
            {
                throw new Exception($"Can't find task with ID = {taskId}");
            }
        }


        public void UpdateTaskStatus(int taskId, Enums.TaskStatus status)
        {
            var task = _taskRepository.GetById(taskId);
            if (task != null)
            {
                task.Status = status;
                _taskRepository.Update(task);
            }
            else
            {
                throw new Exception($"Can't find task with ID = {taskId}");
            }
        }

        public void UpdateTaskPriority(int taskId, TaskPriority priority)
        {
            var task = _taskRepository.GetById(taskId);
            if (task != null)
            {
                task.Priority = priority;
                _taskRepository.Update(task);
            }
            else
            {
                throw new Exception($"Can't find task with ID = {taskId}");
            }
        }

        public void DeleteTask(int taskId)
        {
            var task = _taskRepository.GetById(taskId);
            if (task != null)
            {
                _taskRepository.Delete(task);
            }
            else
            {
                throw new Exception($"Can't find task with ID = {taskId}");
            }
        }

        public Entities.Task GetTaskById(int taskId)
        {
            return _taskRepository.GetById(taskId);
        }

        public List<Entities.Task> GetTasksByUserId(int userId)
        {
            return _taskRepository.GetTasksByUserId(userId);
        }

        public List<Entities.Task> GetTasksByUsername(string username)
        {
            var user = _userRepository.GetByUsername(username);
            if (user == null)
            {
                throw new Exception($"Can't find user {username}");
            }

            return _taskRepository.GetTasksByUserId(user.Id);
        }
    }
}