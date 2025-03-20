using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Utilities;
using TaskTracker.Domain.Interfaces;

namespace TaskTracker.Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User Register(string username, string password)
        {
            var existingUser = _userRepository.GetByUsername(username);
            if (existingUser != null)
            {
                throw new Exception($"User with username {username} already exists");
            }

            var hashedPassword = PasswordHasher.HashPassword(password);
            var userId = _userRepository.Add(username, hashedPassword);
            return _userRepository.GetByUserId(userId);
        }

        public User Login(string username, string password)
        {
            var user = _userRepository.GetByUsername(username);
            if (user == null)
            {
                throw new Exception($"User with username {username} doesn't exist");
            }

            if (PasswordHasher.VerifyPassword(password, user.PasswordHash))
            {
                return user;
            }
            else
            {
                throw new Exception($"Wrong password for user {username}");
            }
        }
    }
}