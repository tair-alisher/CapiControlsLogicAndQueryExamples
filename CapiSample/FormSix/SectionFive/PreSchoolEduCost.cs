using CapiSample.CommonClasses;
using CapiSample.Interfaces;
using System;
using System.IO;

namespace CapiSample.FormSix.SectionFive
{
    internal class PreSchoolEduCost : BaseControl<AnswerDataWithValidRow>, IControl
    {
        public PreSchoolEduCost(string connection) : base(connection) { }

        public void Execute()
        {
            CheckAnswers(base.CreateFile());
            Console.WriteLine("Расходы на дошкольное образование. Проверено.");
        }

        private void CheckAnswers(FileStream file)
        {
            var answers = base.ExecuteQuery(query);
            using (var writer = File.AppendText(file.Name))
            {
                foreach (var answer in answers)
                {
                    if (!answer.ValidRow)
                        writer.WriteLine($"interview: {answer.InterviewKey}; Расходы на дошкольное образование за один или более месяцев должны быть больше нуля.");
                }
            }
            file.Close();
        }

        // если сумма всех затрат на дошкольное образование
        // в интервью, где в ответ на вопрос были ли расходы на дошкольное образование выбран пункт 1 (да),
        // больше нуля, вернет true
        // иначе false
        // 1 - да
        // 2 - нет
        private readonly string query = @"select summary.summaryid as InterviewId
    ,summary.key as InterviewKey
    ,summary.questionnairetitle as QuestionnaireTitle
    ,summary.updatedate as InterviewDate
    ,summary.teamleadname as Region
    ,qe.stata_export_caption as QuestionCode
    ,(coalesce(interview.asdouble, 0)
      + coalesce(
        (
            select _interview.asdouble
            from readside.interviews as _interview
                join readside.questionnaire_entities as _qe
                    on _interview.entityid = _qe.id
                join readside.interviews_id as _interview_id
                    on _interview.interviewid = _interview_id.id
            where _qe.stata_export_caption = 'f6r5c2A3'
                and _qe.parentid = qe.parentid
                and _interview.rostervector = interview.rostervector
                and _interview.interviewid = interview.interviewid
            limit 1
        ), 0)
    + coalesce(
        (
            select _interview.asdouble
            from readside.interviews as _interview
                join readside.questionnaire_entities as _qe
                    on _interview.entityid = _qe.id
                join readside.interviews_id as _interview_id
                    on _interview.interviewid = _interview_id.id
            where _qe.stata_export_caption = 'f6r5c2A4'
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
where qe.stata_export_caption = 'f6r5q2A2'
      and (
          select _interview.asint
            from readside.interviews as _interview
                  join readside.questionnaire_entities as _qe
                      on _interview.entityid = _qe.id
                  join readside.interviews_id as _interview_id
                      on _interview.interviewid = _interview_id.id
          where _qe.stata_export_caption = 'f6r5q1'
              and _interview_id.interviewid = interview_id.interviewid
          limit 1
      ) = 1
order by summary.interviewid";
    }
}
