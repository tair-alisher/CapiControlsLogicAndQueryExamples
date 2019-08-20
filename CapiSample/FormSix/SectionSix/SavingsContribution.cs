using CapiSample.CommonClasses;
using CapiSample.Interfaces;
using System;
using System.IO;

namespace CapiSample.FormSix.SectionSix
{
    internal class SavingsContribution : BaseControl<AnswerData>, IControl
    {
        public SavingsContribution(string connection) : base(connection) { }

        public void Execute()
        {
            CheckAnswers(base.CreateFile());
            Console.WriteLine("Раздел 6. Вопрос 6. Куда Вы вложили свои сбережения6. Проверено.");
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
    s.summaryid as InterviewId
    ,'Форма 6' as Form
    ,'Раздел 6' as Section
    ,'Вопрос 6' as QuestionNumber
    ,'Куда Вы вложили свои сбережения?(в сомах)' as QuestionText
    ,'Указать, куда были вложены сбережения' as InfoMessage
    ,s.key as InterviewKey
    ,s.questionnairetitle as QuestionnaireTitle
    ,s.updatedate as InterviewDate
    ,s.teamleadname as Region
    ,qe.stata_export_caption as QuestionCode
from readside.interviews as i
    join readside.questionnaire_entities as qe
        on i.entityid = qe.id
    join readside.interviews_id as i_id
        on i.interviewid = i_id.id
    join readside.interviewsummaries as s
        on i_id.interviewid = s.interviewid
where (select _i.asint
        from readside.interviews as _i
            join readside.questionnaire_entities as _qe
                on _i.entityid = _qe.id
            join readside.interviews_id as _id
                on _i.interviewid = _id.id
        where _qe.stata_export_caption = 'f6r6q5'
            and _id.interviewid = i_id.interviewid
        limit 1
    ) = 1
    and qe.stata_export_caption = 'f6r6q6'
    and (array_length(i.asintarray, 1) > 0) is false
order by s.interviewid)

union all

(select
    s.summaryid as InterviewId
    ,'Форма 6' as Form
    ,'Раздел 6' as Section
    ,'Вопрос 6' as QuestionNumber
    ,'Куда Вы вложили свои сбережения?(в сомах)' as QuestionText
    ,'Указать, сколько сбережений было вложено' as InfoMessage
    ,s.key as InterviewKey
    ,s.questionnairetitle as QuestionnaireTitle
    ,s.updatedate as InterviewDate
    ,s.teamleadname as Region
    ,qe.stata_export_caption as QuestionCode
from readside.interviews as i
    join readside.questionnaire_entities as qe
        on i.entityid = qe.id
    join readside.interviews_id as i_id
        on i.interviewid = i_id.id
    join readside.interviewsummaries as s
        on i_id.interviewid = s.interviewid
where (
        select array_length(_i.asintarray, 1)
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
        where _qe.stata_export_caption = 'f6r6q6'
            and _id.interviewid = i_id.interviewid
        limit 1
    ) > 0
    and qe.stata_export_caption = 'f6r6q6A2'
    and ((
        coalesce(i.asdouble, 0)
        + coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _qe.stata_export_caption = 'f6r6q6A3'
                and _qe.parentid = qe.parentid
                and _i.rostervector = i.rostervector
                and _id.interviewid = i_id.interviewid
            limit 1), 0)
        + coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _qe.stata_export_caption = 'f6r6q6A4'
                and _qe.parentid = qe.parentid
                and _i.rostervector = i.rostervector
                and _id.interviewid = i_id.interviewid
            limit 1), 0)
    ) > 0) is false
order by s.interviewid)

union all

