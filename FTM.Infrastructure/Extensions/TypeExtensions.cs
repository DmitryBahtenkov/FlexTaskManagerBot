using System.Reflection;

namespace FTM.Infrastructure.Extensions;

public static class TypeExtensions
{
    public static bool ContainsAttribute<TAttribute>(this object o) where TAttribute : Attribute
    {
        return o.GetType().GetCustomAttribute<TAttribute>() is not null;
    }
}