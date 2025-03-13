using TaskTracker.Domain.Interfaces;

namespace TaskTracker.Domain
{
    public static class ServiceLocator
    {
        public static IUserRepository UserRepository { get; set; }
        public static ITaskRepository TaskRepository { get; set; }
    }
}