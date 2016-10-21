using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MyUtils.T4.Extensions
{
    public static class ModelExt
    {
        /// <summary>
        /// 实体扩展方法，获取字段名和字段类型
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="obj">实体类</param>
        /// <returns></returns>
        public static Dictionary<string, Type> GetFieldNames<T>(this T obj) where T : class,new()
        {
            Dictionary<string, Type> dictionary = new Dictionary<string, Type>();
            T tempT = new T();
            PropertyInfo[] infos = tempT.GetType().GetProperties();
            if (infos.Count() > 0)
            {
                foreach (PropertyInfo pro in infos)
                {
                    string name = pro.Name;
                    Type valueType = pro.PropertyType;
                    dictionary.Add(name, valueType);
                }
            }
            return dictionary;
        }
    }
}
