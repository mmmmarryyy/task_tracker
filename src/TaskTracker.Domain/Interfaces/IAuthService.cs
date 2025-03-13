using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Interfaces
{
    public interface IAuthService
    {
        User Register(string username, string password);
        User Login(string username, string password);
    }
}