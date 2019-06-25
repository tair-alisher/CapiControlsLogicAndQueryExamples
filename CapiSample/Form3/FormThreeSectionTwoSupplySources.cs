using CapiSample.CommonClasses;
using CapiSample.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace CapiSample.Form3
{
    internal class FormThreeSectionTwoSupplySources : BaseControl<F3ProductAnswerData>, IControl
    {
        private readonly int supplySourcesAmount = 7;

        public FormThreeSectionTwoSupplySources(string connection) : base(connection) { }

        public void Execute()
        {
            var file = base.CreateFile(@"Reports/FormThreeSectionTwoSupplySourcesReport");
            var validProductList = JArray.Parse(File.ReadAllText(base.ValidProductsBySupplySourcesFileName));

            for (int i = 1; i <= supplySourcesAmount; i++)
            {
                CheckAnswers(file, validProductList[0][i.ToString()], GetDataWithAnswer(i.ToString()));
                Console.WriteLine($"answers with {i} checked.");
            }
        }

        public void CheckAnswers(FileStream file, JToken validProductList, IEnumerable<F3ProductAnswerData> answers)
        {
            var productCodes = new List<string>();
            foreach (var product in validProductList)
                productCodes.Add((string)product);

            using (var writer = File.AppendText(file.Name))
            {
                foreach (var answer in answers)
                    if (!productCodes.Contains(answer.ProductCode))
                        writer.WriteLine($"interview: {answer.InterviewKey}; productCode: {answer.ProductCode}.");
            }
            file.Close();

        }

        private IEnumerable<F3ProductAnswerData> GetDataWithAnswer(string answer)
        {
            return base.ExecuteQuery(string.Format(query, answer));
        }

        private string query = @"select summary.summaryid as InterviewId
    ,summary.key as InterviewKey
    ,summary.questionnairetitle as QuestionnaireTitle
    ,summary.updatedate as InterviewDate
    ,summary.teamleadname as Region
    ,q_entity.stata_export_caption as QuestionCode
    ,interview.asint as ProductSupplySource
    ,interview.rostervector as ProductCode
from readside.interviews as interview
    join readside.questionnaire_entities as q_entity
        on interview.entityid = q_entity.id
    join readside.interviews_id as interview_id
        on interview.interviewid = interview_id.id
    join readside.interviewsummaries as summary
        on interview_id.interviewid = summary.interviewid
where q_entity.stata_export_caption like 'f3r2q6b%'
    and interview.asint = '{0}'
order by summary.summaryid";
    }
}
