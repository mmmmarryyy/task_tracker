using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Interfaces
{
    public interface ITaskRepository
    {
        int Add(string title, string description, int userId, DateTime deadline, TaskPriority priority);
        void Update(Entities.Task task);
        void Delete(Entities.Task task);
        Entities.Task GetById(int taskId);
        List<Entities.Task> GetTasksByUserId(int userId);
    }
}