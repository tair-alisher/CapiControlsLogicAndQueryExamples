using CapiSample.CommonClasses;
using CapiSample.Interfaces;
using System;
using System.IO;

namespace CapiSample.FormSix.SectionFour
{
    internal class TransportationCost : BaseControl<AnswerData>, IControl
    {
        public TransportationCost(string connection) : base(connection) { }

        public void Execute()
        {
            CheckAnswers(base.CreateFile());
            Console.WriteLine("Раздел 4. Вопрос 2. Каким именно видом транспорта пользовалась Ваша семья, и сколько вы потратили на оплату этих услуг. Проверено.");
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

        private readonly string answersWithInvalidValuesQuery = @"(select
    summary.summaryid as InterviewId
    ,'Форма 6' as Form
    ,'Раздел 4' as Section
    ,'Вопрос 2' as QuestionNumber
    ,'Каким именно видом транспорта пользовалась Ваша семья, и сколько вы потратили на оплату этих услуг?(сомов)' as QuestionText
    ,'Должна быть указана сумма расходов на пользование транспортом(общественный городской (сельский) и пригородный транспорт)' as InfoMessage
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
where qe.stata_export_caption = 'f6r4q21A2'
    and 1 = any(
        (select _i.asintarray
        from readside.interviews as _i
            join readside.questionnaire_entities as _qe
                on _i.entityid = _qe.id
            join readside.interviews_id as _id
                on _i.interviewid = _id.id
        where _qe.stata_export_caption = 'f6r4q1'
            and _id.interviewid = interview_id.interviewid
        limit 1)::integer[])
    and ((
        coalesce(interview.asdouble, 0)
        + coalesce(
            (select _interview.asdouble
            from readside.interviews as _interview
                join readside.questionnaire_entities as _qe
                    on _interview.entityid = _qe.id
                join readside.interviews_id as _interview_id
                    on _interview.interviewid = _interview_id.id
            where _qe.stata_export_caption = 'f6r4q21A3'
                and _qe.parentid = qe.parentid
                and _interview.rostervector = interview.rostervector
                and _interview.interviewid = interview.interviewid
            limit 1), 0)
        + coalesce(
            (select _interview.asdouble
            from readside.interviews as _interview
                join readside.questionnaire_entities as _qe
                    on _interview.entityid = _qe.id
                join readside.interviews_id as _interview_id
                    on _interview.interviewid = _interview_id.id
            where _qe.stata_export_caption = 'f6r4q21A4'
               and _qe.parentid = qe.parentid
               and _interview.rostervector = interview.rostervector
               and _interview.interviewid = interview.interviewid
            limit 1), 0)
    ) > 0) is false
order by summary.interviewid)

union all

(select
    summary.summaryid as InterviewId
    ,'Форма 6' as Form
    ,'Раздел 4' as Section
    ,'Вопрос 2' as QuestionNumber
    ,'Каким именно видом транспорта пользовалась Ваша семья, и сколько вы потратили на оплату этих услуг?(сомов)' as QuestionText
    ,'Должна быть указана сумма расходов на пользование транспортом(междугородний (международный) транспорт)' as InfoMessage
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
where qe.stata_export_caption = 'f6r4q22A2'
    and 2 = any(
        (select _i.asintarray
        from readside.interviews as _i
            join readside.questionnaire_entities as _qe
                on _i.entityid = _qe.id
            join readside.interviews_id as _id
                on _i.interviewid = _id.id
        where _qe.stata_export_caption = 'f6r4q1'
            and _id.interviewid = interview_id.interviewid
        limit 1)::integer[])
    and ((
        coalesce(interview.asdouble, 0)
        + coalesce(
            (select _interview.asdouble
            from readside.interviews as _interview
                join readside.questionnaire_entities as _qe
                    on _interview.entityid = _qe.id
                join readside.interviews_id as _interview_id
                    on _interview.interviewid = _interview_id.id
            where _qe.stata_export_caption = 'f6r4q22A3'
                and _qe.parentid = qe.parentid
                and _interview.rostervector = interview.rostervector
                and _interview.interviewid = interview.interviewid
            limit 1), 0)
        + coalesce(
            (select _interview.asdouble
            from readside.interviews as _interview
                join readside.questionnaire_entities as _qe
                    on _interview.entityid = _qe.id
                join readside.interviews_id as _interview_id
                    on _interview.interviewid = _interview_id.id
            where _qe.stata_export_caption = 'f6r4q22A4'
                and _qe.parentid = qe.parentid
                and _interview.rostervector = interview.rostervector
                and _interview.interviewid = interview.interviewid
            limit 1), 0)
    ) > 0) is false
order by summary.interviewid)";
    }
}
