using CapiSample.CommonClasses;
using CapiSample.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CapiSample.Form6
{
    internal class FormSixSectionSixSavingsContribution : BaseControl<AnswerDataWithValidRow>, IControl
    {
        public FormSixSectionSixSavingsContribution(string connection) : base(connection) { }

        public void Execute()
        {
            var file = base.CreateFile($@"{base.ReportsFolderName}/{this.GetType().Name}");
            CheckTypesOfSavings(file);
            Console.WriteLine("Виды сбережений. Проверено.");

            CheckContributionAmount(file);
            Console.WriteLine("Количество вложенных сбережений. Проверено.");

            Console.WriteLine(base.SuccessMessage);
        }

        private void CheckTypesOfSavings(FileStream file)
        {
            var answers = base.ExecuteQuery(typesOfSavingsMustBeSpecifiedQuery);
            using (var writer = File.AppendText(file.Name))
            {
                foreach (var answer in answers)
                {
                    if (!answer.ValidRow)
                        writer.WriteLine($"interview: {answer.InterviewKey}; должен быть выбран вид вклада сбережений.");
                }
            }
            file.Close();
        }

        private void CheckContributionAmount(FileStream file)
        {
            var answers = base.ExecuteQuery(contributionAmontMustBeGreaterThanZeroQuery);
            using (var writer = File.AppendText(file.Name))
            {
                foreach (var answer in answers)
                {
                    if (!answer.ValidRow)
                        writer.WriteLine($"interview: {answer.InterviewKey}; должно быть указано количество вложенных сбережений.");
                }
            }
            file.Close();
        }

        private readonly string typesOfSavingsMustBeSpecifiedQuery = @"select s.summaryid as InterviewId
    ,s.key as InterviewKey
    ,s.questionnairetitle as QuestionnaireTitle
    ,s.updatedate as InterviewDate
    ,s.teamleadname as Region
    ,qe.stata_export_caption as QuestionCode
    ,(
        select array_length(_i.asintarray, 1)
        from readside.interviews as _i
            join readside.questionnaire_entities as _qe
                on _i.entityid = _qe.id
            join readside.interviews_id as _id
                on _i.interviewid = _id.id
            join readside.interviewsummaries as _s
                on _id.interviewid = i_id.interviewid
        where _qe.stata_export_caption = 'f6r6q6'
            and _s.interviewid = s.interviewid
        limit 1
    ) > 0 as ValidRow
from readside.interviews as i
    join readside.questionnaire_entities as qe
        on i.entityid = qe.id
    join readside.interviews_id as i_id
        on i.interviewid = i_id.id
    join readside.interviewsummaries as s
        on i_id.interviewid = s.interviewid
where qe.stata_export_caption = 'f6r6q5'
    and i.asint = 1
order by s.interviewid";

        private readonly string contributionAmontMustBeGreaterThanZeroQuery = @"select s.summaryid as InterviewId
    ,s.key as InterviewKey
    ,s.questionnairetitle as QuestionnaireTitle
    ,s.updatedate as InterviewDate
    ,s.teamleadname as Region
    ,qe.stata_export_caption as QuestionCode
    ,(coalesce(i.asdouble, 0)
     + coalesce(
         (select _i.asdouble
         from readside.interviews as _i
             join readside.questionnaire_entities as _qe
                 on _i.entityid = _qe.id
             join readside.interviews_id as _id
                 on _i.interviewid = _id.id
         where _qe.stata_export_caption = 'f6r6q6A3'
             and _qe.parentid = qe.parentid
             and _i.rostervector = i.rostervector
             and _id.interviewid = i_id.interviewid
         limit 1
     ), 0)
    + coalesce(
         (select _i.asdouble
         from readside.interviews as _i
            join readside.questionnaire_entities as _qe
                on _i.entityid = _qe.id
            join readside.interviews_id as _id
                on _i.interviewid = _id.id
         where _qe.stata_export_caption = 'f6r6q6A4'
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
where qe.stata_export_caption = 'f6r6q6A2'
    and (
        select array_length(_i.asintarray, 1)
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
        where _qe.stata_export_caption = 'f6r6q6'
            and _id.interviewid = i_id.interviewid
        limit 1
    ) > 0
order by s.interviewid";
    }
}
