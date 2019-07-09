using CapiSample.CommonClasses;
using CapiSample.Form6.DataObjects;
using CapiSample.Interfaces;
using System;
using System.IO;
using System.Linq;

namespace CapiSample.FormSix.SectionThree
{
    internal class Hospitalization : BaseControl<F6TreatmentAnswerData>, IControl
    {
        private const int maxAllowableNumberOfHospitalizationPerMonth = 1;

        public Hospitalization(string connection) : base(connection) { }

        public void Execute()
        {
            CheckAnswers(base.CreateFile());
            Console.WriteLine("Количество госпитализаций за прошедшие три месяца проверено.");
        }

        private void CheckAnswers(FileStream file)
        {
            var answers = base.ExecuteQuery(query);
            using (var writer = File.AppendText(file.Name))
            {
                foreach (var answer in answers)
                {
                    if (!answer.ThreeMonths.Any(hospAmount => hospAmount > 0))
                        writer.WriteLine($"interview: {answer.InterviewKey}; должно быть указано количество госпитализаций.");
                    if (answer.ThreeMonths.Any(hospAmount => hospAmount > maxAllowableNumberOfHospitalizationPerMonth))
                        writer.WriteLine($"interview: {answer.InterviewKey}; количество госпитализаций не может быть больше ${maxAllowableNumberOfHospitalizationPerMonth} раз за месяц.");
                }
            }
            file.Close();
        }

        // выборка количества госпитализаций (за прошедшие три месяца)
        // в интервью, где на вопрос, были ли кто госпитализирован за прошедшие 3 месяца, отвечено да (1)
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
        where _qe.stata_export_caption = 'f6r3q5A1'
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
        where _qe.stata_export_caption = 'f6r3q5A2'
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
        where _qe.stata_export_caption = 'f6r3q5A3'
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
where qe.stata_export_caption = 'f6r3q4'
    and interview.asint = '1'
order by summary.interviewid";
    }
}
