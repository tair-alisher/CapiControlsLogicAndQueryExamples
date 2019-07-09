using CapiSample.Form5;
using CapiSample.Interfaces;
using System;

namespace CapiSample.Executors.FormFive
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
                        control = new FormFiveUnitsControl(_connection);
                        break;
                    case "2":
                        control = new FormFiveMaterialsControl(_connection);
                        break;
                    case "3":
                        control = new FormFiveItemUsersControl(_connection);
                        break;
                }

                return control;
            }
        }

        public void ShowControlsList()
        {
            Console.WriteLine("1: Форма 5. Единицы измерения.");
            Console.WriteLine("2: Форма 5. Материал.");
            Console.WriteLine("3: Форма 5. Для кого куплено.");
        }
    }
}
