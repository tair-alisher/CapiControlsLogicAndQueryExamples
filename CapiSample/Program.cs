using CapiSample.Form3;
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
            Console.WriteLine("1: sectionOneUnits");
            Console.WriteLine("2: sectionTwoUnits");
            Console.WriteLine("3: sectionTwoSupplySources");
            Console.WriteLine();

            Console.Write("Type control number: ");

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
            }

            Control.Execute();

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }
    }
}
