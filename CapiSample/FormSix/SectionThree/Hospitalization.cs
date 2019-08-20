using CapiSample.CommonClasses;
using CapiSample.Interfaces;
using System;
using System.IO;

namespace CapiSample.FormSix.SectionThree
{
    internal class Hospitalization : BaseControl<AnswerData>, IControl
    {
        public Hospitalization(string connection) : base(connection) { }

        public void Execute()
        {
            CheckAnswers(base.CreateFile());
            Console.WriteLine("Раздел 3. Вопрос 5. Сколько раз члены Вашего домохозяйства были госпитализированы в течение последних трех месяцев. Проверено.");
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
    ,'Раздел 3' as Section
    ,'Вопрос 5' as QuestionNumber
    ,'Сколько раз члены Вашего домохозяйства были госпитализированы в течение последних трех месяцев?' as QuestionText
    ,'Количество госпитализаций за последние три месяца должно быть больше нуля' as InfoMessage
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
where qe.stata_export_caption = 'f6r3q5A1'
    and (
        select _i.asint
        from readside.interviews as _i
            join readside.questionnaire_entities as _qe
                on _i.entityid = _qe.id
            join readside.interviews_id as _id
                on _i.interviewid = _id.id
        where _qe.stata_export_caption = 'f6r3q4'
            and _id.interviewid = interview_id.interviewid
        limit 1
    ) = 1
    and ((
        coalesce(interview.asdouble, 0)
        + coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _qe.stata_export_caption = 'f6r3q5A2'
                and _id.interviewid = interview_id.interviewid), 0)
        + coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _qe.stata_export_caption = 'f6r3q5A3'
                and _id.interviewid = interview_id.interviewid), 0)
    ) > 0) is false
order by summary.interviewid)

union all

(select
    summary.summaryid as InterviewId
    ,'Форма 6' as Form
    ,'Раздел 3' as Section
    ,'Вопрос 5' as QuestionNumber
    ,'Сколько раз члены Вашего домохозяйства были госпитализированы в течение последних трех месяцев?' as QuestionText
    ,'Количество госпитализаций не должно превышать одного раза за месяц' as InfoMessage
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
where qe.stata_export_caption = 'f6r3q5A1'
    and (
        select _i.asint
        from readside.interviews as _i
            join readside.questionnaire_entities as _qe
                on _i.entityid = _qe.id
            join readside.interviews_id as _id
                on _i.interviewid = _id.id
        where _qe.stata_export_caption = 'f6r3q4'
            and _id.interviewid = interview_id.interviewid
        limit 1
    ) = 1
    and (
        coalesce(interview.asdouble, 0) <= 1
        and coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _qe.stata_export_caption = 'f6r3q5A2'
                and _id.interviewid = interview_id.interviewid), 0) <= 1
        and coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _qe.stata_export_caption = 'f6r3q5A3'
                and _id.interviewid = interview_id.interviewid), 0) <= 1
    ) is false
order by summary.interviewid)";
    }
}
