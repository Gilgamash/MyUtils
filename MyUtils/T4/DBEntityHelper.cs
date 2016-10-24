using MetaDataV2;
using MyUtils.Base.T4;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtils.T4
{
    public class DBEntityHelper
    {
        private string dbConnectStr;

        public DBEntityHelper()
        {
            dbConnectStr = ConfigurationManager.ConnectionStrings["sqlserver"].ConnectionString;
        }

        public TableInfoEntity GetTableInfo(string tableName)
        {
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
    }
}
