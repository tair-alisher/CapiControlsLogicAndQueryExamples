using CapiSample.CommonClasses;
using CapiSample.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace CapiSample.Form6
{
    internal class FormSixSectionTwoQuestionOneCost : BaseControl<F6AnswerData>, IControl
    {
        public FormSixSectionTwoQuestionOneCost(string connection) : base(connection) { }

        public void Execute()
        {
            var file = base.CreateFile(@"Reports/FormSixSectionTowQuestionOneCost");
            CheckAnswers(file, GetAnswersData());

            Console.WriteLine("Done.\n");
        }

        private void CheckAnswers(FileStream file, IEnumerable<F6AnswerData> answers)
        {
            using (var writer = File.AppendText(file.Name))
            {
                foreach (var answer in answers)
                    if (answer.Cost == null || answer.Cost == 0)
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
	,interview.asdouble as Amount,
	(
		select s_interview.asdouble
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
	) as Cost
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
order by summary.interviewid";
    }
}
