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
        // Products
        protected string ValidProductsFileName = @"Catalogs/productsByUnits.json";
        protected string ValidProductsBySupplySourcesFileName = @"Catalogs/productsBySupplySources.json";
        // Items
        protected string ValidItemsByUnitsFileName = @"Catalogs/ItemsByUnits.json";
        protected string ValidItemsByMaterialsFileName = @"Catalogs/ItemsByMaterials.json";
        protected string ValidItemsByUsersFileName = @"Catalogs/ItemsByUsers.json";

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

        protected virtual IEnumerable<T> ExecuteQuery(string query)
        {
            IEnumerable<T> answerData = Enumerable.Empty<T>();
            using (var connection = new NpgsqlConnection(Connection))
            {
                answerData = connection.Query<T>(query);
            }

            return answerData;
        }
    }
}
