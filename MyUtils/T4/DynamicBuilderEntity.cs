using System;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;

namespace MyUtils.T4
{
    /// <summary>
    /// 动态创建实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DynamicBuilderEntity<T>
    {
        private static readonly MethodInfo getValueMethod = typeof(IDataRecord).GetMethod("get_Item", new[] { typeof(int) });
        private static readonly MethodInfo isDBNullMethod = typeof(IDataRecord).GetMethod("IsDBNull", new[] { typeof(int) });
        private delegate T Load(IDataRecord dataRecord);
        public IDataRecord IRecord { get; private set; }
        private Load handler;
        private DynamicBuilderEntity() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRecord"></param>
        /// <returns></returns>
        public T Build()
        {
            return handler(IRecord);
        }

        /// <summary>build </summary>
        /// <param name="dataRecord">数据记录</param>
        /// <returns>泛型</returns>
        public T Build(IDataRecord dataRecord)
        {
            return handler(dataRecord);
        }
        /// <summary>
        /// 建立过程
        /// </summary>
        /// <param name="dataRecord">取数据流字段</param>
        /// <returns></returns>
        public static DynamicBuilderEntity<T> CreateBuilder(IDataRecord dataRecord)
        {
            var dynamicBuilder = new DynamicBuilderEntity<T>();
            dynamicBuilder.IRecord = dataRecord;
            var method = new DynamicMethod("DynamicCreateEntity", typeof(T), new[] { typeof(IDataRecord) }, typeof(T), true);
            ILGenerator generator = method.GetILGenerator();
            LocalBuilder result = generator.DeclareLocal(typeof(T));
            generator.Emit(OpCodes.Newobj, typeof(T).GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Stloc, result);
            if (dataRecord != null && dataRecord.FieldCount > 0)
            {
                for (int i = 0; i < dataRecord.FieldCount; i++)
                {
                    var propertyInfo = typeof(T).GetProperty(dataRecord.GetName(i));
                    if (propertyInfo == null)
                    {
                        continue;
                    }
                    var endIfLabel = generator.DefineLabel();
                    if (propertyInfo.GetSetMethod() == null) continue;
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, isDBNullMethod);
                    generator.Emit(OpCodes.Brtrue, endIfLabel);
                    generator.Emit(OpCodes.Ldloc, result);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, getValueMethod);
                    generator.Emit(OpCodes.Unbox_Any, dataRecord.GetFieldType(i));
                    generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
                    generator.MarkLabel(endIfLabel);
                }
            }
            generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);
            dynamicBuilder.handler = (Load)method.CreateDelegate(typeof(Load));
            return dynamicBuilder;
        }
    }
}