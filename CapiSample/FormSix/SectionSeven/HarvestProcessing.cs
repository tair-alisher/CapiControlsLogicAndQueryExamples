using CapiSample.CommonClasses;
using CapiSample.Interfaces;
using System;
using System.IO;

namespace CapiSample.FormSix.SectionSeven
{
    internal class HarvestProcessing : BaseControl<AnswerDataWithValidRow>, IControl
    {
        public HarvestProcessing(string connection) : base(connection) { }

        public void Execute()
        {
            CheckAnswers(base.CreateFile());
            Console.WriteLine("Производство продукции из урожая. Проверено.");
        }

        private void CheckAnswers(FileStream file)
        {
            var wrongAnswers = base.ExecuteQuery(processedOrSoldAmountMustBeGreaterThanZeroQuery);
            using (var writer = File.AppendText(file.Name))
            {
                foreach (var wrongAnswer in wrongAnswers)
                    base.WriteError(writer, wrongAnswer);
            }
            file.Close();
        }

        private readonly string processedOrSoldAmountMustBeGreaterThanZeroQuery = @"select
    s.summaryid as InterviewId
    ,'Форма 6' as Form
    ,'Раздел 7' as Section
    ,'Вопрос 9' as QuestionNumber
    ,'Какая продукция растениеводства была произведена или продана из выращенного Вами урожая?' as QuestionText
    ,'Количество произведенной продукции за последние три месяца должно быть больше нуля' as InfoMessage
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
where qe.stata_export_caption = 'f6r7q9A4'
    and (
        select _i.asint
        from readside.interviews as _i
            join readside.questionnaire_entities as _qe
                on _i.entityid = _qe.id
            join readside.interviews_id as _id
                on _i.interviewid = _id.id
        where _qe.stata_export_caption = 'f6r7q8'
            and _id.interviewid = i_id.interviewid
        limit 1
    ) = 1
    and (
    (coalesce(i.asdouble, 0) > 0) -- сколько произвели урожая за три месяца
     or ((coalesce( -- сколько продано произведенной продукции за три месяца
        (
            select _i.asdouble
                from readside.interviews as _i
                    join readside.questionnaire_entities as _qe
                         on _i.entityid = _qe.id
                    join readside.interviews_id as _id
                         on _i.interviewid = _id.id
            where _qe.stata_export_caption = 'f6r7q9A71'
                and _id.interviewid = i_id.interviewid
                and _i.rostervector = i.rostervector
                and _qe.parentid = qe.parentid
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
            where _qe.stata_export_caption = 'f6r7q9A2'
                and _id.interviewid = i_id.interviewid
                and _i.rostervector = i.rostervector
                and _qe.parentid = qe.parentid
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
            where _qe.stata_export_caption = 'f6r7q9A3'
                and _id.interviewid = i_id.interviewid
                and _i.rostervector = i.rostervector
                and _qe.parentid = qe.parentid
            limit 1
        ), 0)) > 0)) is false
order by s.interviewid";
    }
}
