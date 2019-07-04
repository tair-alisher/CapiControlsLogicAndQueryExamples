using CapiSample.CommonClasses;
using CapiSample.Form6.DataObjects;
using CapiSample.Interfaces;
using System;
using System.IO;
using System.Linq;

namespace CapiSample.Form6
{
    internal class FormSixSectionThreeTreatment : BaseControl<F6TreatmentAnswerData>, IControl
    {
        private const int maxAllowableNumberOfVisitsPerMonth = 5;

        public FormSixSectionThreeTreatment(string connection) : base(connection) { }

        public void Execute()
        {
            CheckAnswers(base.CreateFile());
            Console.WriteLine("Количество обращений к врачу за прошедшие три месяца проверено.");

            Console.WriteLine(base.SuccessMessage);
        }

        private void CheckAnswers(FileStream file)
        {
            using (var writer = File.AppendText(file.Name))
            {
                var answers = base.ExecuteQuery(query);
                foreach (var answer in answers)
                {
                    if (!answer.ThreeMonths.Any(visitsAmount => visitsAmount > 0))
                        writer.WriteLine($"interview: {answer.InterviewKey}; должно быть указано количество обращений к врачу.");
                    if (answer.ThreeMonths.Any(visitsAmount => visitsAmount > maxAllowableNumberOfVisitsPerMonth))
                        writer.WriteLine($"interview: {answer.InterviewKey}; количество обращений не может быть больше {maxAllowableNumberOfVisitsPerMonth} раз за месяц.");
                }
            }
            file.Close();
        }

        // выборка количества обращений за мед помощью по месяцам (за прошедшие три месяца)
        // в интервью, где на вопрос, были ли расходы на посещение поликлиник, отвечено да (1)
        private readonly string query = @"select summary.summaryid as InterviewId
    ,summary.key as InterviewKey
    ,summary.questionnairetitle as QuestionnaireTitle
    ,summary.updatedate as InterviewDate
    ,summary.teamleadname as Region
    ,qe.stata_export_caption as QuestionCode
    ,(
        select _interview.asdouble
        from readside.interviews as _interview
            join readside.questionnaire_entities as _qe
                on _interview.entityid = _qe.id
            join readside.interviews_id as _interview_id
                on _interview.interviewid = _interview_id.id
        where _qe.stata_export_caption = 'f6r3q2A1'
            and _interview_id.interviewid = interview_id.interviewid
            and _qe.parentid = qe.parentid
    ) as FirstMonth
    ,(
        select _interview.asdouble
        from readside.interviews as _interview
            join readside.questionnaire_entities as _qe
                on _interview.entityid = _qe.id
            join readside.interviews_id as _interview_id
                on _interview.interviewid = _interview_id.id
        where _qe.stata_export_caption = 'f6r3q2A2'
            and _interview_id.interviewid = interview_id.interviewid
            and _qe.parentid = qe.parentid
    ) as SecondMonth
    ,(
        select _interview.asdouble
        from readside.interviews as _interview
            join readside.questionnaire_entities as _qe
                on _interview.entityid = _qe.id
            join readside.interviews_id as _interview_id
                on _interview.interviewid = _interview_id.id
        where _qe.stata_export_caption = 'f6r3q2A3'
            and _interview_id.interviewid = interview_id.interviewid
            and _qe.parentid = qe.parentid
    ) as ThirdMonth
from readside.interviews as interview
    join readside.questionnaire_entities as qe
        on interview.entityid = qe.id
    join readside.interviews_id as interview_id
        on interview.interviewid = interview_id.id
    join readside.interviewsummaries as summary
        on interview_id.interviewid = summary.interviewid
where qe.stata_export_caption = 'f6r3q1'
    and interview.asint = '1'
order by summary.interviewid";
    }
}
