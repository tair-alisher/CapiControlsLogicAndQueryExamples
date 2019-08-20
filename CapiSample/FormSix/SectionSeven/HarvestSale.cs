using CapiSample.CommonClasses;
using CapiSample.Interfaces;
using System;
using System.IO;

namespace CapiSample.FormSix.SectionSeven
{
    internal class HarvestSale : BaseControl<AnswerDataWithValidRow>, IControl
    {
        public HarvestSale(string connection) : base(connection) { }

        public void Execute()
        {
            CheckAnswers(base.CreateFile());
            Console.WriteLine("Сумма, полученная с продажи урожая. Проверено.");
        }

        private void CheckAnswers(FileStream file)
        {
            var wrongAnswers = base.ExecuteQuery(moneyFromSaleMustBeGreaterThanZeroIfWasSaleQuery);
            using (var writer = File.AppendText(file.Name))
            {
                foreach (var wrongAnswer in wrongAnswers)
                    base.WriteError(writer, wrongAnswer);
            }
            file.Close();
        }

        private readonly string moneyFromSaleMustBeGreaterThanZeroIfWasSaleQuery = @"with cte_crops as (
    select
        unnest(array[
            '15124971'
            ,'30074944'
            ,'8084944'
            ,'11054944'
            ,'12054944'
            ,'22054944'
            ,'24064944'
            ,'20064944'
            ,'21064944'
            ,'28064944'
            ,'23064944'
            ,'11351'
            ,'11334000'
            ,'11332000'
            ,'11349100'
            ,'11341100'
            ,'11349300'
            ,'11339100'
            ,'11339300'
            ,'11333000'
            ,'11331200'
            ,'11312100'
            ,'11342000'
            ,'11343100'
            ,'11339200'
            ,'11319'
            ,'11371000'
            ,'14074944'
            ,'12513000'
            ,'12512000'
            ,'12519200'
            ,'12519100'
            ,'12519700'
            ,'11329000'
            ,'11321000'
            ,'12519900'
            ,'12410000'
            ,'12421000'
            ,'12427000'
            ,'12428000'
            ,'12424'
            ,'12423000'
            ,'12425000'
            ,'12422000'
            ,'12429500'
            ,'1211'
            ,'12535000'
            ,'1074944'
            ,'12533000'
            ,'12531000'
            ,'12534000'
            ,'12219200'
            ,'12219100'
            ,'12214000'
            ,'1192'
            ,'11910620'
            ,'11910430'
            ,'11910610'
            ,'11510100'
            ,'11510200'
            ,'11611'
            ,'3074944'
            ,'11199100'
            ,'11380'
            ,'30064944'
            ,'12074944'
            ,'9'
        ]) as code,
        unnest(array[
            'Рис (кг)'/*15124971*/
            ,':Пшеница (кг)'/*30074944*/
            ,':Кукуруза (кг)'/*8084944*/
            ,'Ячмень (кг)'/*11054944*/
            ,'Рожь и овес (кг)'/*12054944*/
            ,'Просо и гречиха (кг)'/*22054944*/
            ,'Горох(кг)'/*24064944*/
            ,'Фасоль (кг)'/*20064944*/
            ,'Бобы кормовые (кг)'/*21064944*/
            ,'Маш(кг)'/*28064944*/
            ,'Чечевица (кг)'/*23064944*/
            ,'Картофель (кг)'/*11351*/
            ,'Помидоры (кг)'/*11334000*/
            ,'Огурцы (кг)'/*11332000*/
            ,'Свекла (кг)'/*11349100*/
            ,'Морковь (кг)'/*11341100*/
            ,':Редька, редис (кг)'/*11349300*/
            ,'Тыква (кг'/*11339100*/
            ,'Патиссоны (кг)'/*11339300*/
            ,'Баклажаны (кг)'/*11333000*/
            ,'Перец (кг)'/*11331200*/
            ,'Капуста (кг)'/*11312100*/
            ,'Чеснок (кг)'/*11342000*/
            ,'Лук репчатый (кг)'/*11343100*/
            ,'Кабачки (кг'/*11339200*/
            ,'Прочие овощи (щавель, укроп, петрушка, жусай и др.) (кг)'/*11319*/
            ,'Сахарная свекла (кг)'/*11371000*/
            ,'Подсолнечник (кг)'/*14074944*/
            ,'Клубника и земляника (кг)'/*12513000*/
            ,'Малина, ежевика, тутовник(кг)'/*12512000*/
            ,'Смородина (кг)'/*12519200*/
            ,'Крыжовник (кг)'/*12519100*/
            ,'Клюква, черника, брусника (кг'/*12519700*/
            ,'Дыни (кг)'/*11329000*/
            ,'Арбузы (кг)'/*11321000*/
            ,'Облепиха (кг)'/*12519900*/
            ,'Яблоки (кг)'/*12410000*/
            ,'Груши (кг)'/*12421000*/
            ,'Слива (кг)'/*12427000*/
            ,'Терн (кг)'/*12428000*/
            ,'Вишня и черешня (кг)'/*12424*/
            ,'Абрикосы (кг)'/*12423000*/
            ,'Персики (кг)'/*12425000*/
            ,'Айва (кг)'/*12422000*/
            ,'Алыча (кг)'/*12429500*/
            ,'Виноград (кг)'/*1211*/
            ,'Орехи грецкие (кг)'/*12535000*/
            ,'Земляные орехи, арахис (кг)'/*1074944*/
            ,'Фундук (кг)'/*12533000*/
            ,'Миндаль (кг)'/*12531000*/
            ,'Фисташки (кг)'/*12534000*/
            ,'Гранат (кг)'/*12219200*/
            ,'Хурма (кг)'/*12219100*/
            ,'Инжир (кг)'/*12214000*/
            ,'Цветы (шт)'/*1192*/
            ,'Травы зеленые сенокосов и пастбищ(ц)'/*11910620*/
            ,'Кукуруза на силос (ц)'/*11910430*/
            ,'Сено (ц)'/*11910610*/
            ,'Табак (кг)'/*11510100*/
            ,'Махорка (кг)'/*11510200*/
            ,'Хлопок (кг)'/*11611*/
            ,'Семена хлопка'/*3074944*/
            ,'Сафлор (кг)'/*11199100*/
            ,'Грибы (кг)'/*11380*/
            ,'Соя (кг)'/*30064944*/
            ,'Рапс, сурепка (кг)'/*12074944*/
            ,'Другая продукция, не включенная выше'/*9*/
        ]) as title
)

select
    s.summaryid as InterviewId
    ,'Форма 6' as Form
    ,'Раздел 7' as Section
    ,'Вопрос 6' as QuestionNumber
    ,'Какая культура была собрана или продана?' as QuestionText
    ,concat(
        'Сумма, на которую был продан урожай за последние три месяца должна быть больше нуля (',
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
where qe.stata_export_caption in ('f6r7q71A71', 'f6r7q71A72', 'f6r7q71A73', 'f6r7q72A71', 'f6r7q72A72', 'f6r7q72A73')
    and i.asdouble > 0
    and (
        (select _i.asdouble
        from readside.interviews as _i
            join readside.questionnaire_entities as _qe
                on _i.entityid = _qe.id
            join readside.interviews_id as _id
                on _i.interviewid = _id.id
        where _id.interviewid = i_id.interviewid
            and _qe.stata_export_caption like 'f6r7q7_A8_'
            and substring(_qe.stata_export_caption, length(_qe.stata_export_caption), 1) = substring(qe.stata_export_caption, length(qe.stata_export_caption), 1)
            and substring(_qe.stata_export_caption, length(_qe.stata_export_caption) - 3, 1) = substring(qe.stata_export_caption, length(qe.stata_export_caption) - 3, 1)
            and _qe.parentid = qe.parentid
            and _i.rostervector = i.rostervector
        limit 1
    ) > 0) is false
order by s.interviewid";
    }
}
