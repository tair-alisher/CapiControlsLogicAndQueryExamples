using CapiSample.CommonClasses;
using CapiSample.Form6.DataObjects;
using CapiSample.Interfaces;
using System;
using System.IO;

namespace CapiSample.FormSix.SectionTwo
{
    internal class RentPaymentQuestion : BaseControl<F6AnswerData>, IControl
    {
        public RentPaymentQuestion(string connection) : base(connection) { }

        public void Execute()
        {
            CheckAnswers(base.CreateFile());
            Console.WriteLine("Раздел 2. Вопрос 11. Каков был размер оплаты за аренду жилья за последние 3 месяца. Проверено.");
        }

        private void CheckAnswers(FileStream file)
        {
            var wrongAnswers = base.ExecuteQuery(answersWithInvalidValuesQuery);
            using (var writer = File.AppendText(file.Name))
            {
                foreach (var wrongAnswer in wrongAnswers)
                    base.WriteError(writer, wrongAnswer);
            }
            file.Close();
        }

        private readonly string answersWithInvalidValuesQuery = @"select
    summary.summaryid as InterviewId
    ,'Форма 6' as Form
    ,'Раздел 2' as Section
    ,'Вопрос 11' as QuestionNumber
    ,'Каков был размер оплаты за аренду жилья за последние 3 месяца?' as QuestionText
    ,'Сумма аренды за один или более месяцев должна быть больше нуля' as InfoMessage
    ,summary.key as InterviewKey
    ,summary.questionnairetitle as QuestionnaireTitle
    ,summary.updatedate as InterviewDate
    ,summary.teamleadname as Region
    ,qe.stata_export_caption as QuestionCode
from readside.interviews as interview
    join readside.questionnaire_entities as qe
        on interview.entityid = qe.id
    join readside.interviews_id as interview_id
        on interview.interviewid = interview_id.id
    join readside.interviewsummaries as summary
        on interview_id.interviewid = summary.interviewid
where qe.stata_export_caption like 'f6r2q11A1'
    and (
        select _i.asint
        from readside.interviews as _i
            join readside.questionnaire_entities as _qe
                on _i.entityid = _qe.id
            join readside.interviews_id as _id
                on _i.interviewid = _id.id
        where _qe.stata_export_caption = 'f6r2q10'
            and _id.interviewid = interview_id.interviewid
        limit 1
    ) = 2
    and ((
        coalesce(interview.asdouble, 0)
        + coalesce(
            (select s_interview.asdouble
            from readside.interviews as s_interview
                join readside.questionnaire_entities as s_qe
                    on s_interview.entityid = s_qe.id
                join readside.interviews_id as s_interview_id
                    on s_interview.interviewid = s_interview_id.id
                join readside.interviewsummaries as s_summary
                    on s_interview_id.interviewid = s_summary.interviewid
                where s_qe.stata_export_caption = 'f6r2q11A3'
                    and s_interview_id.interviewid = summary.interviewid
                limit 1),
            0
        )
        + coalesce(
            (select s_interview.asdouble
            from readside.interviews as s_interview
                join readside.questionnaire_entities as s_qe
                    on s_interview.entityid = s_qe.id
                join readside.interviews_id as s_interview_id
                    on s_interview.interviewid = s_interview_id.id
                join readside.interviewsummaries as s_summary
                    on s_interview_id.interviewid = s_summary.interviewid
            where s_qe.stata_export_caption = 'f6r2q11A3'
                and s_interview_id.interviewid = summary.interviewid
            limit 1),
            0
        )
    ) > 0) is false
order by summary.interviewid";
    }
}
