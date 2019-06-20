using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CapiSample.CommonClasses
{
    internal abstract class BaseControl
    {
        protected string ValidProductsFileName = @"Catalogs/productsByUnits.json";
        protected string ValidProductsBySupplySourcesFileName = @"Catalogs/productsBySupplySources.json";
        protected readonly string Connection;

        public BaseControl(string connection)
        {
            Connection = connection;
        }

        protected FileStream CreateFile(string name)
        {
            var file = File.Create($"{name}-{DateTime.Now.ToString("yyyy.MM.dd-HH.mm")}.txt");
            file.Close();

            return file;
        }

        protected virtual IEnumerable<AnswerData> ExecuteQuery(string query)
        {
            IEnumerable<AnswerData> answerData = Enumerable.Empty<AnswerData>();
            using (var connection = new NpgsqlConnection(Connection))
            {
                answerData = connection.Query<AnswerData>(query);
            }

            return answerData;
        }
    }
}
