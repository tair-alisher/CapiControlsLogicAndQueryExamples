using CapiSample.Form3;
using CapiSample.Interfaces;
using System;

namespace CapiSample.Executors.FormThree
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
                        control = new FormThreeSectionOneUnits(_connection);
                        break;
                    case "2":
                        control = new FormThreeSectionTwoUnits(_connection);
                        break;
                    case "3":
                        control = new FormThreeSectionTwoSupplySources(_connection);
                        break;
                }

                return control;
            }
        }

        public void ShowControlsList()
        {
            Console.WriteLine("1: Форма 3. Раздел 1. Единицы измерения.");
            Console.WriteLine("2: Форма 3. Разедл 2. Единицы измерения.");
            Console.WriteLine("3: Форма 3. Раздел 2. Источники поступления.");
        }
    }
}
