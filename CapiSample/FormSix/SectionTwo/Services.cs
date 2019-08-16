using CapiSample.CommonClasses;
using CapiSample.Form6.DataObjects;
using CapiSample.Interfaces;
using System;
using System.IO;

namespace CapiSample.FormSix.SectionTwo
{
    internal class Services : BaseControl<F6ServicesAnswerData>, IControl
    {
        public Services(string connection) : base(connection) { }

        public void Execute()
        {
            CheckAnswers(base.CreateFile());
            Console.WriteLine("Раздел 2. Вопрос 12. Оплата за коммунальные услуги и электроэнергию. Проверено.");
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
    ,'Вопрос 12' as QuestionNumber
    ,'Оплата за коммунальные услуги и электроэнергию' as QuestionText
    ,'Необходимо указать процент льгот за услугу, при оплате которой вы пользуетесь льготами' as InfoMessage
    ,summary.key as InterviewKey
    ,summary.questionnairetitle as QuestionnaireTitle
    ,summary.updatedate as InterviewDate
    ,summary.teamleadname as Region
    ,qe.stata_export_caption as QuestionCode
    ,interview.rostervector as ServiceCode
from readside.interviews as interview
    join readside.questionnaire_entities as qe
        on interview.entityid = qe.id
    join readside.interviews_id as interview_id
        on interview.interviewid = interview_id.id
    join readside.interviewsummaries as summary
        on interview_id.interviewid = summary.interviewid
where qe.stata_export_caption = 'f6r2q12A5'
    and (
        select _i.asint
        from readside.interviews as _i
            join readside.questionnaire_entities as _qe
                on _i.entityid = _qe.id
            join readside.interviews_id as _id
                on _i.interviewid = _id.id
        where _qe.stata_export_caption = 'f6r2q12A4'
            and _id.interviewid = interview_id.interviewid
            and _i.rostervector = interview.rostervector
            and _qe.parentid = qe.parentid
        limit 1
    ) = 1
    and (interview.asdouble > 0) is false
order by summary.interviewid";
    }
}
