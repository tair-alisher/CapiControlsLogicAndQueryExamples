using CapiSample.CommonClasses;
using CapiSample.Form3.DataObjects;
using CapiSample.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace CapiSample.Form3
{
    internal class FormThreeSectionOneUnits : BaseControl<F3ProductAnswerData>, IControl
    {
        public FormThreeSectionOneUnits(string connection) : base(connection) { }

        public void Execute()
        {
            var file = base.CreateFile($@"{base.ReportsFolderName}/{this.GetType().Name}");

            var validProductList = JArray.Parse(File.ReadAllText(base.ValidProductsFileName));

            CheckAnswers(file, validProductList[0]["кг"], base.ExecuteQuery(interviewDataWhereAnswerIsOneOrTwoQuery));
            Console.WriteLine("Ответы с единицами измерения килограмы или граммы проверены.");

            CheckAnswers(file, validProductList[0]["л"], base.ExecuteQuery(interviewDataWhereAnswerIsThreeOrFiveQuery));
            Console.WriteLine("Ответы с единицами измерения литры или миллилитры проверены.");

            CheckAnswers(file, validProductList[0]["шт"], base.ExecuteQuery(interviewDataWhereAnswerIsFourQuery));
            Console.WriteLine("Ответы с единицами измерения штуки проверены.");

            Console.WriteLine(base.SuccessMessage);
        }

        private void CheckAnswers(FileStream file, JToken validProductsList, IEnumerable<F3ProductAnswerData> answers)
        {
            var productCodes = new List<string>();
            foreach (var product in validProductsList)
                productCodes.Add((string)product["code"]);

            using (var writer = File.AppendText(file.Name))
            {
                foreach (var answer in answers)
                    if (!productCodes.Contains(answer.ProductCode))
                        writer.WriteLine($"interview: {answer.InterviewKey}; productCode: {answer.ProductCode}.");
            }
            file.Close();
        }

        private readonly string interviewDataWhereAnswerIsOneOrTwoQuery = @"select summary.summaryid as InterviewId
    ,summary.key as InterviewKey
    ,summary.questionnairetitle as QuestionnaireTitle
    ,summary.updatedate as InterviewDate
    ,summary.teamleadname as Region
    ,q_entity.stata_export_caption as QuestionCode
    ,interview.asint as UnitsAnswer
    ,(
        select sub_interview.asint
        from readside.interviews sub_interview
        join readside.questionnaire_entities sub_q_entity
            on sub_interview.entityid = sub_q_entity.id
        where sub_q_entity.stata_export_caption = 'tovKod'
            and sub_interview.rostervector = interview.rostervector
            and sub_interview.interviewid = interview.interviewid limit 1
    ) as ProductCode
from readside.interviews as interview
    join readside.questionnaire_entities as q_entity
        on interview.entityid = q_entity.id
    join readside.interviews_id as interview_id
        on interview.interviewid = interview_id.id
    join readside.interviewsummaries as summary
        on interview_id.interviewid = summary.interviewid
where q_entity.stata_export_caption = 'f3r1q6' 
    and (interview.asint = '1' or interview.asint = '2')
order by summary.summaryid";

        private readonly string interviewDataWhereAnswerIsThreeOrFiveQuery = @"select summary.summaryid as InterviewId
    ,summary.key as InterviewKey
    ,summary.questionnairetitle as QuestionnaireTitle
    ,summary.updatedate as InterviewDate
    ,summary.teamleadname as Region
    ,q_entity.stata_export_caption as QuestionCode
    ,interview.asint as UnitsAnswer
    ,(
        select sub_interview.asint
        from readside.interviews sub_interview
        join readside.questionnaire_entities sub_q_entity
            on sub_interview.entityid = sub_q_entity.id
        where sub_q_entity.stata_export_caption = 'tovKod'
            and sub_interview.rostervector = interview.rostervector
            and sub_interview.interviewid = interview.interviewid limit 1
    ) as ProductCode
from readside.interviews as interview
join readside.questionnaire_entities as q_entity
    on interview.entityid = q_entity.id
join readside.interviews_id as interview_id
    on interview.interviewid = interview_id.id
join readside.interviewsummaries as summary
    on interview_id.interviewid = summary.interviewid
where q_entity.stata_export_caption = 'f3r1q6' 
    and (interview.asint = '3' or interview.asint = '5')
order by summary.summaryid";

        private readonly string interviewDataWhereAnswerIsFourQuery = @"select summary.summaryid as InterviewId
    ,summary.key as InterviewKey
    ,summary.questionnairetitle as QuestionnaireTitle
    ,summary.updatedate as InterviewDate
    ,summary.teamleadname as Region
    ,q_entity.stata_export_caption as QuestionCode
    ,interview.asint as UnitsAnswer
    ,(
        select sub_interview.asint
        from readside.interviews sub_interview
        join readside.questionnaire_entities sub_q_entity
            on sub_interview.entityid = sub_q_entity.id
        where sub_q_entity.stata_export_caption = 'tovKod'
            and sub_interview.rostervector = interview.rostervector
            and sub_interview.interviewid = interview.interviewid limit 1
    ) as ProductCode
from readside.interviews as interview
join readside.questionnaire_entities as q_entity
    on interview.entityid = q_entity.id
join readside.interviews_id as interview_id
    on interview.interviewid = interview_id.id
join readside.interviewsummaries as summary
    on interview_id.interviewid = summary.interviewid
where q_entity.stata_export_caption = 'f3r1q6' 
    and interview.asint = '4'
order by summary.summaryid";
    }
}
