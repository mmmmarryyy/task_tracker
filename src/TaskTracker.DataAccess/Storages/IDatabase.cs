using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;

namespace TaskTracker.DataAccess.Storage
{
    public interface IDatabase
    {
        User? GetUserByUsername(string username);
        User? GetUserById(int userId);
        int AddUser(string username, string hashedPassword);

        int AddTask(string title, string description, int userId, DateTime deadline, TaskPriority priority);
        void UpdateTask(Domain.Entities.Task task);
        void DeleteTask(Domain.Entities.Task task);
        Domain.Entities.Task GetTaskById(int taskId);
        List<Domain.Entities.Task> GetTasksByUserId(int userId);
    }
}