using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TaskTracker.DataAccess.Storage;
using TaskTracker.DataAccess;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;
using Xunit;

namespace TaskTracker.Tests
{
    public class EFDatabaseTests
    {
        private EFTaskTrackerContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<EFTaskTrackerContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new EFTaskTrackerContext(options);

            SeedTestData(context);
            return context;
        }

        private void SeedTestData(EFTaskTrackerContext context)
        {
            context.Users.AddRange(
                new User { Id = 1, Username = "admin", PasswordHash = "hash_admin" },
                new User { Id = 2, Username = "user1", PasswordHash = "hash_user1" }
            );
            context.SaveChanges();
            context.Tasks.AddRange(
                new Domain.Entities.Task { Id = 1, Title = "Задача 1", Description = "Описание 1", CreatedDate = DateTime.Now, Deadline = DateTime.Now.AddDays(5), Status = Domain.Enums.TaskStatus.New, Priority = TaskPriority.Medium, UserId = 1 },
                new Domain.Entities.Task { Id = 2, Title = "Задача 2", Description = "Описание 2", CreatedDate = DateTime.Now, Deadline = DateTime.Now.AddDays(3), Status = Domain.Enums.TaskStatus.New, Priority = TaskPriority.High, UserId = 1 }
            );
            context.SaveChanges();
        }

        [Fact]
        public void AddUser_ShouldAddUserAndReturnValidId()
        {
            using var context = GetInMemoryContext();
            var efDb = new EFDatabase(context);

            int newUserId = efDb.AddUser("testuser", "hash_test");
            Xunit.Assert.True(newUserId > 0);
            var user = context.Users.Find(newUserId);
            Xunit.Assert.NotNull(user);
            Xunit.Assert.Equal("testuser", user.Username);
        }

        [Fact]
        public void GetUserByUsername_ShouldReturnCorrectUser()
        {
            using var context = GetInMemoryContext();
            var efDb = new EFDatabase(context);

            var user = efDb.GetUserByUsername("admin");
            Xunit.Assert.NotNull(user);
            Xunit.Assert.Equal(1, user.Id);
        }

        [Fact]
        public void GetUserById_ShouldReturnCorrectUser()
        {
            using var context = GetInMemoryContext();
            var efDb = new EFDatabase(context);

            var user = efDb.GetUserById(2);
            Xunit.Assert.NotNull(user);
            Xunit.Assert.Equal("user1", user.Username);
        }

        [Fact]
        public void AddTask_ShouldAddTaskAndReturnValidId()
        {
            using var context = GetInMemoryContext();
            var efDb = new EFDatabase(context);
            int userId = efDb.AddUser("taskuser", "hash_taskuser");

            int taskId = efDb.AddTask("Новая задача", "Описание", userId, DateTime.Now.AddDays(7), TaskPriority.Low);
            Xunit.Assert.True(taskId > 0);
            var task = context.Tasks.Find(taskId);
            Xunit.Assert.NotNull(task);
            Xunit.Assert.Equal("Новая задача", task.Title);
        }

        [Fact]
        public void GetTaskById_ShouldReturnCorrectTask()
        {
            using var context = GetInMemoryContext();
            var efDb = new EFDatabase(context);

            var task = efDb.GetTaskById(1);
            Xunit.Assert.NotNull(task);
            Xunit.Assert.Equal("Задача 1", task.Title);
        }

        [Fact]
        public void GetTasksByUserId_ShouldReturnAllTasksForUser()
        {
            using var context = GetInMemoryContext();
            var efDb = new EFDatabase(context);

            var tasks = efDb.GetTasksByUserId(1);
            Xunit.Assert.NotEmpty(tasks);
            Xunit.Assert.True(tasks.All(t => t.UserId == 1));
        }

        [Fact]
        public void UpdateTask_ShouldModifyTaskProperties()
        {
            using var context = GetInMemoryContext();
            var efDb = new EFDatabase(context);

            var task = efDb.GetTaskById(1);
            task.Title = "Обновлённая задача";
            efDb.UpdateTask(task);

            var updatedTask = context.Tasks.Find(1);
            Xunit.Assert.Equal("Обновлённая задача", updatedTask.Title);
        }

        [Fact]
        public void DeleteTask_ShouldRemoveTaskFromDatabase()
        {
            using var context = GetInMemoryContext();
            var efDb = new EFDatabase(context);

            var task = efDb.GetTaskById(2);
            efDb.DeleteTask(task);

            Xunit.Assert.Throws<Exception>(() => efDb.GetTaskById(2));
        }

        [Fact]
        public void AddUser_WithDuplicateUsername_ShouldAddIndependentUser()
        {
            using var context = GetInMemoryContext();
            var efDb = new EFDatabase(context);

            int id1 = efDb.AddUser("duplicate", "hash1");
            int id2 = efDb.AddUser("duplicate", "hash2");
            Xunit.Assert.NotEqual(id1, id2);
        }

        [Fact]
        public void UpdateTask_NonExistingTask_ShouldThrowException()
        {
            using var context = GetInMemoryContext();
            var efDb = new EFDatabase(context);
            var fakeTask = new Domain.Entities.Task { Id = 999, Title = "Fake", Description = "Fake", CreatedDate = DateTime.Now, Deadline = DateTime.Now.AddDays(1), Status = Domain.Enums.TaskStatus.New, Priority = TaskPriority.Low, UserId = 1 };
            Xunit.Assert.Throws<DbUpdateConcurrencyException>(() => efDb.UpdateTask(fakeTask));
        }
    }
}
