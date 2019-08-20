using CapiSample.CommonClasses;
using CapiSample.Interfaces;
using System;
using System.IO;

namespace CapiSample.FormSix.SectionSix
{
    internal class AlimonyPayment : BaseControl<AnswerData>, IControl
    {
        public AlimonyPayment(string connection) : base(connection) { }

        public void Execute()
        {
            CheckAnswers(base.CreateFile());
            Console.WriteLine("Раздел 6. Вопрос 27. Какова сумма на уплату алиментов. Проверено.");
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
    ,'Вопрос 27' as QuestionNumber
    ,'Какова сумма на уплату алиментов?' as QuestionText
    ,'Должна быть указана сумма' as InfoMessage
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
where qe.stata_export_caption = 'f6r6q27A1'
    and (
        select _i.asint
        from readside.interviews as _i
            join readside.questionnaire_entities as _qe
                on _i.entityid = _qe.id
            join readside.interviews_id as _id
                on _i.interviewid = _id.id
        where _qe.stata_export_caption = 'f6r6q26'
            and _id.interviewid = i_id.interviewid
        limit 1
    ) = 1
    and ((
        coalesce(i.asdouble, 0)
        + coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                        on _i.entityid = _qe.id
                join readside.interviews_id as _id
                        on _i.interviewid = _id.id
            where _qe.stata_export_caption = 'f6r6q27A2'
                and _id.interviewid = i_id.interviewid
            limit 1), 0)
        + coalesce(
            (select _i.asdouble
            from readside.interviews as _i
                join readside.questionnaire_entities as _qe
                        on _i.entityid = _qe.id
                join readside.interviews_id as _id
                        on _i.interviewid = _id.id
            where _qe.stata_export_caption = 'f6r6q27A3'
                and _id.interviewid = i_id.interviewid
            limit 1), 0)
    ) > 0) is false
order by s.interviewid";
    }
}
