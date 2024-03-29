﻿using CapiSample.CommonClasses;

namespace CapiSample.Form6.DataObjects
{
    internal class F6AnswerData : AnswerData
    {
        public string WasUsed { get; set; }
        public string NotNullAnswersAmount { get; set; }
        public double Amount { get; set; }
        public double? Cost { get; set; }
        public int? PaymentAmount { get; set; }
    }
}
