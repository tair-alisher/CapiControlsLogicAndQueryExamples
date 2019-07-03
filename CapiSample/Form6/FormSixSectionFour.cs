using CapiSample.CommonClasses;
using CapiSample.Interfaces;
using System;
using System.IO;

namespace CapiSample.Form6
{
    internal class FormSixSectionFour : BaseControl<AnswerDataWithValidRow>, IControl
    {
        public FormSixSectionFour(string connection) : base(connection) { }

        public void Execute()
        {
            var file = base.CreateFile($@"{base.ReportsFolderName}/{this.GetType().Name}");

            CheckAnswers(file);
            Console.WriteLine("Расходы на транспорт. Проверено.");

            Console.WriteLine(base.SuccessMessage);
        }

        private void CheckAnswers(FileStream file)
        {
            var answers = base.ExecuteQuery(query);
            using (var writer = File.AppendText(file.Name))
            {
                foreach (var answer in answers)
                {
                    if (!answer.ValidRow)
                        writer.WriteLine($"interview: {answer.InterviewKey}; Расходы за городской/междугородний за один или более месяцев должны быть больше нуля.");
                }
            }
            file.Close();
        }

        // если сумма всех затрат на транспорт (общественный городской + междугородний)
        // в интервью, где в ответ на вопрос пользовались ли услугами пассажирского транспорта отмечены пункты 1 и/или 2
        // больше нуля, вернет true
        // иначе false
        // 1 - общественный городской и пригородный
        // 2 - междугородний (международный)
        // 9 - нет расходов
        private readonly string query = @"select summary.summaryid as InterviewId
    ,summary.key as InterviewKey
    ,summary.questionnairetitle as QuestionnaireTitle
    ,summary.updatedate as InterviewDate
    ,summary.teamleadname as Region
    ,qe.stata_export_caption as QuestionCode
    ,interview.asintarray
    ,((coalesce(
        (
            select _interview.asdouble
            from readside.interviews as _interview
                join readside.questionnaire_entities as _qe
                    on _interview.entityid = _qe.id
                join readside.interviews_id as _interview_id
                    on _interview.interviewid = _interview_id.id
            where _qe.stata_export_caption = 'f6r4q21A2'
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
            where _qe.stata_export_caption = 'f6r4q21A3'
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
            where _qe.stata_export_caption = 'f6r4q21A4'
                and _interview.interviewid = interview.interviewid
            limit 1
        ), 0)
      )
      + coalesce(
        (
            select _interview.asdouble
            from readside.interviews as _interview
                join readside.questionnaire_entities as _qe
                    on _interview.entityid = _qe.id
                join readside.interviews_id as _interview_id
                    on _interview.interviewid = _interview_id.id
            where _qe.stata_export_caption = 'f6r4q22A2'
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
            where _qe.stata_export_caption = 'f6r4q22A3'
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
            where _qe.stata_export_caption = 'f6r4q22A4'
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
where qe.stata_export_caption = 'f6r4q1'
    and array_length(interview.asintarray, 1) > 0
    and not (9 = any(interview.asintarray))
order by summary.interviewid";
    }
}
