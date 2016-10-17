using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyUtils.Entity
{
    /// <summary>实体转换辅助类</summary>   
    public class EntityConvertHelper<T> where T : class, new()
    {
        /// <summary>DateTable转List</summary>
        /// <param name="dt">DateTable</param>
        /// <returns>List</returns>
        public static List<T> ConvertToList(DataTable dt)
        {
            List<T> list = new List<T>();
            T t = new T();
            PropertyInfo[] propertys = t.GetType().GetProperties();// 获得此模型的公共属性     
            foreach (DataRow dr in dt.Rows)
            {
                t = new T();
                foreach (PropertyInfo pi in propertys)
                {
                    if (dt.Columns.Contains(pi.Name))
                    {
                        if (!pi.CanWrite) continue;// 判断此属性是否有Setter     
                        object value = dr[pi.Name];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                list.Add(t);
            }
            return list;
        }

        /// <summary>DateTable转实体</summary>
        /// <param name="dt">DateTable</param>
        /// <returns>实体</returns>
        public static T ConvertToEntity(DataTable dt)
        {
            if (dt == null || dt.Rows.Count.Equals(0)) return new T();
            var list = ConvertToList(dt);
            if (list != null && list.Count > 0) return list[0];
            return new T();
        }

        /// <summary>List转换成DataTable, 主键为自增长</summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="value">List</param>
        /// <returns>DataTable</returns>
        public static DataTable ConvertToDataTable(List<T> value)
        {
            var properties = TypeDescriptor.GetProperties(typeof(T));
            var dataTable = new DataTable();
            for (var i = 0; i < properties.Count; i++)
            {
                var property = properties[i];
                dataTable.Columns.Add(property.Name, property.PropertyType);
            }
            var data = new object[properties.Count];
            value.ForEach(x =>
            {
                for (var i = 0; i < data.Length; i++)
                {
                    data[i] = properties[i].GetValue(x);
                }
                dataTable.Rows.Add(data);
            });
            //移除PkValue列
            if (dataTable.Columns.Contains("PkValue"))
            {
                dataTable.Columns.Remove("PkValue");
            }
            return dataTable;
        }
    }
}
