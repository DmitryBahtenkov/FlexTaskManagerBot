using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace FTM.Domain.Helpers;

public static class EnumHelper
{
    public static string GetDisplay<TEnum>(this TEnum @enum) where TEnum : Enum
    {
        var enumType = typeof(TEnum);
        var memberInfos = enumType.GetMember(@enum.ToString());
        var enumValueMemberInfo = memberInfos.First(m => m.DeclaringType == enumType);
        var valueAttribute = enumValueMemberInfo.GetCustomAttribute<DisplayAttribute>();

        return valueAttribute?.Name ?? @enum.ToString();
    }
}