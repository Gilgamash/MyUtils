using MetaDataV2;
using MyUtils.Base.T4;
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
        private static string dbConnectStr;

        public static TableInfoEntity GetTableInfo(string filePath, string dbName, string tableName)
        {
            string pathStr = Path.GetDirectoryName(filePath);
            string configPath = Directory.GetFiles(pathStr, "*.config").FirstOrDefault() ?? Directory.GetParent(pathStr).GetFiles("*.config").FirstOrDefault().FullName;
            var config = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = configPath }, ConfigurationUserLevel.None);
            dbConnectStr = ((ConnectionStringsSection)config.GetSection("connectionStrings")).ConnectionStrings[dbName + "_connect"].ConnectionString;
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
                entity.TableName = tableName;
                entity.FieldInfo = fieldInfo;
            }
            return entity;
        }

        public static string SetModel(TableInfoEntity table)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("public class {0}",table.TableName);
            NewLine(builder);
            builder.Append("{");
            NewLine(builder);
            builder.Append("}");
            return builder.ToString();
        }

        private static void NewLine(StringBuilder builder)
        {
            builder.AppendLine();
        }
    }
}
