using TaskTracker.DataAccess.Repositories;
using TaskTracker.DataAccess.Storage;
using TaskTracker.Domain;

namespace TaskTracker.DataAccess
{
    public static class DependencyRegister
    {
        public static void RegisterDependencies()
        {
            IDatabase database = new InMemoryDatabase();
            ServiceLocator.UserRepository = new UserRepository(database);
            ServiceLocator.TaskRepository = new TaskRepository(database);
        }
    }
}