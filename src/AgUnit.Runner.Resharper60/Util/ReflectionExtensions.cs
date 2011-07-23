using System;
using System.Collections.Generic;
using System.Reflection;

namespace AgUnit.Runner.Resharper60.Util
{
    public static class ReflectionExtensions
    {
        private static readonly IDictionary<Tuple<Type, string>, FieldInfo> FieldInfoCache = new Dictionary<Tuple<Type, string>, FieldInfo>();
        private static readonly IDictionary<Tuple<Type, string>, PropertyInfo> PropertyInfoCache = new Dictionary<Tuple<Type, string>, PropertyInfo>();
        private static readonly IDictionary<Tuple<Type, string>, MethodInfo> MethodInfoCache = new Dictionary<Tuple<Type, string>, MethodInfo>();

        public static void SetField(this object target, string name, object value)
        {
            GetFieldInfo(target, name).SetValue(target, value);
        }

        public static T GetField<T>(this object target, string name)
        {
            return (T)GetFieldInfo(target, name).GetValue(target);
        }

        public static void SetProperty(this object target, string name, object value)
        {
            GetPropertyInfo(target, name).SetValue(target, value, null);
        }

        public static T GetProperty<T>(this object target, string name)
        {
            return (T)GetPropertyInfo(target, name).GetValue(target, null);
        }

        public static object CallMethod<T>(this object target, string name, params object[] parameters)
        {
            return (T)CallMethod(target, name, parameters);
        }

        public static object CallMethod(this object target, string name, params object[] parameters)
        {
            return GetMethodInfo(target, name).Invoke(target, parameters);
        }

        private static FieldInfo GetFieldInfo(object target, string fieldName)
        {
            return GetMemberInfo(target, fieldName, FieldInfoCache, (type, name, bindingAttr) => type.GetField(name, bindingAttr));
        }

        private static PropertyInfo GetPropertyInfo(object target, string propertyName)
        {
            return GetMemberInfo(target, propertyName, PropertyInfoCache, (type, name, bindingAttr) => type.GetProperty(name, bindingAttr));
        }

        private static MethodInfo GetMethodInfo(object target, string methodName)
        {
            return GetMemberInfo(target, methodName, MethodInfoCache, (type, name, bindingAttr) => type.GetMethod(name, bindingAttr));
        }

        private static T GetMemberInfo<T>(object target, string name, IDictionary<Tuple<Type, string>, T> cache, Func<Type, string, BindingFlags, T> getMemberInfo)
            where T : MemberInfo
        {
            if (target == null) throw new ArgumentNullException("target");
            if (name == null) throw new ArgumentNullException("name");

            var targetType = target.GetType();
            var cacheKey = Tuple.Create(targetType, name);

            T memberInfo;

            if (!cache.TryGetValue(cacheKey, out memberInfo))
            {
                memberInfo = GetMemberInfoRecursive(name, targetType, getMemberInfo);
                cache[cacheKey] = memberInfo;
            }

            return memberInfo;
        }

        private static T GetMemberInfoRecursive<T>(string name, Type targetType, Func<Type, string, BindingFlags, T> getMemberInfo)
            where T : MemberInfo
        {
            var memberInfo = getMemberInfo(targetType, name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (memberInfo == null && targetType.BaseType != null)
            {
                memberInfo = GetMemberInfoRecursive(name, targetType.BaseType, getMemberInfo);
            }

            return memberInfo;
        }
    }
}