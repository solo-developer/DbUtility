using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace DbUtility
{
    public class StoredProcedureRequestModel
    {
        public string ParamName { get; set; }
        public object Value { get; set; }
        public bool IsCustomType { get; set; }

        public string CustomTableTypeName { get; set; }
        public string CustomTypeColumnName { get; set; }

        public object[] CustomTypeValues { get; set; }

        public Type ColumnType { get; set; } = typeof(int);
    }
    public static class StoredProcedureHelper<T> where T : DbContext, new()
    {
        public static List<ResponseModel> ExecuteStoredProcedure<ResponseModel>(string spName, List<StoredProcedureRequestModel> spParams)
        {
            List<ResponseModel> response = new List<ResponseModel>();
            string paramsCombined = string.Join(",", spParams.Select(a => a.ParamName));

            object[] paramValues = BuildSPValues(spParams);
            using (var context = new T())
            {
                response = context.Database.SqlQuery<ResponseModel>($"EXEC {spName} {paramsCombined}", paramValues).ToList();
            }
            return response;
        }

        public static ResponseModel GetAnObjectExecutingStoredProcedure<ResponseModel>(string spName, List<StoredProcedureRequestModel> spParams) where ResponseModel : new()
        {
            ResponseModel response;
            string paramsCombined = string.Join(",", spParams.Select(a => a.ParamName));

            object[] paramValues = BuildSPValues(spParams);
            using (var context = new T())
            {
                response = context.Database.SqlQuery<ResponseModel>($"EXEC {spName} {paramsCombined}", paramValues).FirstOrDefault();
            }
            return response;
        }

        public static void ExecuteStoredProcedure(string spName, List<StoredProcedureRequestModel> spParams)
        {
            string paramsCombined = string.Join(",", spParams.Select(a => a.ParamName));

            object[] paramValues = BuildSPValues(spParams);
            using (var context = new T())
            {
                context.Database.ExecuteSqlCommand($"EXEC {spName} {paramsCombined}", paramValues);
            }
        }

        private static object[] BuildSPValues(List<StoredProcedureRequestModel> spParams)
        {
            List<object> values = new List<object>();
            foreach (var param in spParams)
            {
                if (!param.IsCustomType)
                {
                    values.Add(new SqlParameter($"{param.ParamName}", param.Value));
                    continue;
                }
                var table = new DataTable();
                table.Clear();
               // var columnType = param.CustomTypeValues.GetType().GetElementType();
                table.Columns.Add($"{param.CustomTypeColumnName}", param.ColumnType);
                foreach (var val in param.CustomTypeValues)
                {
                    DataRow row = table.NewRow();
                    row[$"{param.CustomTypeColumnName}"] = val;
                    table.Rows.Add(row);
                }
                var customTypeParam = new SqlParameter($"{param.ParamName}", SqlDbType.Structured);
                customTypeParam.TypeName = $"{param.CustomTableTypeName}";
                customTypeParam.Value = table;
                values.Add(customTypeParam);
            }
            return values.ToArray();
        }
    }
}
