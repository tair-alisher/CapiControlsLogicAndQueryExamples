using CapiSample.CommonClasses;
using CapiSample.Form6.DataObjects;
using CapiSample.Interfaces;
using System;
using System.IO;

namespace CapiSample.Form6
{
    internal class FormSixSectionTwoServices : BaseControl<F6ServicesAnswerData>, IControl
    {
        public FormSixSectionTwoServices(string connection) : base(connection) { }

        public void Execute()
        {
            CheckAnswers(base.CreateFile());
            Console.WriteLine("Коммунальные услуги (льготы). Проверено.");

            Console.WriteLine(base.SuccessMessage);
        }

        private void CheckAnswers(FileStream file)
        {
            var answers = base.ExecuteQuery(query);
            using (var writer = File.AppendText(file.Name))
            {
                foreach (var answer in answers)
                    if (answer.BenefitPercentage == null || answer.BenefitPercentage <= 0)
                        writer.WriteLine($"interview: {answer.InterviewKey};");
            }
            file.Close();
        }

        // выбрать данные интервью,
        // в которых на вопрос "пользуетесь ли льготами..." отвечено "да"
        private readonly string query = @"select summary.summaryid as InterviewId
    ,summary.key as InterviewKey
    ,summary.questionnairetitle as QuestionnaireTitle
    ,summary.updatedate as InterviewDate
    ,summary.teamleadname as Region
    ,qe.stata_export_caption as QuestionCode
    ,interview.rostervector as ServiceCode
    ,interview.asint as HasBenefit
    ,(
        select _interview.asdouble
        from readside.interviews as _interview
            join readside.questionnaire_entities as _qe
                on _interview.entityid = _qe.id
            join readside.interviews_id as _interview_id
                on _interview.interviewid = _interview_id.id
        where _interview_id.interviewid = interview_id.interviewid
            and _interview.rostervector = interview.rostervector
            and _qe.parentid = qe.parentid
            and _qe.stata_export_caption = 'f6r2q12A5'
        limit 1
    ) BenefitPercentage
from readside.interviews as interview
    join readside.questionnaire_entities as qe
        on interview.entityid = qe.id
    join readside.interviews_id as interview_id
        on interview.interviewid = interview_id.id
    join readside.interviewsummaries as summary
        on interview_id.interviewid = summary.interviewid
where qe.stata_export_caption = 'f6r2q12A4'
    and interview.asint = 1
order by summary.interviewid";
    }
}
