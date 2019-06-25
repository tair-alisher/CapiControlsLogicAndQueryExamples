using CapiSample.CommonClasses;

namespace CapiSample.Form6.DataObjects
{
    internal class F6ServicesAnswerData : AnswerData
    {
        public string ServiceCode { get; set; }
        public int HasBenefit { get; set; }
        public double? BenefitPercentage { get; set; }
    }
}
