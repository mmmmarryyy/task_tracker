using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Interfaces
{
    public interface ITaskService
    {
        int CreateTask(string title, string description, string username, DateTime deadline, TaskPriority priority);
        void UpdateTaskTitle(int taskId, string title);
        void UpdateTaskDescription(int taskId, string description);
        void UpdateTaskDeadline(int taskId, DateTime deadline);
        void UpdateTaskStatus(int taskId, Enums.TaskStatus status);
        void UpdateTaskPriority(int taskId, TaskPriority priority);
        void DeleteTask(int taskId);
        Entities.Task GetTaskById(int taskId);
        List<Entities.Task> GetTasksByUserId(int userId);
        List<Entities.Task> GetTasksByUsername(string username);
    }
}