using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtils.Base.T4
{
    public class TableInfoEntity
    {
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string Key { get; set; }
        public string Type { get; set; }
        public string Length { get; set; }
        public string CanNull { get; set; }
        public string DefaultValue { get; set; }
        public string Remark { get; set; }
    }
}
