using CapiSample.CommonClasses;
using CapiSample.Interfaces;
using System;
using System.IO;

namespace CapiSample.Form6
{
    internal class FormSixSectionSixPropertyTax : BaseControl<AnswerDataWithValidRow>, IControl
    {
        public FormSixSectionSixPropertyTax(string connection) : base(connection) { }

        public void Execute()
        {
            CheckAnswers(base.CreateFile());
            Console.WriteLine("Уплата налога на недвижимость. Проверено.");
        }

        private void CheckAnswers(FileStream file)
        {
            var answers = base.ExecuteQuery(taxAmountMustBeGreaterThanZeroQuery);
            using (var writer = File.AppendText(file.Name))
            {
                foreach (var answer in answers)
                {
                    if (!answer.ValidRow)
                        writer.WriteLine($"interview: {answer.InterviewKey}; сумма, выплаченная на уплату налога на недвижимость, должна быть больше нуля.");
                }
            }
            file.Close();
        }

        private readonly string taxAmountMustBeGreaterThanZeroQuery = @"select s.summaryid as InterviewId
    ,s.key as InterviewKey
    ,s.questionnairetitle as QuestionnaireTitle
    ,s.updatedate as InterviewDate
    ,s.teamleadname as Region
    ,qe.stata_export_caption as QuestionCode
    ,(coalesce(
        (
            select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _qe.stata_export_caption = 'f6r6q15A1'
                and _id.interviewid = i_id.interviewid
            limit 1
        ), 0)
     + coalesce(
        (
            select _i.asdouble
                from readside.interviews as _i
                    join readside.questionnaire_entities as _qe
                         on _i.entityid = _qe.id
                    join readside.interviews_id as _id
                         on _i.interviewid = _id.id
            where _qe.stata_export_caption = 'f6r6q15A2'
                and _id.interviewid = i_id.interviewid
            limit 1
        ), 0)
    + coalesce(
        (
            select _i.asdouble
                from readside.interviews as _i
                    join readside.questionnaire_entities as _qe
                        on _i.entityid = _qe.id
                    join readside.interviews_id as _id
                        on _i.interviewid = _id.id
            where _qe.stata_export_caption = 'f6r6q15A3'
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
where qe.stata_export_caption = 'f6r6q14'
    and i.asint = 1
order by s.interviewid";
    }
}
