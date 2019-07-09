using CapiSample.CommonClasses;
using CapiSample.Interfaces;
using System;
using System.IO;

namespace CapiSample.FormSix.SectionSix
{
    internal class PropertyPurchase : BaseControl<AnswerDataWithValidRow>, IControl
    {
        public PropertyPurchase(string connection) : base(connection) { }

        public void Execute()
        {
            CheckAnswers(base.CreateFile());
            Console.WriteLine("Покупка недвижимости. Проверено.");
        }

        private void CheckAnswers(FileStream file)
        {
            var answers = base.ExecuteQuery(purchaseCostMustBeGreaterThanZeroQuery);
            using (var writer = File.AppendText(file.Name))
            {
                foreach (var answer in answers)
                {
                    if (!answer.ValidRow)
                        writer.WriteLine($"interview: {answer.InterviewKey}; должны быть указаны расходы при покупке недвижимости за один или более месяцев.");
                }
            }
            file.Close();
        }

        private readonly string purchaseCostMustBeGreaterThanZeroQuery = @"select s.summaryid as InterviewId
    ,s.key as InterviewKey
    ,s.questionnairetitle as QuestionnaireTitle
    ,s.updatedate as InterviewDate
    ,s.teamleadname as Region
    ,qe.stata_export_caption as QuestionCode
    ,(coalesce(i.asdouble, 0)
     + coalesce(
         (select _i.asdouble
         from readside.interviews as _i
             join readside.questionnaire_entities as _qe
                 on _i.entityid = _qe.id
             join readside.interviews_id as _id
                 on _i.interviewid = _id.id
         where _qe.stata_export_caption = 'f6r6q2A3'
             and _qe.parentid = qe.parentid
             and _i.rostervector = i.rostervector
             and _id.interviewid = i_id.interviewid
         limit 1
     ), 0)
    + coalesce(
         (select _i.asdouble
         from readside.interviews as _i
            join readside.questionnaire_entities as _qe
                on _i.entityid = _qe.id
            join readside.interviews_id as _id
                on _i.interviewid = _id.id
         where _qe.stata_export_caption = 'f6r6q2A4'
            and _qe.parentid = qe.parentid
            and _i.rostervector = i.rostervector
            and _id.interviewid = i_id.interviewid
         limit 1
    ), 0) > 0) as ValidRow
from readside.interviews as i
    join readside.questionnaire_entities as qe
        on i.entityid = qe.id
    join readside.interviews_id as i_id
        on i.interviewid = i_id.id
    join readside.interviewsummaries as s
        on i_id.interviewid = s.interviewid
where qe.stata_export_caption = 'f6r6q2A2'
    and (
        select _i.asint
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
        where _qe.stata_export_caption = 'f6r6q1'
            and _id.interviewid = i_id.interviewid
        limit 1
    ) = 1
order by s.interviewid";
    }
}
