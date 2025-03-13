using Moq;
using TaskTracker.Domain;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;
using TaskTracker.Domain.Interfaces;
using TaskTracker.Domain.Services;

namespace TaskTracker.Tests
{
    [TestFixture]
    public class TaskServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<ITaskRepository> _taskRepositoryMock;
        private TaskService _taskService;

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _taskRepositoryMock = new Mock<ITaskRepository>();

            ServiceLocator.UserRepository = _userRepositoryMock.Object;
            ServiceLocator.TaskRepository = _taskRepositoryMock.Object;

            _taskService = new TaskService();
        }

        [Test]
        public void CreateTask_ValidUser_ShouldReturnTaskId()
        {
            // Arrange
            string username = "testuser";
            int userId = 10;
            int taskId = 100;
            DateTime deadline = DateTime.Now.AddDays(1);

            var user = new User { Id = userId, Username = username, PasswordHash = "hash" };

            _userRepositoryMock.Setup(repo => repo.GetByUsername(username)).Returns(user);
            _taskRepositoryMock.Setup(repo => repo.Add("Test Task", "Test Description", userId, deadline, TaskPriority.Medium))
                               .Returns(taskId);

            // Act
            int result = _taskService.CreateTask("Test Task", "Test Description", username, deadline, TaskPriority.Medium);

            // Assert
            Assert.That(taskId, Is.EqualTo(result));
            _taskRepositoryMock.Verify(repo => repo.Add("Test Task", "Test Description", userId, deadline, TaskPriority.Medium), Times.Once);
        }

        [Test]
        public void CreateTask_UserNotFound_ShouldThrowException()
        {
            // Arrange
            string username = "nonexistent";
            DateTime deadline = DateTime.Now.AddDays(1);

            _userRepositoryMock.Setup(repo => repo.GetByUsername(username)).Returns((User)null);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _taskService.CreateTask("Task", "Desc", username, deadline, TaskPriority.Low));
            Assert.That(ex.Message, Does.Contain("Can't find user"));
        }

        [Test]
        public void UpdateTaskTitle_TaskExists_ShouldUpdateTitle()
        {
            // Arrange
            int taskId = 200;
            string newTitle = "Updated Title";
            var task = new Domain.Entities.Task
            {
                Id = taskId,
                Title = "Old Title",
                Description = "Desc",
                UserId = 1,
                Deadline = DateTime.Now.AddDays(1),
                Status = Domain.Enums.TaskStatus.New,
                Priority = TaskPriority.Low
            };

            _taskRepositoryMock.Setup(repo => repo.GetById(taskId)).Returns(task);
            _taskRepositoryMock.Setup(repo => repo.Update(It.IsAny<Domain.Entities.Task>()));

            // Act
            _taskService.UpdateTaskTitle(taskId, newTitle);

            // Assert
            _taskRepositoryMock.Verify(repo => repo.Update(
                It.Is<Domain.Entities.Task>(t => t.Id == taskId && t.Title == newTitle)
            ), Times.Once);
        }

        [Test]
        public void UpdateTaskTitle_TaskDoesNotExist_ShouldThrowException()
        {
            // Arrange
            int taskId = 300;
            _taskRepositoryMock.Setup(repo => repo.GetById(taskId)).Returns((Domain.Entities.Task)null);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _taskService.UpdateTaskTitle(taskId, "New Title"));
            Assert.That(ex.Message, Does.Contain("Can't find task"));
        }

        [Test]
        public void DeleteTask_TaskExists_ShouldCallDelete()
        {
            // Arrange
            int taskId = 400;
            var task = new Domain.Entities.Task
            {
                Id = taskId,
                Title = "Task to delete",
                Description = "Desc",
                UserId = 1,
                Deadline = DateTime.Now.AddDays(1),
                Status = Domain.Enums.TaskStatus.New,
                Priority = TaskPriority.Low
            };

            _taskRepositoryMock.Setup(repo => repo.GetById(taskId)).Returns(task);
            _taskRepositoryMock.Setup(repo => repo.Delete(task)).Callback(() =>
            {
                _taskRepositoryMock.Setup(repo => repo.GetById(taskId)).Returns((Domain.Entities.Task)null);
            });

            // Act
            _taskService.DeleteTask(taskId);

            // Assert
            _taskRepositoryMock.Verify(repo => repo.Delete(task), Times.Once);
            Assert.That(_taskRepositoryMock.Object.GetById(taskId), Is.Null);
        }

        [Test]
        public void GetTasksByUsername_ExistingUser_ShouldReturnTasks()
        {
            // Arrange
            string username = "userWithTasks";
            int userId = 5;
            var user = new User { Id = userId, Username = username, PasswordHash = "hash" };
            var tasks = new List<Domain.Entities.Task>
            {
                new Domain.Entities.Task { Id = 1, Title = "Task 1", UserId = userId, Deadline = DateTime.Now.AddDays(1), Status = Domain.Enums.TaskStatus.New, Priority = TaskPriority.Medium },
                new Domain.Entities.Task { Id = 2, Title = "Task 2", UserId = userId, Deadline = DateTime.Now.AddDays(2), Status = Domain.Enums.TaskStatus.InProgress, Priority = TaskPriority.High }
            };

            _userRepositoryMock.Setup(repo => repo.GetByUsername(username)).Returns(user);
            _taskRepositoryMock.Setup(repo => repo.GetTasksByUserId(userId)).Returns(tasks);

            // Act
            var result = _taskService.GetTasksByUsername(username);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(tasks.Count, Is.EqualTo(result.Count));
            _taskRepositoryMock.Verify(repo => repo.GetTasksByUserId(userId), Times.Once);
        }

        [Test]
        public void GetTasksByUsername_UserNotFound_ShouldThrowException()
        {
            // Arrange
            string username = "unknown";
            _userRepositoryMock.Setup(repo => repo.GetByUsername(username)).Returns((User)null);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _taskService.GetTasksByUsername(username));
            Assert.That(ex.Message, Does.Contain("Can't find user"));
        }
    }
}
