using CapiSample.Form3;
using CapiSample.Form5;
using CapiSample.Form6;
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
                Directory.CreateDirectory("Reports");

                Console.WriteLine("Availabel controls:");
                Console.WriteLine("1: Форма 3. Раздел 1. Единицы измерения.");
                Console.WriteLine("2: Форма 3. Разедл 2. Единицы измерения.");
                Console.WriteLine("3: Форма 3. Раздел 2. Источники поступления.");
                Console.WriteLine("4: Форма 5. Единицы измерения.");
                Console.WriteLine("5: Форма 5. Материал.");
                Console.WriteLine("6: Форма 5. Для кого куплено.");
                Console.WriteLine("7: Форма 6. Раздел 2. Пункт 1. Расходы на топливо."); // всегда успешно. создан для примера запроса.
                Console.WriteLine("8: Форма 6. Раздел 2. Пункт 1. Стоимость купленного топлива.");
                Console.WriteLine("9: Форма 6. Раздел 2. Оплата за аренду жилья.");
                Console.WriteLine("10: Форма 6. Раздел 2. Если есть льготы, указать процент.");

                Console.Write("\nType control number: ");

                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                IConfiguration config = builder.Build();
                string connectionString = config.GetConnectionString("remote");

                IControl Control = new FormThreeSectionOneUnits(connectionString);
                string controlName = Console.ReadLine();

                switch (controlName)
                {
                    case "1":
                        Control = new FormThreeSectionOneUnits(connectionString);
                        break;
                    case "2":
                        Control = new FormThreeSectionTwoUnits(connectionString);
                        break;
                    case "3":
                        Control = new FormThreeSectionTwoSupplySources(connectionString);
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
                    case "7":
                        Control = new FormSixSectionTwoQuestionOneOccupancy(connectionString);
                        break;
                    case "8":
                        Control = new FormSixSectionTwoQuestionOneCost(connectionString);
                        break;
                    case "9":
                        Control = new FormSixSectionTwoRentPaymentQuestion(connectionString);
                        break;
                    case "10":
                        Control = new FormSixSectionTwoServices(connectionString);
                        break;
                }

                Control.Execute();
            }
        }
    }
}