(select
    s.summaryid as InterviewId
    ,'Форма 6' as Form
    ,'Раздел 6' as Section
    ,'Вопрос 6' as QuestionNumber
    ,'Куда Вы вложили свои сбережения?(в сомах)' as QuestionText
    ,'Сумма долларов, евро и прочей валюты должна быть равна количеству купленной иностранной валюты (СКВ)(в прошлом месяце)' as InfoMessage
    ,s.key as InterviewKey
    ,s.questionnairetitle as QuestionnaireTitle
    ,s.updatedate as InterviewDate
    ,s.teamleadname as Region
    ,qe.stata_export_caption as QuestionCode
from readside.interviews as i
    join readside.questionnaire_entities as qe
        on i.entityid = qe.id
    join readside.interviews_id as i_id
        on i.interviewid = i_id.id
    join readside.interviewsummaries as s
        on i_id.interviewid = s.interviewid
where qe.stata_export_caption = 'f6r6q6A2'
    and i.rostervector = '4'
    and ((
        coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _qe.stata_export_caption = 'f6r6q6A2'
                and _id.interviewid = i_id.interviewid
                and _qe.parentid = qe.parentid
                and _i.rostervector = '41'
            limit 1), 0)
        + coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _qe.stata_export_caption = 'f6r6q6A2'
                and _id.interviewid = i_id.interviewid
                and _qe.parentid = qe.parentid
                and _i.rostervector = '42'
            limit 1), 0)
        + coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _qe.stata_export_caption = 'f6r6q6A2'
                and _id.interviewid = i_id.interviewid
                and _qe.parentid = qe.parentid
                and _i.rostervector = '43'
            limit 1), 0)
    ) = i.asdouble) is false
order by s.interviewid)

union all

(select
    s.summaryid as InterviewId
    ,'Форма 6' as Form
    ,'Раздел 6' as Section
    ,'Вопрос 6' as QuestionNumber
    ,'Куда Вы вложили свои сбережения?(в сомах)' as QuestionText
    ,'Сумма долларов, евро и прочей валюты должна быть равна количеству купленной иностранной валюты (СКВ)(два месяца назад)' as InfoMessage
    ,s.key as InterviewKey
    ,s.questionnairetitle as QuestionnaireTitle
    ,s.updatedate as InterviewDate
    ,s.teamleadname as Region
    ,qe.stata_export_caption as QuestionCode
from readside.interviews as i
    join readside.questionnaire_entities as qe
        on i.entityid = qe.id
    join readside.interviews_id as i_id
        on i.interviewid = i_id.id
    join readside.interviewsummaries as s
        on i_id.interviewid = s.interviewid
where qe.stata_export_caption = 'f6r6q6A3'
    and i.rostervector = '4'
    and ((
        coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _qe.stata_export_caption = 'f6r6q6A3'
                and _id.interviewid = i_id.interviewid
                and _qe.parentid = qe.parentid
                and _i.rostervector = '41'
            limit 1), 0)
        + coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _qe.stata_export_caption = 'f6r6q6A3'
                and _id.interviewid = i_id.interviewid
                and _qe.parentid = qe.parentid
                and _i.rostervector = '42'
            limit 1), 0)
        + coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _qe.stata_export_caption = 'f6r6q6A3'
                and _id.interviewid = i_id.interviewid
                and _qe.parentid = qe.parentid
                and _i.rostervector = '43'
            limit 1), 0)
    ) = i.asdouble) is false
order by s.interviewid)

union all

(select
    s.summaryid as InterviewId
    ,'Форма 6' as Form
    ,'Раздел 6' as Section
    ,'Вопрос 6' as QuestionNumber
    ,'Куда Вы вложили свои сбережения?(в сомах)' as QuestionText
    ,'Сумма долларов, евро и прочей валюты должна быть равна количеству купленной иностранной валюты (СКВ)(три месяца назад)' as InfoMessage
    ,s.key as InterviewKey
    ,s.questionnairetitle as QuestionnaireTitle
    ,s.updatedate as InterviewDate
    ,s.teamleadname as Region
    ,qe.stata_export_caption as QuestionCode
from readside.interviews as i
    join readside.questionnaire_entities as qe
        on i.entityid = qe.id
    join readside.interviews_id as i_id
        on i.interviewid = i_id.id
    join readside.interviewsummaries as s
        on i_id.interviewid = s.interviewid
where qe.stata_export_caption = 'f6r6q6A4'
    and i.rostervector = '4'
    and ((
        coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _qe.stata_export_caption = 'f6r6q6A4'
                and _id.interviewid = i_id.interviewid
                and _qe.parentid = qe.parentid
                and _i.rostervector = '41'
            limit 1), 0)
        + coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _qe.stata_export_caption = 'f6r6q6A4'
                and _id.interviewid = i_id.interviewid
                and _qe.parentid = qe.parentid
                and _i.rostervector = '42'
            limit 1), 0)
        + coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _qe.stata_export_caption = 'f6r6q6A4'
                and _id.interviewid = i_id.interviewid
                and _qe.parentid = qe.parentid
                and _i.rostervector = '43'
            limit 1), 0)
    ) = i.asdouble) is false
order by s.interviewid)";
    }
}
