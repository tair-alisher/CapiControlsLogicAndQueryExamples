﻿using CapiSample.CommonClasses;
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
            Console.WriteLine("Сумма, полученная с продажи продкуции. Проверено.");
        }

        private void CheckAnswers(FileStream file)
        {
            var answers = base.ExecuteQuery(moneyFromSaleMustBeGreaterThanZeroIfWasSaleQuery);
            using (var writer = File.AppendText(file.Name))
            {
                foreach (var answer in answers)
                {
                    if (!answer.ValidRow)
                        writer.WriteLine($"interview: {answer.InterviewKey}; если была продажа продукции, должна быть указана сумма продажи.");
                }
            }
            file.Close();
        }

        private readonly string moneyFromSaleMustBeGreaterThanZeroIfWasSaleQuery = @"select s.summaryid as InterviewId
    ,s.key as InterviewKey
    ,s.questionnairetitle as QuestionnaireTitle
    ,s.updatedate as InterviewDate
    ,s.teamleadname as Region
    ,qe.stata_export_caption as QuestionCode
    ,(
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
    ) > 0) as ValidRow
from readside.interviews as i
    join readside.questionnaire_entities as qe
        on i.entityid = qe.id
    join readside.interviews_id as i_id
        on i.interviewid = i_id.id
    join readside.interviewsummaries as s
        on i_id.interviewid = s.interviewid
where qe.stata_export_caption in ('f6r7q71A71', 'f6r7q71A72', 'f6r7q71A73', 'f6r7q72A71', 'f6r7q72A72', 'f6r7q72A73')
    and i.asdouble > 0
order by s.interviewid";
    }
}