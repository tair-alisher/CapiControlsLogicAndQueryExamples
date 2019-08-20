﻿using CapiSample.CommonClasses;
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

        private readonly string processedOrSoldAmountMustBeGreaterThanZeroQuery = @"with cte_products as (
    select
        unnest(array[
            '106121'
            ,'106111'
            ,'106122200'
            ,'106122910'
            ,'106214300'
            ,'104125000'
            ,'104124000'
            ,'104121000'
            ,'104126000'
            ,'103925210'
            ,'103925100'
            ,'103925220'
            ,'103925230'
            ,'103922900'
            ,'103219'
            ,'103925900'
            ,'103918190'
            ,'103913500'
            ,'103913900'
            ,'1103'
            ,'110110850'
            ,'1105'
            ,'1081'
            ,'9'
        ]) as code,
        unnest(array[
            'Пшеничная мука (кг)'/*106121*/
            ,'Рис (кг)'/*106111*/
            ,'Кукурузная мука (кг)'/*106122200*/
            ,'Ячменная крупа (толокно) (кг)'/*106122910*/
            ,'Кукурузное масло (нераф) (л)'/*106214300*/
            ,'Хлопковое масло (нераф) (л)'/*104125000*/
            ,'Подсолнечное , сафлоровое масло (нераф) (л)'/*104124000*/
            ,'Соевое масло (нераф) (л)'/*104121000*/
            ,'Рапсовое, сурепное,горчичное масло (нераф) (л)'/*104126000*/
            ,'Курага (кг)'/*103925210*/
            ,'Изюм (кг)'/*103925100*/
            ,'Чернослив (кг)'/*103925220*/
            ,'Яблоки,груши сушеные (кг)'/*103925230*/
            ,'Варенье, джем, пюре, желе, повидло (кг)'/*103922900*/
            ,'Соки фруктовые и овощные (л)'/*103219*/
            ,'Консервированные фрукты и ягоды (компот) (л)'/*103925900*/
            ,'Консервированные и соленые овощи (л)'/*103918190*/
            ,'Сушеные грибы (кг)'/*103913500*/
            ,'Сушеные овощи (кг)'/*103913900*/
            ,'Домашнее вино (виноградное, фруктово-ягодное) (л)'/*1103*/
            ,'Водка, самогон (л)'/*110110850*/
            ,'Пиво (л)'/*1105*/
            ,'Сахар (кг)'/*1081*/
            ,'Другая продукция, не включенная выше'/*9*/
        ]) as title
)

select
    s.summaryid as InterviewId
    ,'Форма 6' as Form
    ,'Раздел 7' as Section
    ,'Вопрос 9' as QuestionNumber
    ,'Какая продукция растениеводства была произведена или продана из выращенного Вами урожая?' as QuestionText
    ,concat(
        'Количество произведенной продукции за последние три месяца должно быть больше нуля (',
        (select title from cte_products where code = i.rostervector limit 1),
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
