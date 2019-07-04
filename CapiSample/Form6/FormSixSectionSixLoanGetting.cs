using CapiSample.CommonClasses;
using CapiSample.Interfaces;
using System;
using System.IO;

namespace CapiSample.Form6
{
    internal class FormSixSectionSixLoanGetting : BaseControl<AnswerDataWithValidRow>, IControl
    {
        public FormSixSectionSixLoanGetting(string connection) : base(connection) { }

        public void Execute()
        {
            var file = base.CreateFile();

            CheckLoanAmount(file);
            Console.WriteLine("Сумма кредита или долга. Проверено.");

            CheckSourceOfIncome(file);
            Console.WriteLine("Источник поступления кредита или долга. Проверено.");

            CheckLoanPurpose(file);
            Console.WriteLine("Цель получения кредита или долга. Проверено");

            Console.WriteLine(base.SuccessMessage);
        }

        private void CheckLoanAmount(FileStream file)
        {
            var answers = base.ExecuteQuery(loanAmountMustBeGreaterThanZeroQuery);
            using (var writer = File.AppendText(file.Name))
            {
                foreach (var answer in answers)
                {
                    if (!answer.ValidRow)
                        writer.WriteLine($"interview: {answer.InterviewKey}; сумма кредита или долга за один или более месяцев должна быть больше нуля.");
                }
            }
            file.Close();
        }

        private void CheckSourceOfIncome(FileStream file)
        {
            var answers = base.ExecuteQuery(incomeSourceMustBeSpecifiedQuery);
            using (var writer = File.AppendText(file.Name))
            {
                foreach (var answer in answers)
                {
                    if (!answer.ValidRow)
                        writer.WriteLine($"interview: {answer.InterviewKey}; необходимо указать источник поступления долга или кредита.");
                }
            }
            file.Close();
        }

        private void CheckLoanPurpose(FileStream file)
        {
            var answers = base.ExecuteQuery(loanPurposeMustBeSpecifiedQuery);
            using (var writer = File.AppendText(file.Name))
            {
                foreach (var answer in answers)
                {
                    if (!answer.ValidRow)
                        writer.WriteLine($"interview: {answer.InterviewKey}; необходимо указать для каких целей взят долг или кредит.");
                }
            }
            file.Close();
        }

        private readonly string loanAmountMustBeGreaterThanZeroQuery = @"select s.summaryid as InterviewId
    ,s.key as InterviewKey
    ,s.questionnairetitle as QuestionnaireTitle
    ,s.updatedate as InterviewDate
    ,s.teamleadname as Region
    ,qe.stata_export_caption as QuestionCode
    ,(coalesce(
		(
			select _i.asdouble
			from readside.interviews as _i
				join readside.questionnaire_entities as _qe
					on _i.entityid = _qe.id
				join readside.interviews_id as _id
					on _i.interviewid = _id.id
			where _qe.stata_export_caption = 'f6r6q8A1'
				and _qe.parentid = qe.parentid
				and _i.rostervector = i.rostervector
				and _id.interviewid = i_id.interviewid
			limit 1
		), 0)
     + coalesce(
		(
			select _i.asdouble
				from readside.interviews as _i
					join readside.questionnaire_entities as _qe
			 			on _i.entityid = _qe.id
					join readside.interviews_id as _id
			 			on _i.interviewid = _id.id
			where _qe.stata_export_caption = 'f6r6q8A2'
				and _qe.parentid = qe.parentid
				and _i.rostervector = i.rostervector
				and _id.interviewid = i_id.interviewid
			limit 1
		), 0)
    + coalesce(
		(
			select _i.asdouble
				from readside.interviews as _i
					join readside.questionnaire_entities as _qe
						on _i.entityid = _qe.id
					join readside.interviews_id as _id
						on _i.interviewid = _id.id
			where _qe.stata_export_caption = 'f6r6q8A3'
				and _qe.parentid = qe.parentid
				and _i.rostervector = i.rostervector
				and _id.interviewid = i_id.interviewid
			limit 1
		), 0) > 0) as ValidRow
from readside.interviews as i
    join readside.questionnaire_entities as qe
        on i.entityid = qe.id
    join readside.interviews_id as i_id
        on i.interviewid = i_id.id
    join readside.interviewsummaries as s
        on i_id.interviewid = s.interviewid
where qe.stata_export_caption = 'f6r6q7'
    and i.asint = 1
order by s.interviewid";

        private readonly string incomeSourceMustBeSpecifiedQuery = @"select s.summaryid as InterviewId
    ,s.key as InterviewKey
    ,s.questionnairetitle as QuestionnaireTitle
    ,s.updatedate as InterviewDate
    ,s.teamleadname as Region
    ,qe.stata_export_caption as QuestionCode
	,((
		select _i.asint
		from readside.interviews as _i
			join readside.questionnaire_entities as _qe
				on _i.entityid = _qe.id
			join readside.interviews_id as _id
				on _i.interviewid = _id.id
		where _qe.stata_export_caption = 'f6r6q9'
			and _i.interviewid = i.interviewid
		limit 1
	) > 0) as ValidRow
from readside.interviews as i
    join readside.questionnaire_entities as qe
        on i.entityid = qe.id
    join readside.interviews_id as i_id
        on i.interviewid = i_id.id
    join readside.interviewsummaries as s
        on i_id.interviewid = s.interviewid
where qe.stata_export_caption = 'f6r6q7'
    and i.asint = 1
order by s.interviewid";

        private readonly string loanPurposeMustBeSpecifiedQuery = @"select s.summaryid as InterviewId
    ,s.key as InterviewKey
    ,s.questionnairetitle as QuestionnaireTitle
    ,s.updatedate as InterviewDate
    ,s.teamleadname as Region
    ,qe.stata_export_caption as QuestionCode
	,((
		select _i.asint
		from readside.interviews as _i
			join readside.questionnaire_entities as _qe
				on _i.entityid = _qe.id
			join readside.interviews_id as _id
				on _i.interviewid = _id.id
		where _qe.stata_export_caption = 'f6r6q10'
			and _i.interviewid = i.interviewid
		limit 1
	) > 0) as ValidRow
from readside.interviews as i
    join readside.questionnaire_entities as qe
        on i.entityid = qe.id
    join readside.interviews_id as i_id
        on i.interviewid = i_id.id
    join readside.interviewsummaries as s
        on i_id.interviewid = s.interviewid
where qe.stata_export_caption = 'f6r6q7'
    and i.asint = 1
order by s.interviewid";
    }
}
