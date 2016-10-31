using MetaDataV2;
using MyUtils.T4;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtils.T4
{
    public static class DBEntityHelper
    {
        public static TableInfoEntity GetTableInfo(string filePath, string dbName, string tableName)
        {
            string pathStr = Path.GetDirectoryName(filePath);
            string configPath = Directory.GetFiles(pathStr, "*.config").FirstOrDefault() ?? Directory.GetParent(pathStr).GetFiles("*.config").FirstOrDefault().FullName;
            var config = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = configPath }, ConfigurationUserLevel.None);
            string dbConnectStr = ((ConnectionStringsSection)config.GetSection("connectionStrings")).ConnectionStrings[dbName + "_connect"].ConnectionString;
            TableInfoEntity entity = new TableInfoEntity();
            List<TableFieldInfo> fieldInfo = new List<TableFieldInfo>();
            MdFactory.SetConnectionStr(dbConnectStr);
            Database MyDb = MdFactory.SetCurrentDbName("TCInterVacationCommon", true);
            Dictionary<string, FieldObject> Decs = new Dictionary<string, FieldObject>();
            Dictionary<string, TableObject> TableDecs = new Dictionary<string, TableObject>();
            foreach (TableObject table in MyDb.GetTableView())
            {
                if (!table.name.Equals(tableName)) continue;
                foreach (FieldObject field in table.Columns)
                {
                    fieldInfo.Add(new TableFieldInfo
                    {
                        FieldName = field.ColumnName,
                        Remark = field.DeTextSimplified,
                        IsKey = field.isPK,
                        IsNull = field.isNull,
                        DefaultValue = field.defaultVal,
                        Type = field.DbTypeNameStr
                    });
                }
                entity.DatabaseName = dbName;
                entity.TableName = tableName;
                entity.FieldInfo = fieldInfo;
            }
            return entity;
        }

        public static string SetModel(TableInfoEntity table)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("using System;").NewLine().NewLine();
            builder.AppendFormat("public partial class {0}", table.TableName).NewLine();
            builder.AppendFormat("{{").NewLine().Tab();
            builder.AppendFormat("#region sql").NewLine().Tab();
            builder.AppendFormat("///<summary>查询SQL</summary>").NewLine().Tab();
            builder.AppendFormat("public static string selectSql = \"select {0} from {1}.dbo.{2} with(nolock) where 1=1\";", string.Join(",", table.FieldInfo.Select(t => t.FieldName)), table.DatabaseName, table.TableName).NewLine().Tab();
            builder.AppendFormat("#endregion").NewLine().Tab();
            builder.AppendFormat("#region 表字段").NewLine();
            table.FieldInfo.ForEach(t =>
            {
                builder.Tab();
                builder.AppendFormat("///<summary>{0}</summary>", t.Remark).NewLine().Tab();
                builder.AppendFormat("public {0} {1} {{ get; set; }}", ConvertParamsType(t.Type), t.FieldName).NewLine();
            });
            builder.Tab();
            builder.AppendFormat("#endregion").NewLine();
            builder.AppendFormat("}}");
            return builder.ToString();
        }

        private static StringBuilder NewLine(this StringBuilder builder)
        {
            return builder.AppendLine();
        }

        private static StringBuilder Tab(this StringBuilder builder)
        {
            return builder.Append("\t");
        }

        private static string ConvertParamsType(string type)
        {
            switch (type)
            {
                case "String": return "string";
                case "Int16": return "short";
                case "Int32": return "int";
                case "Int64": return "long";
                case "Byte": return "byte";
                default: return type;
            }
        }
    }
}
