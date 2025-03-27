using System;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using TaskTracker.Domain.Interfaces;
using TaskTracker.Domain.Enums;
using TaskTracker.Domain.Services;

namespace TaskTracker.UI
{
    class Program
    {
        static void Main()
        {
            var autofacContainer = AutofacContainerSetup.BuildContainer();
            using (var scope = autofacContainer.BeginLifetimeScope())
            {
                var appRunner = scope.Resolve<AppRunner>();
                appRunner.ConfigurationValue = scope.Resolve<string>();
                appRunner.SetEnvironment(scope.Resolve<IEnvironment>());

                appRunner.Run();

                var authService = scope.Resolve<IAuthService>();
                var taskService = scope.Resolve<ITaskService>();

                Console.WriteLine("Choose action:");
                Console.WriteLine("1 - Register");
                Console.WriteLine("2 - Login");

                string choice = Console.ReadLine();
                bool continueAuth = true;

                while (continueAuth)
                {
                    switch (choice)
                    {
                        case "1":
                            Console.WriteLine("Enter Username:");
                            string regUsername = Console.ReadLine();
                            Console.WriteLine("Enter Password:");
                            string regPassword = Console.ReadLine();
                            try
                            {
                                Console.WriteLine("DEBUG try to register in auth service");
                                var newUser = authService.Register(regUsername, regPassword);
                                Console.WriteLine("You are registered; now Login!");
                                choice = "2";
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        case "2":
                            Console.WriteLine("Enter Username:");
                            string loginUsername = Console.ReadLine();
                            Console.WriteLine("Enter Password:");
                            string loginPassword = Console.ReadLine();
                            try
                            {
                                var user = authService.Login(loginUsername, loginPassword);
                                Console.WriteLine("Log in successful!");
                                continueAuth = false;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        default:
                            Console.WriteLine("Invalid choice, please select 1 or 2.");
                            break;
                    }
                    if (continueAuth)
                    {
                        Console.WriteLine("Choose action (1-Register, 2-Login):");
                        choice = Console.ReadLine();
                    }
                }

                bool continueTasks = true;
                while (continueTasks)
                {
                    Console.WriteLine("Choose action:");
                    Console.WriteLine("1 - Create task");
                    Console.WriteLine("2 - Update task");
                    Console.WriteLine("3 - Delete task");
                    Console.WriteLine("4 - Get list of all user tasks");
                    Console.WriteLine("5 - Send notification");
                    Console.WriteLine("6 - Stop program");

                    choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            Console.WriteLine("Enter username:"); // TODO: remember it in future
                            string taskUser = Console.ReadLine();

                            Console.WriteLine("Enter task title:");
                            string title = Console.ReadLine();

                            Console.WriteLine("Enter task description:");
                            string desc = Console.ReadLine();

                            Console.WriteLine("Enter deadline (in format yyyy-MM-dd):"); // TODO: add smarter validation
                            DateTime deadline = DateTime.Parse(Console.ReadLine());

                            Console.WriteLine("Choose Priority (Low, Medium, High):");
                            TaskPriority priority = (TaskPriority)Enum.Parse(typeof(TaskPriority), Console.ReadLine());

                            try
                            {
                                int taskId = taskService.CreateTask(title, desc, taskUser, deadline, priority);
                                Console.WriteLine($"Created task successfully! Id = {taskId}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        case "2":
                            // TODO:
                            Console.WriteLine("Sorry, you can't update task now");
                            break;
                        case "3":
                            Console.Write("Enter ID for task you want to delete: ");
                            int deleteId = int.Parse(Console.ReadLine());

                            try
                            {
                                taskService.DeleteTask(deleteId);
                                Console.WriteLine("Delete task successfully!");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                            break;
                        case "4":
                            Console.Write("Enter your username: ");
                            string userNameForTasks = Console.ReadLine();

                            try
                            {
                                var tasks = taskService.GetTasksByUsername(userNameForTasks);
                                if (tasks.Count == 0)
                                {
                                    Console.WriteLine("Can't find your tasks");
                                }
                                else
                                {
                                    foreach (var t in tasks)
                                    {
                                        Console.WriteLine($"ID: {t.Id}, Title: {t.Title}, Deadline: {t.Deadline.ToShortDateString()}, Status: {t.Status}, Priority: {t.Priority}");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        case "5":
                            var serviceProvider = MicrosoftDIContainerSetup.BuildServiceProvider();
                            var notificationService = serviceProvider.GetRequiredService<INotificationService>();
                            notificationService.Notify("Notification sent from menu action 5");
                            break;
                        case "6":
                            continueTasks = false;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please select a valid option.");
                            break;
                    }
                }
            }
        }
    }
}
