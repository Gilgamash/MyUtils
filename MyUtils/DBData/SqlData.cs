using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using MyUtils.Base.DBData;

namespace MyUtils.DBData
{
    public class SqlData : IDbConfig
    {
        private string dbConnectStr;

        public void DbConfig()
        {
            dbConnectStr = ConfigurationManager.ConnectionStrings[""].ConnectionString;
        }

        public int Add<T>(T entity)
        {
            int id = 0;
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = new SqlConnection(dbConnectStr))
            {

            }
            return id;
        }

        public int Update<T>(T entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(int id)
        {
            throw new NotImplementedException();
        }

        public T SelectEntity<T>(int id)
        {
            throw new NotImplementedException();
        }

        public List<T> SelectList<T>(NameValueCollection param)
        {
            throw new NotImplementedException();
        }

        private string ConvertSql<T>(T entity, DBOperateEnum operate)
        {
            string sql = string.Empty;
            switch (operate)
            {
                case DBOperateEnum.Insert:
                    break;
                case DBOperateEnum.Update:
                    break;
                case DBOperateEnum.Delete:
                    break;
                case DBOperateEnum.Select:
                    break;
                default:
                    break;
            }
            return sql;
        }

        private string InsertSql<T>(T entity)
        {
            string sql = string.Empty;

            return sql;
        }
    }
}
