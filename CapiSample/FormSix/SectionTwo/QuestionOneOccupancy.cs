using CapiSample.CommonClasses;
using CapiSample.Interfaces;
using System;
using System.IO;

namespace CapiSample.FormSix.SectionTwo
{
    internal class QuestionOneOccupancy : BaseControl<AnswerData>, IControl
    {
        public QuestionOneOccupancy(string connection) : base(connection) { }

        public void Execute()
        {
            CheckAnswers(base.CreateFile());
            Console.WriteLine("Раздел 2. Вопрос 1.1. Какие виды топлива приобрели/израсходовали. Проверено.");
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
    s.summaryid as InterviewId
    ,'Форма 6' as Form
    ,'Раздел 2' as Section
    ,'Вопрос 1.1' as QuestionNumber
    ,'Какие виды топлива Вы приобрели и как израсходовали?' as QuestionText
    ,'Необходимо указать количество купленного топлива, полученного в качестве подарка, заготовленного своими силами или израсходованного на отопление за один или несколько месяцев' as InfoMessage
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
where qe.stata_export_caption = 'f6r2q11A13'
    and (
        select _i.asint
        from readside.interviews as _i
            join readside.questionnaire_entities as _qe
                on _i.entityid = _qe.id
            join readside.interviews_id as _id
                on _i.interviewid = _id.id
        where _qe.stata_export_caption = 'f6r2q1'
            and _id.interviewid = i_id.interviewid
        limit 1
    ) = 1
    and (( -- где количество меньше или равно нулю
        coalesce(i.asdouble, 0)
        + coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _id.interviewid = i_id.interviewid
                and _qe.stata_export_caption like 'f6r2q11A23'
                and _qe.parentid = qe.parentid
                and _i.rostervector = i.rostervector
            limit 1),
            0
        )
        + coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _id.interviewid = i_id.interviewid
                and _qe.stata_export_caption like 'f6r2q11A33'
                and _qe.parentid = qe.parentid
                and _i.rostervector = i.rostervector
            limit 1),
            0
        )
        + coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _id.interviewid = i_id.interviewid
                and _qe.stata_export_caption like 'f6r2q11A15'
                and _qe.parentid = qe.parentid
                and _i.rostervector = i.rostervector
            limit 1),
            0
        )
        + coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _id.interviewid = i_id.interviewid
                and _qe.stata_export_caption like 'f6r2q11A25'
                and _qe.parentid = qe.parentid
                and _i.rostervector = i.rostervector
            limit 1),
            0
        )
        + coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _id.interviewid = i_id.interviewid
                and _qe.stata_export_caption like 'f6r2q11A35'
                and _qe.parentid = qe.parentid
                and _i.rostervector = i.rostervector
            limit 1),
            0
        )
        + coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _id.interviewid = i_id.interviewid
                and _qe.stata_export_caption like 'f6r2q11A16'
                and _qe.parentid = qe.parentid
                and _i.rostervector = i.rostervector
            limit 1),
            0
        )
        + coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _id.interviewid = i_id.interviewid
                and _qe.stata_export_caption like 'f6r2q11A26'
                and _qe.parentid = qe.parentid
                and _i.rostervector = i.rostervector
            limit 1),
            0
        )
        + coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _id.interviewid = i_id.interviewid
                and _qe.stata_export_caption like 'f6r2q11A36'
                and _qe.parentid = qe.parentid
                and _i.rostervector = i.rostervector
            limit 1),
            0
        )
        + coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _id.interviewid = i_id.interviewid
                and _qe.stata_export_caption like 'f6r2q11A17'
                and _qe.parentid = qe.parentid
                and _i.rostervector = i.rostervector
            limit 1),
            0
        )
        + coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _id.interviewid = i_id.interviewid
                and _qe.stata_export_caption like 'f6r2q11A27'
                and _qe.parentid = qe.parentid
                and _i.rostervector = i.rostervector
            limit 1),
            0
        )
        + coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
            where _id.interviewid = i_id.interviewid
                and _qe.stata_export_caption like 'f6r2q11A37'
                and _qe.parentid = qe.parentid
                and _i.rostervector = i.rostervector
            limit 1),
            0
        )
    ) > 0) is false
order by s.interviewid";
    }
}
