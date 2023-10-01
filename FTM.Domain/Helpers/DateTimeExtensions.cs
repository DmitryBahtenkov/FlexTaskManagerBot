namespace FTM.Domain.Helpers;

public static class DateTimeExtensions
{
    public static string ToBotFormatWithTime(this DateTime dateTime)
    {
        return dateTime.ToString("dd.MM.yyyy HH:mm");
    }
    
    public static DateTime FromString(this string str)
    {
        return DateTime.ParseExact(str, "dd.MM.yyyy HH:mm", null);
    }
}