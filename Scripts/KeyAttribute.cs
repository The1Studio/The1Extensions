namespace UniT.Extensions
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class KeyAttribute : Attribute
    {
        public string Key { get; }

        public KeyAttribute(string key)
        {
            this.Key = key;
        }
    }

    public static class KeyAttributeExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetKey(this Type type)
        {
            return type.GetCustomAttribute<KeyAttribute>()?.Key ?? type.Name;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string[] GetKeys(this Type type)
        {
            return type.GetCustomAttributes<KeyAttribute>().Select(attr => attr.Key).ToArray();
        }
    }
}