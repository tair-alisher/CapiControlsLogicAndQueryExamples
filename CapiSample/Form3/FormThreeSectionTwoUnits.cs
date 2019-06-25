using CapiSample.CommonClasses;
using CapiSample.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace CapiSample.Form3
{
    internal class FormThreeSectionTwoUnits : BaseControl<F3ProductAnswerData>, IControl
    {
        public FormThreeSectionTwoUnits(string connection) : base(connection) { }

        public void Execute()
        {
            var file = base.CreateFile(@"Reports/FormThreeSectionTwoUnitsReport");
            var validProductList = JArray.Parse(File.ReadAllText(base.ValidProductsFileName));

            CheckAnswers(file, validProductList[0]["кг"], GetDataWithAnswerOneOrTwo());
            Console.WriteLine("answers with kg or gr checked.");

            CheckAnswers(file, validProductList[0]["л"], GetDataWithAnswerThreeOrFive());
            Console.WriteLine("answers with ml or l checked.");

            CheckAnswers(file, validProductList[0]["шт"], GetDataWithAnswerFour());
            Console.WriteLine("answers with sht checked.");

            Console.WriteLine("Done.\n");
        }

        private void CheckAnswers(FileStream file, JToken validProductList, IEnumerable<F3ProductAnswerData> answers)
        {
            var productCodes = new List<string>();
            foreach (var product in validProductList)
                productCodes.Add((string)product["code"]);

            using (var writer = File.AppendText(file.Name))
            {
                foreach (var answer in answers)
                    if (!productCodes.Contains(answer.ProductCode))
                        writer.WriteLine($"interview: {answer.InterviewKey}; productCode: {answer.ProductCode}.");
            }
            file.Close();
        }

        private IEnumerable<F3ProductAnswerData> GetDataWithAnswerOneOrTwo()
        {
            string query = @"select summary.summaryid as InterviewId
    ,summary.key as InterviewKey
    ,summary.questionnairetitle as QuestionnaireTitle
    ,summary.updatedate as InterviewDate
    ,summary.teamleadname as Region
    ,q_entity.stata_export_caption as QuestionCode
    ,interview.asint as UnitsAnswer
    ,interview.rostervector as ProductCode
from readside.interviews as interview
    join readside.questionnaire_entities as q_entity
        on interview.entityid = q_entity.id
    join readside.interviews_id as interview_id
        on interview.interviewid = interview_id.id
    join readside.interviewsummaries as summary
        on interview_id.interviewid = summary.interviewid
where q_entity.stata_export_caption like 'f3r2q5b%'
    and (interview.asint = '1' or interview.asint = '2')
order by summary.summaryid";

            return base.ExecuteQuery(query);
        }

        private IEnumerable<F3ProductAnswerData> GetDataWithAnswerThreeOrFive()
        {
            string query = @"select summary.summaryid as InterviewId
    ,summary.key as InterviewKey
    ,summary.questionnairetitle as QuestionnaireTitle
    ,summary.updatedate as InterviewDate
    ,summary.teamleadname as Region
    ,q_entity.stata_export_caption as QuestionCode
    ,interview.asint as UnitsAnswer
    ,interview.rostervector as ProductCode
from readside.interviews as interview
    join readside.questionnaire_entities as q_entity
        on interview.entityid = q_entity.id
    join readside.interviews_id as interview_id
        on interview.interviewid = interview_id.id
    join readside.interviewsummaries as summary
        on interview_id.interviewid = summary.interviewid
where q_entity.stata_export_caption like 'f3r2q5b%'
    and (interview.asint = '3' or interview.asint = '5')
order by summary.summaryid";

            return base.ExecuteQuery(query);
        }

        private IEnumerable<F3ProductAnswerData> GetDataWithAnswerFour()
        {
            string query = @"select summary.summaryid as InterviewId
    ,summary.key as InterviewKey
    ,summary.questionnairetitle as QuestionnaireTitle
    ,summary.updatedate as InterviewDate
    ,summary.teamleadname as Region
    ,q_entity.stata_export_caption as QuestionCode
    ,interview.asint as UnitsAnswer
    ,interview.rostervector as ProductCode
from readside.interviews as interview
    join readside.questionnaire_entities as q_entity
        on interview.entityid = q_entity.id
    join readside.interviews_id as interview_id
        on interview.interviewid = interview_id.id
    join readside.interviewsummaries as summary
        on interview_id.interviewid = summary.interviewid
where q_entity.stata_export_caption like 'f3r2q5b%'
    and interview.asint = '4'
order by summary.summaryid";

            return base.ExecuteQuery(query);
        }
    }
}
