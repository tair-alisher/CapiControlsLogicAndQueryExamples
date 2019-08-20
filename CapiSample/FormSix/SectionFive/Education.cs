using CapiSample.CommonClasses;
using CapiSample.Interfaces;
using System;
using System.IO;

namespace CapiSample.FormSix.SectionFive
{
    internal class Education : BaseControl<AnswerData>, IControl
    {
        public Education(string connection) : base(connection) { }

        public void Execute()
        {
            CheckAnswers(base.CreateFile());
            Console.WriteLine("Раздел 5. Вопрос 4. Сколько вы потратили на учебу. Проверено.");
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
    ,'Раздел 5' as Section
    ,'Вопрос 4' as QuestionNumber
    ,'Сколько Вы потратили на учебу?' as QuestionText
    ,'Должна быть указана сумма расходов' as InfoMessage
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
where qe.stata_export_caption = 'f6r5q41A2'
      and (
          select _interview.asint
            from readside.interviews as _interview
                  join readside.questionnaire_entities as _qe
                      on _interview.entityid = _qe.id
                  join readside.interviews_id as _interview_id
                      on _interview.interviewid = _interview_id.id
          where _qe.stata_export_caption = 'f6r5q3'
              and _interview_id.interviewid = interview_id.interviewid
          limit 1
      ) = 1
      and (coalesce(interview.asint, 0)
      + coalesce(
        (select _interview.asint
        from readside.interviews as _interview
            join readside.questionnaire_entities as _qe
                on _interview.entityid = _qe.id
            join readside.interviews_id as _interview_id
                on _interview.interviewid = _interview_id.id
        where _qe.stata_export_caption = 'f6r5q41A3'
            and _qe.parentid = qe.parentid
            and _interview.rostervector = interview.rostervector
            and _interview.interviewid = interview.interviewid
        limit 1), 0)
    + coalesce(
        (select _interview.asint
        from readside.interviews as _interview
            join readside.questionnaire_entities as _qe
                on _interview.entityid = _qe.id
            join readside.interviews_id as _interview_id
                on _interview.interviewid = _interview_id.id
        where _qe.stata_export_caption = 'f6r5q41A4'
            and _qe.parentid = qe.parentid
            and _interview.rostervector = interview.rostervector
            and _interview.interviewid = interview.interviewid
        limit 1), 0)
    > 0) is false
order by summary.interviewid";
    }
}
