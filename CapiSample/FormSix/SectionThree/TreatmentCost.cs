using CapiSample.CommonClasses;
using CapiSample.Interfaces;
using System;
using System.IO;

namespace CapiSample.FormSix.SectionThree
{
    internal class TreatmentCost : BaseControl<AnswerData>, IControl
    {
        public TreatmentCost(string connection) : base(connection) { }

        public void Execute()
        {
            var file = base.CreateFile();

            CheckAnswers(base.CreateFile());
            Console.WriteLine("Раздел 3. Вопрос 3. На что были потрачены эти деньги. Проверено.");
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
    ,'Раздел 3' as Section
    ,'Вопрос 3' as QuestionNumber
    ,'На что были потрачены эти деньги?(включая стоимость подарков и наличные расходы в сомах)' as QuestionText
    ,'Должна быть указана сумма расходов на лечение за последние три месяца' as InfoMessage
    ,summary.key as InterviewKey
    ,summary.questionnairetitle as QuestionnaireTitle
    ,summary.updatedate as InterviewDate
    ,summary.teamleadname as Region
    ,qe.stata_export_caption as QuestionCode
    ,interview.rostervector as TreatmentType
from readside.interviews as interview
    join readside.questionnaire_entities as qe
        on interview.entityid = qe.id
    join readside.interviews_id as interview_id
        on interview.interviewid = interview_id.id
    join readside.interviewsummaries as summary
        on interview_id.interviewid = summary.interviewid
where qe.stata_export_caption = 'f6r3q3A2'
    and (
        select _i.asint
        from readside.interviews as _i
            join readside.questionnaire_entities as _qe
                on _i.entityid = _qe.id
            join readside.interviews_id as _id
                on _i.interviewid = _id.id
        where _qe.stata_export_caption = 'f6r3q1'
            and _id.interviewid = interview_id.interviewid
        limit 1
    ) = 1
    and ((
        coalesce(interview.asdouble, 0)
        + coalesce(
            (select _interview.asdouble
            from readside.interviews as _interview
                join readside.questionnaire_entities as _qe
                    on _interview.entityid = _qe.id
                join readside.interviews_id as _interview_id
                    on _interview.interviewid = _interview_id.id
            where _qe.stata_export_caption = 'f6r3q3A3'
                and _qe.parentid = qe.parentid
                and _interview.rostervector = interview.rostervector
                and _interview_id.interviewid = interview_id.interviewid
            limit 1), 0)
        + coalesce(
            (select _interview.asdouble
            from readside.interviews as _interview
                join readside.questionnaire_entities as _qe
                    on _interview.entityid = _qe.id
                join readside.interviews_id as _interview_id
                    on _interview.interviewid = _interview_id.id
            where _qe.stata_export_caption = 'f6r3q3A4'
                and _qe.parentid = qe.parentid
                and _interview.rostervector = interview.rostervector
                and _interview_id.interviewid = interview_id.interviewid
            limit 1), 0)
    ) > 0) is false
order by summary.interviewid";
    }
}
