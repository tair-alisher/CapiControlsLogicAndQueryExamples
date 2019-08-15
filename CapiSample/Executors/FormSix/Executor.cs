using CapiSample.Interfaces;
using System;
using Controls = CapiSample.FormSix;

namespace CapiSample.Executors.FormSix
{
    internal class Executor : IExecutor
    {
        private readonly string _connection;

        public Executor(string connection)
        {
            _connection = connection;
        }

        public string ControlNumber { get; set; }

        public IControl Control
        {
            get
            {
                IControl control = null;
                switch (ControlNumber)
                {
                    case "1":
                        control = new Controls.SectionTwo.QuestionOneOccupancy(_connection);
                        break;
                    case "2":
                        control = new Controls.SectionTwo.QuestionOneCost(_connection);
                        break;
                    case "3":
                        control = new Controls.SectionTwo.RentPaymentQuestion(_connection);
                        break;
                    case "4":
                        control = new Controls.SectionTwo.Services(_connection);
                        break;
                    case "5":
                        control = new Controls.SectionThree.Treatment(_connection);
                        break;
                    case "6":
                        control = new Controls.SectionThree.TreatmentCost(_connection);
                        break;
                    case "7":
                        control = new Controls.SectionThree.Hospitalization(_connection);
                        break;
                    case "8":
                        control = new Controls.SectionThree.HospitalizationCost(_connection);
                        break;
                    case "9":
                        control = new Controls.SectionThree.Medicine(_connection);
                        break;
                    case "10":
                        control = new Controls.SectionFour.TransportationCost(_connection);
                        break;
                    case "11":
                        control = new Controls.SectionFive.PreSchoolEduCost(_connection);
                        break;
                    case "12":
                        control = new Controls.SectionFive.Education(_connection);
                        break;
                    case "13":
                        control = new Controls.SectionSix.PropertyPurchase(_connection);
                        break;
                    case "14":
                        control = new Controls.SectionSix.SavingsWaste(_connection);
                        break;
                    case "15":
                        control = new Controls.SectionSix.SavingsContribution(_connection);
                        break;
                    case "16":
                        control = new Controls.SectionSix.LoanGetting(_connection);
                        break;
                    case "17":
                        control = new Controls.SectionSix.LoanRepayment(_connection);
                        break;
                    case "18":
                        control = new Controls.SectionSix.PropertyTax(_connection);
                        break;
                    case "19":
                        control = new Controls.SectionSix.IncomeTax(_connection);
                        break;
                    case "20":
                        control = new Controls.SectionSix.MembershipFee(_connection);
                        break;
                    case "21":
                        control = new Controls.SectionSix.Insurance(_connection);
                        break;
                    case "22":
                        control = new Controls.SectionSix.TechInspection(_connection);
                        break;
                    case "23":
                        control = new Controls.SectionSix.Lend(_connection);
                        break;
                    case "24":
                        control = new Controls.SectionSix.AlimonyPayment(_connection);
                        break;
                    case "25":
                        control = new Controls.SectionSix.FinancialSupport(_connection);
                        break;
                    case "26":
                        control = new Controls.SectionSix.GrainPurchase(_connection);
                        break;
                    case "27":
                        control = new Controls.SectionSix.OtherExpenses(_connection);
                        break;
                    case "28":
                        control = new Controls.SectionSix.OtherTaxes(_connection);
                        break;
                    case "29":
                        control = new Controls.SectionSeven.PlotCosts(_connection);
                        break;
                    case "30":
                        control = new Controls.SectionSeven.Harvesting(_connection);
                        break;
                    case "31":
                        control = new Controls.SectionSeven.HarvestSale(_connection);
                        break;
                    case "32":
                        control = new Controls.SectionSeven.HarvestProcessing(_connection);
                        break;
                    case "33":
                        control = new Controls.SectionSeven.ProductsSale(_connection);
                        break;
                    case "34":
                        control = new Controls.SectionSeven.GatheringAndHunting(_connection);
                        break;
                    case "35":
                        control = new Controls.SectionSeven.ObtainedThroughGatheringAndHuntingProductsSale(_connection);
                        break;
                    case "36":
                        control = new Controls.SectionSeven.LivestockAtTheBeginning(_connection);
                        break;
                    case "37":
                        control = new Controls.SectionSeven.LivestockPurchase(_connection);
                        break;
                    case "38":
                        control = new Controls.SectionSeven.LivestockSale(_connection);
                        break;
                    default:
                        control = new Controls.SectionTwo.QuestionOneOccupancy(_connection);
                        break;
                }

                return control;
            }
        }

        public void ShowControlsList()
        {
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
            Console.WriteLine("24. Форма 6. Раздел 6. Выплата алиментов.");
            Console.WriteLine("25. Форма 6. Раздел 6. Оказание помощи друзьям/родственникам.");
            Console.WriteLine("26. Форма 6. Раздел 6. Расходы на покупку зерна.");
            Console.WriteLine("27. Форма 6. Раздел 6. Прочие расходы.");
            Console.WriteLine("28. Форма 6. Раздел 6. Прочие налоги.");
            Console.WriteLine("29. Форма 6. Раздел 7. Расходы на участок.");
            Console.WriteLine("30. Форма 6. Раздел 7. Сбор урожая.");
            Console.WriteLine("31. Форма 6. Раздел 7. Продажа урожая.");
            Console.WriteLine("32. Форма 6. Раздел 7. Производство продукции.");
            Console.WriteLine("33. Форма 6. Раздел 7. Продажа продукции.");
            Console.WriteLine("34. Форма 6. Раздел 7. Сбор продуктов/охота/рыбная ловля.");
            Console.WriteLine("35. Форма 6. Раздел 7. Продажа продуктов полученных посредством сбора, охоты, рыбной ловли.");
            Console.WriteLine("36. Форма 6. Раздел 7. Наличие скота на начало отчетного квартала.");
            Console.WriteLine("37. Форма 6. Раздел 7. Покупка скота.");
            Console.WriteLine("38. Форма 6. Раздел 7. Продажа скота.");
        }
    }
}
