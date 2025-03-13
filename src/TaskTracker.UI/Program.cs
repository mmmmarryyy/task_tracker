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

            Console.WriteLine("�������� ��������:");
            Console.WriteLine("1 - ����������");
            Console.WriteLine("2 - �����");

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
                            return;
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
                            return;
                        }
                        break;
                }
            }

            continueFlag = true;

            while (continueFlag)
            {
                Console.WriteLine("�������� ��������:");
                Console.WriteLine("1 - ������� ������");
                Console.WriteLine("2 - �������� ������");
                Console.WriteLine("3 - ������� ������");
                Console.WriteLine("4 - �������� ������ ����� ������������");
                Console.WriteLine("5 - ��������� ������ ���������");

                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        // TODO
                        break;
                    case "2":
                        // TODO
                        break;
                    case "3":
                        // TODO
                        break;
                    case "4":
                        // TODO
                        break;
                    case "5":
                        continueFlag = false;
                        break;
                }
            }
        }
    }
}