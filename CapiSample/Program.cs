using CapiSample.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace CapiSample
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                bool isDone = false;
                Directory.CreateDirectory("Reports");

                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                IConfiguration config = builder.Build();
                string connectionString = config.GetConnectionString("remote");

                Console.WriteLine("Доступные для проверки формы:");
                Console.WriteLine("3. Форма 3");
                Console.WriteLine("5. Форма 5");
                Console.WriteLine("6. Форма 6");

                Console.Write("\nВыберите номер формы: ");
                string formNumber = Console.ReadLine();

                IExecutor Executor = null;
                switch (formNumber)
                {
                    case "3":
                        Executor = new Executors.FormThree.Executor(connectionString);
                        break;
                    case "5":
                        Executor = new Executors.FormFive.Executor(connectionString);
                        break;
                    case "6":
                        Executor = new Executors.FormSix.Executor(connectionString);
                        break;
                    default:
                        isDone = true;
                        break;
                }

                if (isDone)
                    break;

                Console.WriteLine("Доступные контроли:");
                Executor.ShowControlsList();

                Console.Write("\nВыберите номер контроля: ");
                string controlNumber = Console.ReadLine();

                Executor.ControlNumber = controlNumber;
                Console.WriteLine();

                Executor.Control.Execute();
                Console.WriteLine();
            }
        }
    }
}
