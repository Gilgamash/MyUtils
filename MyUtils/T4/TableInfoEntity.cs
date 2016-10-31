using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtils.T4
{
    public class TableInfoEntity
    {
        public string DatabaseName { get; set; }
        public string TableName { get; set; }
        public List<TableFieldInfo> FieldInfo { get; set; }
    }

    public class TableFieldInfo
    {
        public string FieldName { get; set; }
        public bool IsKey { get; set; }
        public bool IsNull { get; set; }
        public string DefaultValue { get; set; }
        public string Remark { get; set; }
        public string Type { get; set; }
    }
}
