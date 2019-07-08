using CapiSample.CommonClasses;
using System.Collections.Generic;

namespace CapiSample.Form6.DataObjects
{
    internal class F6TreatmentAnswerData : AnswerData
    {
        public int FirstMonth { get; set; }
        public int SecondMonth { get; set; }
        public int ThirdMonth { get; set; }

        public List<int> ThreeMonths
        {
            get
            {
                return new List<int>()
                {
                    FirstMonth,
                    SecondMonth,
                    ThirdMonth
                };
            }
        }
    }
}
