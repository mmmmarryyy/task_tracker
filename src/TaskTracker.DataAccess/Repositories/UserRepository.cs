using TaskTracker.DataAccess.Storage;
using TaskTracker.Domain.Interfaces;
using TaskTracker.Domain.Entities;

namespace TaskTracker.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDatabase _database;

        public UserRepository(IDatabase database)
        {
            _database = database;
        }

        public User GetByUserId(int userId)
        {
            return _database.GetUserById(userId);
        }

        public User GetByUsername(string username)
        {
            return _database.GetUserByUsername(username);
        }

        public int Add(string username, string passwordHash)
        {
            return _database.AddUser(username, passwordHash);
        }
    }
}