using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtils.T4
{
    public static class DBEntitySql
    {
        public static string GetTableInfoSql()
        {
            return @"SELECT
                [TableName] = Case When A.colorder=1 Then D.name Else '' End,
                [FieldName] = A.name,
                [Key] = Case When exists(SELECT 1 FROM sysobjects Where xtype = 'PK' and parent_obj = A.id and name in (SELECT name FROM sysindexes WHERE indid in( SELECT indid FROM sysindexkeys WHERE id = A.id AND colid = A.colid))) then '√' else '' end,
                [Type] = B.name,
                [Length] = COLUMNPROPERTY(A.id, A.name,'PRECISION'),
                [CanNull] = Case When A.isnullable=1 Then '√'Else '' End,
                [DefaultValue] = isnull(E.Text,''),
                [Remark] = isnull(G.[value],'')
                FROM syscolumns A
                Left Join systypes B On A.xusertype=B.xusertype
                Inner Join sysobjects D On A.id=D.id and D.xtype='U' and D.name<>'dtproperties'
                Left Join syscomments E on A.cdefault= E.id
                Left Join sys.extended_properties G on A.id= G.major_id and A.colid= G.minor_id
                Left Join sys.extended_properties F On D.id= F.major_id and F.minor_id= 0
                where d.name=@tablename Order By A.id,A.colorder";
        }
    }
}
