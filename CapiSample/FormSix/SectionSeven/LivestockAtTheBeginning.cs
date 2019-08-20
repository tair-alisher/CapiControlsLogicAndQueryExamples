﻿using CapiSample.CommonClasses;
using CapiSample.Interfaces;
using System;
using System.IO;

namespace CapiSample.FormSix.SectionSeven
{
    internal class LivestockAtTheBeginning : BaseControl<AnswerDataWithValidRow>, IControl
    {
        public LivestockAtTheBeginning(string connection) : base(connection) { }

        public void Execute()
        {
            CheckAnswers(base.CreateFile());
            Console.WriteLine("Количество голов на начало года. Проверено.");
        }

        private void CheckAnswers(FileStream file)
        {
            var wrongAnswers = base.ExecuteQuery(livestockAmountMustBeGreaterThanZeroQuery);
            using (var writer = File.AppendText(file.Name))
            {
                foreach (var wrongAnswer in wrongAnswers)
                    base.WriteError(writer, wrongAnswer);
            }
            file.Close();
        }

        private readonly string livestockAmountMustBeGreaterThanZeroQuery = @"with cte_livestock as(
    select
        unnest(array[1,2,3,4,5,6,7,8,9,10,11,12,13,14,18,19,20,22,23,24,25]) as code,
        unnest(array[
            'Коровы'/*1*/
            ,'Нетели и телки старше года'/*2*/
            ,'Быки и волы старше года'/*3*/
            ,'Телята до года'/*4*/
            ,'Свиньи старше 9 месяцев'/*5*/
            ,'Подсвинки от 4 до 9 месяце'/*6*/
            ,'Поросята до 4 месяцев'/*7*/
            ,'Овцы старше года'/*8*/
            ,'Ягнята до года'/*9*/
            ,'Козы старше года'/*10*/
            ,'Козлята до года'/*11*/
            ,'Лошади'/*12*/
            ,'Молодняк лошадей'/*13*/
            ,'Ослы'/*14*/
            ,'Птица взрослая'/*18*/
            ,'Молодняк птицы'/*19*/
            ,'Яки'/*20*/
            ,'Кролики'/*22*/
            ,'Нутрии'/*23*/
            ,'Пчелы (семей)'/*24*/
            ,'Другие виды скота'/*25*/
            ]) as title
)

select
    s.summaryid as InterviewId
    ,'Форма 6' as Form
    ,'Раздел 7' as Section
    ,'Вопрос 13' as QuestionNumber
    ,'Какой скот, птица или другие животные есть у Вас в наличии?' as QuestionText
    ,concat(
        'Количество голов скота на начало отчетного квартала должно быть больше нуля (',
        (select title from cte_livestock where code::text = i.rostervector limit 1),
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
where qe.stata_export_caption = 'f6r7q13A1'
    and (
        select _i.asint
        from readside.interviews as _i
            join readside.questionnaire_entities as _qe
                on _i.entityid = _qe.id
            join readside.interviews_id as _id
                on _i.interviewid = _id.id
        where _id.interviewid = i_id.interviewid
            and _qe.stata_export_caption = 'f6r7q12'
        limit 1
    ) = 1
    and (i.asdouble > 0) is false
order by s.interviewid";
    }
}
