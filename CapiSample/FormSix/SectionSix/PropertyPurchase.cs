using CapiSample.CommonClasses;
using CapiSample.Interfaces;
using System;
using System.IO;

namespace CapiSample.FormSix.SectionSix
{
    internal class PropertyPurchase : BaseControl<AnswerData>, IControl
    {
        public PropertyPurchase(string connection) : base(connection) { }

        public void Execute()
        {
            CheckAnswers(base.CreateFile());
            Console.WriteLine("Раздел 6. Вопрос 2. Какие примерно денежные расходы были у Вас при покупке недвижимости. Проверено.");
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
    s.summaryid as InterviewId
    ,'Форма 6' as Form
    ,'Раздел 6' as Section
    ,'Вопрос 2' as QuestionNumber
    ,'Какие примерно денежные расходы были у Вас при покупке недвижимости?' as QuestionText
    ,'Должна быть указана сумма расхода' as InfoMessage
    ,s.key as InterviewKey
    ,s.questionnairetitle as QuestionnaireTitle
    ,s.updatedate as InterviewDate
    ,s.teamleadname as Region
    ,qe.stata_export_caption as QuestionCode
from readside.interviews as i
    join readside.questionnaire_entities as qe
        on i.entityid = qe.id
    join readside.interviews_id as i_id
        on i.interviewid = i_id.id
    join readside.interviewsummaries as s
        on i_id.interviewid = s.interviewid
where (
        select _i.asint
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                    on _i.entityid = _qe.id
                join readside.interviews_id as _id
                    on _i.interviewid = _id.id
        where _qe.stata_export_caption = 'f6r6q1'
            and _id.interviewid = i_id.interviewid
        limit 1
    ) = 1
    and qe.stata_export_caption = 'f6r6q2A2'
    and ((
        coalesce(i.asdouble, 0)
        + coalesce(
         (select _i.asdouble
         from readside.interviews as _i
             join readside.questionnaire_entities as _qe
                 on _i.entityid = _qe.id
             join readside.interviews_id as _id
                 on _i.interviewid = _id.id
         where _qe.stata_export_caption = 'f6r6q2A3'
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
         where _qe.stata_export_caption = 'f6r6q2A4'
            and _qe.parentid = qe.parentid
            and _i.rostervector = i.rostervector
            and _id.interviewid = i_id.interviewid
         limit 1
        ), 0)
    ) > 0) is false
order by s.interviewid";
    }
}
