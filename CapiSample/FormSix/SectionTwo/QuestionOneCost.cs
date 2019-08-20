using CapiSample.CommonClasses;
using CapiSample.Interfaces;
using System;
using System.IO;

namespace CapiSample.FormSix.SectionTwo
{
    internal class QuestionOneCost : BaseControl<AnswerData>, IControl
    {
        public QuestionOneCost(string connection) : base(connection) { }

        public void Execute()
        {
            CheckAnswers(base.CreateFile());
            Console.WriteLine("Стоимость купленного топлива. Проверено.");
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
    ,'Вопрос 1.1' as QuestionNumber
    ,'Какие виды топлива Вы приобрели и как израсходовали?' as QuestionText
    ,'Сумма, уплаченная за топливо, должна быть больше нуля' as InfoMessage
    ,summary.key as InterviewKey
    ,summary.questionnairetitle as QuestionnaireTitle
    ,summary.updatedate as InterviewDate
    ,summary.teamleadname as Region
    ,qe.stata_export_caption as QuestionCode
    ,interview.asdouble as Amount
from readside.interviews as interview
    join readside.questionnaire_entities as qe
        on interview.entityid = qe.id
    join readside.interviews_id as interview_id
        on interview.interviewid = interview_id.id
    join readside.interviewsummaries as summary
        on interview_id.interviewid = summary.interviewid
where qe.stata_export_caption like 'f6r2q11A_3'
    and interview.asdouble is not null
    and interview.asdouble != 0
    and (
        (select s_interview.asdouble
        from readside.interviews as s_interview
            join readside.questionnaire_entities as s_qe
                on s_interview.entityid = s_qe.id
            join readside.interviews_id as s_interview_id
                on s_interview.interviewid = s_interview_id.id
            join readside.interviewsummaries as s_summary
                on s_interview_id.interviewid = s_summary.interviewid
        where s_summary.interviewid = summary.interviewid
            and s_qe.stata_export_caption like 'f6r2q11A_4'
            and substring(s_qe.stata_export_caption, length(s_qe.stata_export_caption) - 1, 1) = substring(qe.stata_export_caption, length(qe.stata_export_caption) - 1, 1)
            and s_qe.parentid = qe.parentid
            and s_interview.rostervector = interview.rostervector
        order by summary.interviewid
        limit 1
    ) > 0) is false
order by summary.interviewid";
    }
}
