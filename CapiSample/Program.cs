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

                Console.WriteLine("Доступные контроли:");
                switch (formNumber)
                {
                    case "3":
                        Console.WriteLine("1: Форма 3. Раздел 1. Единицы измерения.");
                        Console.WriteLine("2: Форма 3. Разедл 2. Единицы измерения.");
                        Console.WriteLine("3: Форма 3. Раздел 2. Источники поступления.");
                        break;
                    case "5":
                        Console.WriteLine("1: Форма 5. Единицы измерения.");
                        Console.WriteLine("2: Форма 5. Материал.");
                        Console.WriteLine("3: Форма 5. Для кого куплено.");
                        break;
                    case "6":
                        Console.WriteLine("1: Форма 6. Раздел 2. Пункт 1. Расходы на топливо.");
                        Console.WriteLine("2: Форма 6. Раздел 2. Пункт 1. Стоимость купленного топлива.");
                        Console.WriteLine("3: Форма 6. Раздел 2. Оплата за аренду жилья.");
                        Console.WriteLine("4: Форма 6. Раздел 2. Если есть льготы, указать процент.");
                        Console.WriteLine("5. Форма 6. Раздел 3. Обращения за мед. помощью.");
                        Console.WriteLine("6. Форма 6. Раздел 3. Расходы на амбулаторное лечение.");
                        Console.WriteLine("7. Форма 6. Раздел 3. Госпитализация.");
                        Console.WriteLine("8. Форма 6. Раздел 3. Расходы на стационарное лечение.");
                        Console.WriteLine("9. Форма 6. Раздел 3. Расходы на мед. принадлежности, лекарство.");
                        Console.WriteLine("10. Форма 6. Раздел 4. Расходы на транспорт.");
                        Console.WriteLine("11. Форма 6. Раздел 5. Расходы на дошкольное образование.");
                        Console.WriteLine("12. Форма 6. Раздел 5. Расходы на образование.");
                        Console.WriteLine("13. Форма 6. Раздел 6. Покупка недвижимости.");
                        Console.WriteLine("14. Форма 6. Раздел 6. Трата сбережений.");
                        Console.WriteLine("15. Форма 6. Раздел 6. Вложение сбережений.");
                        Console.WriteLine("16. Форма 6. Раздел 6. Брали ли в долг или кредит.");
                        Console.WriteLine("17. Форма 6. Раздел 6. Возврат долга или кредита.");
                        Console.WriteLine("18. Форма 6. Раздел 6. Уплата налога на недвижимость.");
                        Console.WriteLine("19. Форма 6. Раздел 6. Уплата подоходного налога.");
                        Console.WriteLine("20. Форма 6. Раздел 6. Выплата членских взносов.");
                        Console.WriteLine("21. Форма 6. Раздел 6. Взносы по страхованию.");
                        Console.WriteLine("22. Форма 6. Раздел 6. Траты на техосмотр.");
                        Console.WriteLine("23. Форма 6. Раздел 6. Давали ли деньги в долг.");
                        break;
                    default:
                        done = true;
                        break;
                }

                if (done)
                    break;

                Console.Write("\nВыберите номер контроля: ");

                IControl Control = null;
                string controlCollectedNumber = formNumber + Console.ReadLine();

                switch (controlCollectedNumber)
                {
                    case "31":
                        Control = new FormThreeSectionOneUnits(connectionString);
                        break;
                    case "32":
                        Control = new FormThreeSectionTwoUnits(connectionString);
                        break;
                    case "33":
                        Control = new FormThreeSectionTwoSupplySources(connectionString);
                        break;
                    case "51":
                        Control = new FormFiveUnitsControl(connectionString);
                        break;
                    case "52":
                        Control = new FormFiveMaterialsControl(connectionString);
                        break;
                    case "53":
                        Control = new FormFiveItemUsersControl(connectionString);
                        break;
                    case "61":
                        Control = new FormSixSectionTwoQuestionOneOccupancy(connectionString);
                        break;
                    case "62":
                        Control = new FormSixSectionTwoQuestionOneCost(connectionString);
                        break;
                    case "63":
                        Control = new FormSixSectionTwoRentPaymentQuestion(connectionString);
                        break;
                    case "64":
                        Control = new FormSixSectionTwoServices(connectionString);
                        break;
                    case "65":
                        Control = new FormSixSectionThreeTreatment(connectionString);
                        break;
                    case "66":
                        Control = new FormSixSectionThreeTreatmentCost(connectionString);
                        break;
                    case "67":
                        Control = new FormSixSectionThreeHospitalization(connectionString);
                        break;
                    case "68":
                        Control = new FormSixSectionThreeHospitalizationCost(connectionString);
                        break;
                    case "69":
                        Control = new FormSixSectionThreeMedicine(connectionString);
                        break;
                    case "610":
                        Control = new FormSixSectionFour(connectionString);
                        break;
                    case "611":
                        Control = new FormSixSectionFivePreSchoolEduCost(connectionString);
                        break;
                    case "612":
                        Control = new FormSixSectionFiveEducation(connectionString);
                        break;
                    case "613":
                        Control = new FormSixSectionSixPropertyPurchase(connectionString);
                        break;
                    case "614":
                        Control = new FormSixSectionSixSavingsWaste(connectionString);
                        break;
                    case "615":
                        Control = new FormSixSectionSixSavingsContribution(connectionString);
                        break;
                    case "616":
                        Control = new FormSixSectionSixLoanGetting(connectionString);
                        break;
                    case "617":
                        Control = new FormSixSectionSixLoanRepayment(connectionString);
                        break;
                    case "618":
                        Control = new FormSixSectionSixPropertyTax(connectionString);
                        break;
                    case "619":
                        Control = new FormSixSectionSixIncomeTax(connectionString);
                        break;
                    case "620":
                        Control = new FormSixSectionSixMembershipFee(connectionString);
                        break;
                    case "621":
                        Control = new FormSixSectionSixInsurance(connectionString);
                        break;
                    case "622":
                        Control = new FormSixSectionSixTechInspection(connectionString);
                        break;
                    case "623":
                        Control = new FormSixSectionSixLend(connectionString);
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
