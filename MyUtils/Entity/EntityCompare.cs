using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyUtils.Entity
{
    /// <summary>
    /// 实体比较帮助类
    /// </summary>
    public static class EntityCompare
    {
        /// <summary>
        /// 实体比较，field不传或长度为0则比较所有字段
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="firstEntity">实体1</param>
        /// <param name="secondEntity">实体2</param>
        /// <param name="field">比较字段</param>
        /// <returns>实体字段值比较结果</returns>
        public static bool CompareModel<T>(this T firstEntity, T secondEntity, List<string> field = null)
        {
            bool flag = true;
            if (field == null || field.Count.Equals(0))
            {
                field = typeof(T).GetProperties().Select(t => t.Name).ToList();
            }
            foreach (string s in field)
            {
                PropertyInfo propertyInfo = typeof(T).GetProperty(s);
                var a1 = propertyInfo.GetValue(firstEntity, null);
                var a2 = propertyInfo.GetValue(secondEntity, null);
                if (a1.Equals(a2)) continue;
                flag = false;
                break;
            }
            return flag;
        }
    }
}
