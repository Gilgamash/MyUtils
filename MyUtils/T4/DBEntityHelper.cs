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
            MdFactory.SetConnectionStr(dbConnectStr);
            Database MyDb = MdFactory.SetCurrentDbName("TCInterVacationCommon", true);
            Dictionary<string, FieldObject> Decs = new Dictionary<string, FieldObject>();
            Dictionary<string, TableObject> TableDecs = new Dictionary<string, TableObject>();
            foreach (TableObject table in MyDb.GetTableView())
            {
                if (!TableDecs.ContainsKey(table.name)) TableDecs.Add(table.name, table);
                foreach (FieldObject field in table.Columns)
                    if (!Decs.ContainsKey(field.ColumnName)) Decs.Add(field.ColumnName, field);
            }
            return entity;
        }
    }
}
