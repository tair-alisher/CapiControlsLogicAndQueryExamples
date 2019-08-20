using CapiSample.CommonClasses;
using CapiSample.Interfaces;
using System;
using System.IO;

namespace CapiSample.FormSix.SectionSeven
{
    internal class ObtainedThroughGatheringAndHuntingProductsSale : BaseControl<AnswerDataWithValidRow>, IControl
    {
        public ObtainedThroughGatheringAndHuntingProductsSale(string connection) : base(connection) { }

        public void Execute()
        {
            CheckAnswers(base.CreateFile());
            Console.WriteLine("Сумма, полученная с продажи продукции. Проверено.");
        }

        private void CheckAnswers(FileStream file)
        {
            var wrongAnswers = base.ExecuteQuery(moneyFromSaleMustBeGreaterThanZeroQuery);
            using (var writer = File.AppendText(file.Name))
            {
                foreach (var wrongAnswer in wrongAnswers)
                    base.WriteError(writer, wrongAnswer);
            }
            file.Close();
        }

        private readonly string moneyFromSaleMustBeGreaterThanZeroQuery = @"with cte_crops as (
    select
        unnest(array[
            '1'
            ,'2'
            ,'3'
            ,'4'
            ,'5'
            ,'6'
            ,'61'
        ]) as code,
        unnest(array[
            'Грибы (кг)'/*1*/
            ,'Ягоды и фрукты (кг)'/*2*/
            ,'Орехи (кг)'/*3*/
            ,'Лекарственные травы (кг)'/*4*/
            ,'Мясо диких животных и птиц (кг)'/*5*/
            ,'Рыба (кг)'/*6*/
            ,'в т.ч. сазан (кг)'/*61*/
        ]) as title
)

select s.summaryid as InterviewId
    ,'Форма 6' as Form
    ,'Раздел 7' as Section
    ,'Вопрос 11' as QuestionNumber
    ,'Что именно Вы собирали и как использовали?' as QuestionText
    ,concat(
        'Сумма, на которую была продана продукция, должна быть больше нуля (',
        (select title from cte_crops where code = i.rostervector limit 1),
        ')'
    )  as InfoMessage
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
where qe.stata_export_caption in ('f6r7q11A71', 'f6r7q11A72', 'f6r7q11A73')
    and i.asdouble > 0
    and (
        (select _i.asdouble
        from readside.interviews as _i
            join readside.questionnaire_entities as _qe
                on _i.entityid = _qe.id
            join readside.interviews_id as _id
                on _i.interviewid = _id.id
        where _id.interviewid = i_id.interviewid
            and _qe.stata_export_caption like 'f6r7q11A8_'
            and substring(_qe.stata_export_caption, length(_qe.stata_export_caption), 1) = substring(qe.stata_export_caption, length(qe.stata_export_caption), 1)
            and _qe.parentid = qe.parentid
            and _i.rostervector = i.rostervector
        limit 1
    ) > 0) is false
order by s.interviewid";
    }
}
