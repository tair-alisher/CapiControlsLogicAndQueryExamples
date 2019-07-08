using CapiSample.CommonClasses;
using CapiSample.Interfaces;
using System;
using System.IO;

namespace CapiSample.FormSix.SectionFive
{
    internal class Education : BaseControl<AnswerDataWithValidRow>, IControl
    {
        public Education(string connection) : base(connection) { }

        public void Execute()
        {
            CheckAnswers(base.CreateFile());
            Console.WriteLine("Расходы на образование. Проверено.");
        }

        private void CheckAnswers(FileStream file)
        {
            var answers = base.ExecuteQuery(query);
            using (var writer = File.AppendText(file.Name))
            {
                foreach (var answer in answers)
                {
                    if (!answer.ValidRow)
                        writer.WriteLine($"interview: {answer.InterviewKey}; Расходы на школьное образование за один или более месяцев должны быть больше нуля.");
                }
            }
            file.Close();
        }

        private readonly string query = @"select summary.summaryid as InterviewId
    ,summary.key as InterviewKey
    ,summary.questionnairetitle as QuestionnaireTitle
    ,summary.updatedate as InterviewDate
    ,summary.teamleadname as Region
    ,qe.stata_export_caption as QuestionCode
    ,(coalesce(interview.asint, 0)
      + coalesce(
        (
            select _interview.asint
            from readside.interviews as _interview
                join readside.questionnaire_entities as _qe
                    on _interview.entityid = _qe.id
                join readside.interviews_id as _interview_id
                    on _interview.interviewid = _interview_id.id
            where _qe.stata_export_caption = 'f6r5q41A3'
                and _qe.parentid = qe.parentid
                and _interview.rostervector = interview.rostervector
                and _interview.interviewid = interview.interviewid
            limit 1
        ), 0)
    + coalesce(
        (
            select _interview.asint
            from readside.interviews as _interview
                join readside.questionnaire_entities as _qe
                    on _interview.entityid = _qe.id
                join readside.interviews_id as _interview_id
                    on _interview.interviewid = _interview_id.id
            where _qe.stata_export_caption = 'f6r5q41A4'
                and _qe.parentid = qe.parentid
                and _interview.rostervector = interview.rostervector
                and _interview.interviewid = interview.interviewid
            limit 1
        ), 0) > 0) as ValidRow
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
order by summary.interviewid";
    }
}
