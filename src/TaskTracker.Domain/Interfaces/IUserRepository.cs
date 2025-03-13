using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Interfaces
{
    public interface IUserRepository
    {
        User GetByUsername(string username);
        User GetByUserId(int userId);
        int Add(string username, string hashedPassword);
    }
}