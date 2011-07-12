using System;
using System.Collections.Generic;
using System.Reflection;

namespace AgUnit.Runner.Resharper60.Util
{
    public static class ReflectionExtensions
    {
        private static readonly IDictionary<Tuple<Type, string>, FieldInfo> FieldInfoCache = new Dictionary<Tuple<Type, string>, FieldInfo>();

        public static void SetField(this object target, string fieldName, object value)
        {
            GetFieldInfo(target, fieldName).SetValue(target, value);
        }

        public static T GetField<T>(this object target, string fieldName)
        {
            return (T)GetFieldInfo(target, fieldName).GetValue(target);
        }

        private static FieldInfo GetFieldInfo(object target, string fieldName)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (fieldName == null) throw new ArgumentNullException("fieldName");

            var targetType = target.GetType();
            var cacheKey = Tuple.Create(targetType, fieldName);

            FieldInfo fieldInfo;

            if (!FieldInfoCache.TryGetValue(cacheKey, out fieldInfo))
            {
                fieldInfo = targetType.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
                FieldInfoCache[cacheKey] = fieldInfo;
            }

            return fieldInfo;
        }
    }
}