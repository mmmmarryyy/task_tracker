using TaskTracker.DataAccess.Storage;
using TaskTracker.Domain.Interfaces;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;

namespace TaskTracker.DataAccess.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IDatabase _database;

        public TaskRepository(IDatabase database)
        {
            _database = database;
        }

        public int Add(string title, string description, int userId, DateTime deadline, TaskPriority priority)
        {
            return _database.AddTask(title, description, userId, deadline, priority);
        }

        public void Update(Domain.Entities.Task task)
        {
            _database.UpdateTask(task);
        }

        public void Delete(Domain.Entities.Task task)
        {
            _database.DeleteTask(task);
        }

        public Domain.Entities.Task GetById(int taskId)
        {
            return _database.GetTaskById(taskId);
        }

        public List<Domain.Entities.Task> GetTasksByUserId(int userId)
        {
            return _database.GetTasksByUserId(userId);
        }
    }
}