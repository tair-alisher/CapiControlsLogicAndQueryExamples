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
                bool done = false;
                Directory.CreateDirectory("Reports");

                Console.WriteLine("Доступные контроли:");
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
                Console.WriteLine("11. Форма 6. Раздел 3. Обращения за мед. помощью.");
                Console.WriteLine("12. Форма 6. Раздел 3. Расходы на амбулаторное лечение.");
                Console.WriteLine("13. Форма 6. Раздел 3. Госпитализация.");
                Console.WriteLine("14. Форма 6. Раздел 3. Расходы на стационарное лечение.");
                Console.WriteLine("15. Форма 6. Раздел 3. Расходы на мед. принадлежности, лекарство.");
                Console.WriteLine("16. Форма 6. Раздел 4. Расходы на транспорт.");
                Console.WriteLine("17. Форма 6. Раздел 5. Расходы на дошкольное образование.");
                Console.WriteLine("18. Форма 6. Раздел 5. Расходы на образование.");
                Console.WriteLine("19. Форма 6. Раздел 6. Покупка недвижимости.");
                Console.WriteLine("20. Форма 6. Раздел 6. Трата сбережений.");
                Console.WriteLine("21. Форма 6. Раздел 6. Вложение сбережений.");
                Console.WriteLine("22. Форма 6. Раздел 6. Брали ли в долг или кредит.");

                Console.Write("\nВыберите номер контроля: ");

                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                IConfiguration config = builder.Build();
                string connectionString = config.GetConnectionString("remote");

                IControl Control = null;
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
                    case "11":
                        Control = new FormSixSectionThreeTreatment(connectionString);
                        break;
                    case "12":
                        Control = new FormSixSectionThreeTreatmentCost(connectionString);
                        break;
                    case "13":
                        Control = new FormSixSectionThreeHospitalization(connectionString);
                        break;
                    case "14":
                        Control = new FormSixSectionThreeHospitalizationCost(connectionString);
                        break;
                    case "15":
                        Control = new FormSixSectionThreeMedicine(connectionString);
                        break;
                    case "16":
                        Control = new FormSixSectionFour(connectionString);
                        break;
                    case "17":
                        Control = new FormSixSectionFivePreSchoolEduCost(connectionString);
                        break;
                    case "18":
                        Control = new FormSixSectionFiveEducation(connectionString);
                        break;
                    case "19":
                        Control = new FormSixSectionSixPropertyPurchase(connectionString);
                        break;
                    case "20":
                        Control = new FormSixSectionSixSavingsWaste(connectionString);
                        break;
                    case "21":
                        Control = new FormSixSectionSixSavingsContribution(connectionString);
                        break;
                    case "22":
                        Control = new FormSixSectionSixLoanGetting(connectionString);
                        break;
                    default:
                        done = true;
                        break;
                }

                if (done)
                    break;

                Console.WriteLine();
                Control.Execute();
                Console.WriteLine();
            }
        }
    }
}
