using CapiSample.CommonClasses;
using CapiSample.Form6.DataObjects;
using CapiSample.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace CapiSample.Form6
{
    internal class FormSixSectionTwoQuestionOneOccupancy : BaseControl<F6AnswerData>, IControl
    {
        public FormSixSectionTwoQuestionOneOccupancy(string connection) : base(connection) { }

        public void Execute()
        {
            var file = base.CreateFile(@"Reports/FormSixSectionOneOccupancy");
            this.CheckAnswers(file, GetAnswersData());

            Console.WriteLine("Done.\n");
        }

        private void CheckAnswers(FileStream file, IEnumerable<F6AnswerData> answers)
        {
            using (var writer = File.AppendText(file.Name))
            {
                foreach (var answer in answers)
                    if (int.Parse(answer.NotNullAnswersAmount) <= 0)
                        writer.WriteLine($"interview: {answer.InterviewKey};");
            }
            file.Close();
        }

        private IEnumerable<F6AnswerData> GetAnswersData()
        {
            return base.ExecuteQuery(query);
        }

        private readonly string query = @"select summary.summaryid as InterviewId
    ,summary.key as InterviewKey
    ,summary.questionnairetitle as QuestionnaireTitle
    ,summary.updatedate as InterviewDate
    ,summary.teamleadname as Region
    ,qe.stata_export_caption as QuestionCode
    ,interview.asint as WasUsed,
    (
        select count(s_interview.asdouble)
        from readside.interviews as s_interview
            join readside.questionnaire_entities as s_qe
                on s_interview.entityid = s_qe.id
            join readside.interviews_id as s_interview_id
                on s_interview.interviewid = s_interview_id.id
            join readside.interviewsummaries as s_summary
                on s_interview_id.interviewid = s_summary.interviewid
        where s_summary.interviewid = summary.interviewid
            and(s_qe.stata_export_caption like 'f6r2q11A_3'
            or s_qe.stata_export_caption like 'f6r2q11A_5'
            or s_qe.stata_export_caption like 'f6r2q11A_6'
            or s_qe.stata_export_caption like 'f6r2q11A_7')
            and (s_interview.asdouble is not null
            and s_interview.asdouble != 0)
    ) as NotNullAnswersAmount
from readside.interviews as interview
    join readside.questionnaire_entities as qe
        on interview.entityid = qe.id
    join readside.interviews_id as interview_id
        on interview.interviewid = interview_id.id
    join readside.interviewsummaries as summary
        on interview_id.interviewid = summary.interviewid
where qe.stata_export_caption = 'f6r2q1'
    and interview.asint = 1";
    }
}
