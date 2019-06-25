using CapiSample.CommonClasses;
using CapiSample.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace CapiSample.Form5
{
    internal class FormFiveUnitsControl : BaseControl<F5ItemAnswerData>, IControl
    {
        public FormFiveUnitsControl(string connection) : base(connection) { }

        private readonly Dictionary<string, string> UnitsDir = new Dictionary<string, string>()
        {
            { "kilogram", "1" },
            { "gram", "2" },
            { "liter", "3" },
            { "pieces", "4" },
            { "milliliter", "5" },
            { "meter", "6" },
            { "sq_meter", "7" },
            { "cub_meter", "8" },
            { "pair", "9" }
        };

        public void Execute()
        {
            var file = base.CreateFile(@"Reports/FormFiveUnitsReport");
            var validItemsList = JToken.Parse(File.ReadAllText(base.ValidItemsByUnitsFileName));

            foreach (var units in UnitsDir)
            {
                CheckAnswers(file, validItemsList[units.Value], GetDataWhereAnswerIs(units.Value));
                Console.WriteLine($"Items with {units.Key} units checked.");
            }

            Console.WriteLine("Done.\n");
        }

        private void CheckAnswers(FileStream file, JToken validItemsList, IEnumerable<F5ItemAnswerData> answers)
        {
            var itemCodes = new List<string>();
            foreach (var item in validItemsList)
                itemCodes.Add((string)item);

            using (var writer = File.AppendText(file.Name))
            {
                foreach (var answer in answers)
                    if (!itemCodes.Contains(answer.ItemCode))
                        writer.WriteLine($"interview: {answer.InterviewKey}; itemCode: {answer.ItemCode}.");
            }
            file.Close();
        }

        private IEnumerable<F5ItemAnswerData> GetDataWhereAnswerIs(string answer)
        {
            return base.ExecuteQuery(string.Format(query, answer));
        }

        private readonly string query = @"select summary.summaryid as InterviewId
    ,summary.key as InterviewKey
    ,summary.questionnairetitle as QuestionnaireTitle
    ,summary.updatedate as InterviewDate
    ,summary.teamleadname as Region
    ,question_entity.stata_export_caption as QuestionCode
    ,interview.asint as UnitsAnswer
    ,(
        select s_interview.asint
        from readside.interviews as s_interview
            join readside.interviews_id as s_interview_id
                on s_interview.interviewid = s_interview_id.id
            join readside.interviewsummaries as s_summary
                on s_interview_id.interviewid = s_summary.interviewid
            join readside.questionnaire_entities as s_question_entity
                on s_interview.entityid = s_question_entity.id
        where s_summary.interviewid = summary.interviewid
            and s_question_entity.stata_export_caption like 'f5tovKod%'
            and s_question_entity.parentid = question_entity.parentid
            and s_interview.rostervector = interview.rostervector
        limit 1
) as ItemCode
from readside.interviews as interview
    join readside.questionnaire_entities as question_entity
        on interview.entityid = question_entity.id
    join readside.interviews_id as interview_id
        on interview.interviewid = interview_id.id
    join readside.interviewsummaries as summary
        on interview_id.interviewid = summary.interviewid
where question_entity.stata_export_caption like 'f5q5%'
    and interview.asint = '{0}'
order by summary.summaryid";
    }
}
