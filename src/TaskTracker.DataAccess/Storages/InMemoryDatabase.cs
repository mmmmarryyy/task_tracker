using System;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;

namespace TaskTracker.DataAccess.Storage
{
    public class InMemoryDatabase: IDatabase
    {
        private List<User> _users { get; set; } = new List<User>();
        private List<Domain.Entities.Task> _tasks { get; set; } = new List<Domain.Entities.Task>();
        private Random random = new Random();

        public User GetUserById(int userId)
        {
            return _users.FirstOrDefault(u => u.Id == userId);
        }

        public User GetUserByUsername(string username) {
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public int AddUser(string username, string hashedPassword) {
            var id = random.Next();
            var existingUser = GetUserById(id);
            while (existingUser != null)
            {
                id = random.Next();
                existingUser = GetUserById(id);
            }

            var user = new User
            {
                Id = random.Next(),
                Username = username,
                PasswordHash = hashedPassword
            };
            _users.Add(user);
            return user.Id;
        }

        public int AddTask(string title, string description, int userId, DateTime deadline, TaskPriority priority)
        {
            var id = random.Next();
            var existingTask = GetTaskById(id);
            while (existingTask != null)
            {
                id = random.Next();
                existingTask = GetTaskById(id);
            }

            var task = new Domain.Entities.Task
            {
                Id = random.Next(),
                Title = title,
                Description = description,
                UserId = userId,
                Deadline = deadline,
                Status = Domain.Enums.TaskStatus.New,
                Priority = priority,
            };
            _tasks.Add(task);
            return task.Id;
        }

        public void UpdateTask(Domain.Entities.Task task)
        {
            var index = _tasks.FindIndex(t => t.Id == task.Id); // TODO: check what if index is out of bounds
            _tasks[index] = task;
        }

        public void DeleteTask(Domain.Entities.Task task)
        {
            _tasks.Remove(task);
        }

        public Domain.Entities.Task GetTaskById(int taskId) 
        {
            return _tasks.FirstOrDefault(t => t.Id == taskId);
        }

        public List<Domain.Entities.Task> GetTasksByUserId(int userId)
        {
            return _tasks.Where(t => t.UserId == userId).ToList();
        }
    }
}