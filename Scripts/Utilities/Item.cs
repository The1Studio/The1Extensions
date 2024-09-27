#nullable enable
namespace TheOne.Extensions
{
    using System.Diagnostics.CodeAnalysis;

    public static class Item
    {
        public static T S<T>(T item) => item;

        public static bool IsTrue(bool item) => item;

        public static bool IsFalse(bool item) => !item;

        public static bool IsNull<T>([NotNullWhen(false)] T? item) => item is null;

        public static bool IsNotNull<T>([NotNullWhen(true)] T? item) => item is { };
    }
}