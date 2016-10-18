using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtils.DBData
{
    public class SqlData : IDbConfig
    {
        public void DbConfig()
        {
            throw new NotImplementedException();
        }

        public int Add<T>(T entity)
        {
            throw new NotImplementedException();
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
    }
}
