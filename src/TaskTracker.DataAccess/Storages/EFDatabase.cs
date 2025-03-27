using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;

namespace TaskTracker.DataAccess.Storage
{
    public class EFDatabase : IDatabase
    {
        private readonly EFTaskTrackerContext _context;

        public EFDatabase(EFTaskTrackerContext context)
        {
            _context = context;
            _context.Database.EnsureCreated(); // TODO: delete after adding migrations
        }

        public int AddUser(string username, string hashedPassword)
        {
            var user = new User
            {
                Username = username,
                PasswordHash = hashedPassword
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.Id;
        }

        public int AddTask(string title, string description, int userId, DateTime deadline, TaskPriority priority)
        {
            var task = new Domain.Entities.Task
            {
                Title = title,
                Description = description,
                UserId = userId,
                Deadline = deadline,
                Status = Domain.Enums.TaskStatus.New,
                Priority = priority,
                CreatedDate = DateTime.Now
            };
            _context.Tasks.Add(task);
            _context.SaveChanges();
            return task.Id;
        }

        public void DeleteTask(Domain.Entities.Task task)
        {
            _context.Tasks.Remove(task);
            _context.SaveChanges();
        }

        public User? GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public User? GetUserById(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }

        public Domain.Entities.Task GetTaskById(int taskId)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                throw new Exception($"Задача с Id = {taskId} не найдена");
            }
            return task;
        }

        public List<Domain.Entities.Task> GetTasksByUserId(int userId)
        {
            return _context.Tasks.Where(t => t.UserId == userId).ToList();
        }

        public void UpdateTask(Domain.Entities.Task task)
        {
            _context.Tasks.Update(task);
            _context.SaveChanges();
        }
    }
}
