using CapiSample.CommonClasses;
using CapiSample.Interfaces;
using System;
using System.IO;

namespace CapiSample.FormSix.SectionSeven
{
    internal class GatheringAndHunting : BaseControl<AnswerDataWithValidRow>, IControl
    {
        public GatheringAndHunting(string connection) : base(connection) { }

        public void Execute()
        {
            CheckAnswers(base.CreateFile());
            Console.WriteLine("Сбор продуктов/охота/рыбная ловля. Проверено.");
        }

        private void CheckAnswers(FileStream file)
        {
            var wrongAnswers = base.ExecuteQuery(collectedOrSoldAmountMustBeGreaterThanZeroQuery);
            using (var writer = File.AppendText(file.Name))
            {
                foreach (var wrongAnswer in wrongAnswers)
                    base.WriteError(writer, wrongAnswer);
            }
            file.Close();
        }

        private readonly string collectedOrSoldAmountMustBeGreaterThanZeroQuery = @"select
    s.summaryid as InterviewId
    ,'Форма 6' as Form
    ,'Раздел 7' as Section
    ,'Вопрос 11' as QuestionNumber
    ,'Что именно Вы собирали и как использовали?' as QuestionText
    ,'Количество проданных/собранных продуктов за последние три месяца должно быть больше нуля' as InfoMessage
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
where qe.stata_export_caption = 'f6r7q11A3'
    and (
        select _i.asint
        from readside.interviews as _i
            join readside.questionnaire_entities as _qe
                on _i.entityid = _qe.id
            join readside.interviews_id as _id
                on _i.interviewid = _id.id
        where _qe.stata_export_caption = 'f6r7q10'
            and _id.interviewid = i_id.interviewid
        limit 1
    ) = 1
	and (
	(coalesce(i.asdouble, 0) > 0) -- сколько собрано ягод, орехов... за три месяца
     or ((coalesce( -- сколько продано за три месяца
        (
            select _i.asdouble
                from readside.interviews as _i
                    join readside.questionnaire_entities as _qe
                         on _i.entityid = _qe.id
                    join readside.interviews_id as _id
                         on _i.interviewid = _id.id
            where _qe.stata_export_caption = 'f6r7q11A71'
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
            where _qe.stata_export_caption = 'f6r7q11A72'
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
            where _qe.stata_export_caption = 'f6r7q11A73'
                and _id.interviewid = i_id.interviewid
                and _i.rostervector = i.rostervector
                and _qe.parentid = qe.parentid
            limit 1
        ), 0)) > 0)) is false
order by s.interviewid";
    }
}
