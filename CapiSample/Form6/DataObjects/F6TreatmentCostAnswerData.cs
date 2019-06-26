using CapiSample.CommonClasses;

namespace CapiSample.Form6.DataObjects
{
    internal class F6TreatmentCostAnswerData : AnswerData
    {
        public int TreatmentType { get; set; }
        public bool IsTreatmentCostAnswered { get; set; }
        public int NumberOfQuestionsToAnswer { get; set; }
        public int NumberOfActuallyAnswered { get; set; }
    }
}
