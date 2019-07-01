using CapiSample.CommonClasses;
using CapiSample.Form6.DataObjects;
using CapiSample.Interfaces;
using System;
using System.IO;

namespace CapiSample.Form6
{
    internal class FormSixSectionThreeHospitalizationCost : BaseControl<F6TreatmentCostAnswerData>, IControl
    {
        public FormSixSectionThreeHospitalizationCost(string connection) : base(connection) { }

        public void Execute()
        {
            var file = base.CreateFile($@"Reports/{this.GetType().Name}");

            CheckAnsweredQuestionsData(file);
            Console.WriteLine("Соответствие выбранных типов лечения и фактически отвеченных. Проверено.");

            CheckHospitalizationCostData(file);
            Console.WriteLine("Расходы на стационарное лечение. Проверено.");

            Console.WriteLine(base.SuccessMessage);
        }

        private void CheckAnsweredQuestionsData(FileStream file)
        {
            var answers = base.ExecuteQuery(answeredQuestionsQuery);
            using (var writer = File.AppendText(file.Name))
            {
                foreach (var answer in answers)
                {
                    if (answer.NumberOfQuestionsToAnswer != answer.NumberOfActuallyAnswered)
                        writer.WriteLine($"interview: {answer.InterviewKey}; количество выбранных типов лечения и фактически отвеченных не совпадает.");
                }
            }
            file.Close();
        }

        private void CheckHospitalizationCostData(FileStream file)
        {
            var answers = base.ExecuteQuery(hospitalizationCostQuery);
            using (var writer = File.AppendText(file.Name))
            {
                foreach (var answer in answers)
                {
                    if (false == answer.IsTreatmentCostAnswered)
                        writer.WriteLine($"interview: {answer.InterviewKey}; количество расходов на стационарное лечение в один или более месяцев должно быть больше нуля.");
                }
            }
            file.Close();
        }

        // выбирает количество выбранных значений стационарного лечения
        // и количество полей для заполнения 
        // (если поля для заполнения автоматически создаются при выборе типа,
        // то данная проверка не нужна)
        private readonly string answeredQuestionsQuery = @"select summary.summaryid as InterviewId
    ,summary.key as InterviewKey
    ,summary.questionnairetitle as QuestionnaireTitle
    ,summary.updatedate as InterviewDate
    ,summary.teamleadname as Region
    ,qe.stata_export_caption as QuestionCode
    ,array_length(interview.asintarray, 1) as NumberOfQuestionsToAnswer
    ,(
        select count(distinct(_interview.rostervector))
        from readside.interviews as _interview
            join readside.questionnaire_entities as _qe
                on _interview.entityid = _qe.id
            join readside.interviews_id as _interview_id
                on _interview.interviewid = _interview_id.id
        where _qe.stata_export_caption in ('f6r3q6A2', 'f6r3q6A3', 'f6r3q6A4')
            and _interview_id.interviewid = interview_id.interviewid
    ) as NumberOfActuallyAnswered
from readside.interviews as interview
    join readside.questionnaire_entities as qe
        on interview.entityid = qe.id
    join readside.interviews_id as interview_id
        on interview.interviewid = interview_id.id
    join readside.interviewsummaries as summary
        on interview_id.interviewid = summary.interviewid
where qe.stata_export_caption = 'f6r3q6'
    and interview.asintarray is not null
order by summary.interviewid";

        // суммирует расходы на стационарное лечение за все три месяца
        // и возвращает true если их сумма больше нуля, т.е. расходы были

        // если тип лечения не выбран, то в таблице не будет и полей с кодами вопроса 'f6r3q6A_'
        // если же тип лечения выбран, то в таблице будут поля с кодами вопроса 'f6r3q6A_',
        // и значение одного или несколько из них должно быть больше нуля
        private readonly string hospitalizationCostQuery = @"select summary.summaryid as InterviewId
    ,summary.key as InterviewKey
    ,summary.questionnairetitle as QuestionnaireTitle
    ,summary.updatedate as InterviewDate
    ,summary.teamleadname as Region
    ,qe.stata_export_caption as QuestionCode
    ,interview.rostervector as TreatmentType
    ,((coalesce(interview.asdouble, 0)
    + coalesce(
        (
            select _interview.asdouble
            from readside.interviews as _interview
                join readside.questionnaire_entities as _qe
                    on _interview.entityid = _qe.id
                join readside.interviews_id as _interview_id
                    on _interview.interviewid = _interview_id.id
            where _qe.stata_export_caption = 'f6r3q6A3'
                and _qe.parentid = qe.parentid
                and _interview.rostervector = interview.rostervector
                and _interview.interviewid = interview.interviewid
            limit 1
        ), 0)
    + coalesce(
        (
            select _interview.asdouble
            from readside.interviews as _interview
                join readside.questionnaire_entities as _qe
                    on _interview.entityid = _qe.id
                join readside.interviews_id as _interview_id
                    on _interview.interviewid = _interview_id.id
            where _qe.stata_export_caption = 'f6r3q6A4'
                and _qe.parentid = qe.parentid
                and _interview.rostervector = interview.rostervector
                and _interview.interviewid = interview.interviewid
            limit 1
        ), 0)
      ) > 0) as IsTreatmentCostAnswered
from readside.interviews as interview
    join readside.questionnaire_entities as qe
        on interview.entityid = qe.id
    join readside.interviews_id as interview_id
        on interview.interviewid = interview_id.id
    join readside.interviewsummaries as summary
        on interview_id.interviewid = summary.interviewid
where qe.stata_export_caption = 'f6r3q6A2'
    and (select _interview.asint
        from readside.interviews as _interview
            join readside.questionnaire_entities as _qe
                on _interview.entityid = _qe.id
            join readside.interviews_id as _interview_id
                on _interview.interviewid = _interview_id.id
        where _qe.stata_export_caption = 'f6r3q4'
            and _interview_id.interviewid = interview_id.interviewid
        limit 1) = 1
order by summary.interviewid";
    }
}
