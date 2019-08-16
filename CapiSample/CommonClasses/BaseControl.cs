using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CapiSample.CommonClasses
{
    internal abstract class BaseControl<T> where T : class
    {
        protected string ReportsFolderName = "Reports";

        protected readonly string Connection;

        public BaseControl(string connection)
        {
            Connection = connection;
        }

        protected FileStream CreateFile()
        {
            string name = $@"{ReportsFolderName}/{GetType().Name}";
            return CreateFile(name);
        }

        protected FileStream CreateFile(string name)
        {
            var file = File.Create($"{name}-{DateTime.Now.ToString("yyyy.MM.dd-HH.mm")}.txt");
            file.Close();

            return file;
        }

        protected IEnumerable<T> ExecuteQuery(string query)
        {
            var answerData = Enumerable.Empty<T>();
            using (var connection = new NpgsqlConnection(Connection))
            {
                answerData = connection.Query<T>(query);
            }

            return answerData;
        }

        protected void WriteError(StreamWriter writer, AnswerData wrongAnswer)
        {
            writer.WriteLine($"Номер интервью: {wrongAnswer.InterviewKey};");
            writer.WriteLine($"Форма: {wrongAnswer.Form}");
            writer.WriteLine($"Раздел: {wrongAnswer.Section}");
            writer.WriteLine($"Вопрос: {wrongAnswer.QuestionNumber}");
            writer.WriteLine($"Вопрос: {wrongAnswer.QuestionText}");
            writer.WriteLine($"Ошибка: {wrongAnswer.InfoMessage}");
            writer.WriteLine();
        }
    }
}
