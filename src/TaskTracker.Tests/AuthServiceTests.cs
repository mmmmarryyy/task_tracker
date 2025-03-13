using Moq;
using TaskTracker.Domain;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Interfaces;
using TaskTracker.Domain.Services;
using TaskTracker.Domain.Utilities;

namespace TaskTracker.Tests
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private AuthService _authService;

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();

            ServiceLocator.UserRepository = _userRepositoryMock.Object;

            _authService = new AuthService();
        }

        [Test]
        public void Register_UserDoesNotExist_ShouldReturnNewUser()
        {
            // Arrange
            string username = "newuser";
            string password = "password123";
            int userId = 1;
            var newUser = new User { Id = userId, Username = username, PasswordHash = PasswordHasher.HashPassword(password) };

            _userRepositoryMock.Setup(repo => repo.GetByUsername(username)).Returns((User)null);
            _userRepositoryMock.Setup(repo => repo.Add(username, It.IsAny<string>())).Returns(userId);
            _userRepositoryMock.Setup(repo => repo.GetByUserId(userId)).Returns(newUser);

            // Act
            var result = _authService.Register(username, password);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(userId, Is.EqualTo(result.Id));
            Assert.That(username, Is.EqualTo(result.Username));
        }

        [Test]
        public void Register_UserAlreadyExists_ShouldThrowException()
        {
            // Arrange
            string username = "existinguser";
            string password = "password123";
            var existingUser = new User { Id = 1, Username = username, PasswordHash = PasswordHasher.HashPassword(password) };

            _userRepositoryMock.Setup(repo => repo.GetByUsername(username)).Returns(existingUser);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _authService.Register(username, password));
            Assert.That(ex.Message, Does.Contain("already exists"));
        }

        [Test]
        public void Login_ValidCredentials_ShouldReturnUser()
        {
            // Arrange
            string username = "validuser";
            string password = "securepass";
            string hashed = PasswordHasher.HashPassword(password);
            var validUser = new User { Id = 2, Username = username, PasswordHash = hashed };

            _userRepositoryMock.Setup(repo => repo.GetByUsername(username)).Returns(validUser);

            // Act
            var result = _authService.Login(username, password);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(validUser.Id, Is.EqualTo(result.Id));
        }

        [Test]
        public void Login_InvalidPassword_ShouldThrowException()
        {
            // Arrange
            string username = "validuser";
            string password = "wrongpass";
            string correctHash = PasswordHasher.HashPassword("correctpass");
            var validUser = new User { Id = 2, Username = username, PasswordHash = correctHash };

            _userRepositoryMock.Setup(repo => repo.GetByUsername(username)).Returns(validUser);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _authService.Login(username, password));
            Assert.That(ex.Message, Does.Contain("Wrong password"));
        }
    }
}
