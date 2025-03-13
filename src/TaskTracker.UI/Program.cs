using System.Reflection;
using TaskTracker.Domain.Interfaces;
using TaskTracker.Domain.Services;

namespace TaskTracker.UI
{
    class Program
    {
        static void Main()
        {
            Assembly dataAccessAssembly = Assembly.Load("TaskTracker.DataAccess");
            Type registerType = dataAccessAssembly.GetType("TaskTracker.DataAccess.DependencyRegister");
            registerType.GetMethod("RegisterDependencies").Invoke(null, null);

            IAuthService authService = new AuthService();
            ITaskService taskService = new TaskService();

            Console.WriteLine("Choose action:");
            Console.WriteLine("1 - Register");
            Console.WriteLine("2 - Login");

            string choice = Console.ReadLine();

            bool continueFlag = true;

            while (continueFlag)
            {
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Enter Username");
                        string regUsername = Console.ReadLine();
                        Console.WriteLine("Enter Password");
                        string regPassword = Console.ReadLine();
                        try
                        {
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
                        Console.WriteLine("Enter Username");
                        string loginUsername = Console.ReadLine();
                        Console.WriteLine("Enter Password");
                        string loginPassword = Console.ReadLine();
                        try
                        {
                            var newUser = authService.Register(loginUsername, loginPassword);
                            Console.WriteLine("Log in successful!");
                            continueFlag = false;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                }
            }

            continueFlag = true;

            while (continueFlag)
            {
                Console.WriteLine("Choose action:");
                Console.WriteLine("1 - Create task");
                Console.WriteLine("2 - Update task");
                Console.WriteLine("3 - Delete task");
                Console.WriteLine("4 - Get list of all user tasks");
                Console.WriteLine("5 - Stop program");

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
                        continueFlag = false;
                        break;
                }
            }
        }
    }
}