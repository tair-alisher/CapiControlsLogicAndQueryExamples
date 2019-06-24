using CapiSample.Form3;
using CapiSample.Form5;
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
            Directory.CreateDirectory("Reports");

            Console.WriteLine("Availabel controls:");
            Console.WriteLine("1: Форма 3. Раздел 1. Единицы измерения.");
            Console.WriteLine("2: Форма 3. Разедл 2. Единицы измерения.");
            Console.WriteLine("3: Форма 3. Раздел 2. Источники поступления.");
            Console.WriteLine("4: Форма 5. Единицы измерения.");
            Console.WriteLine("5: Форма 5. Материал.");
            Console.WriteLine("6: Форма 5. Для кого куплено.");

            Console.Write("\nType control number: ");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration config = builder.Build();
            string connectionString = config.GetConnectionString("remote");

            IControl Control = new SectionOneUnitsControl(connectionString);
            string controlName = Console.ReadLine();

            switch (controlName)
            {
                case "1":
                    Control = new SectionOneUnitsControl(connectionString);
                    break;
                case "2":
                    Control = new SectionTwoUnitsControl(connectionString);
                    break;
                case "3":
                    Control = new SectionTwoSupplySourcesControl(connectionString);
                    break;
                case "4":
                    Control = new FormFiveUnitsControl(connectionString);
                    break;
                case "5":
                    Control = new FormFiveMaterialsControl(connectionString);
                    break;
                case "6":
                    Control = new FormFiveItemUsersControl(connectionString);
                    break;
            }

            Control.Execute();

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }
    }
}
