using System.ComponentModel.DataAnnotations;

namespace FTM.Domain.Helpers
{
    #region Сокращенные названия дней недели (ru) 
    public enum RuDayOfWeek : int
    {
        [Display(Name = "Пн")]
        Monday = 0,
        [Display(Name = "Вт")]
        Tuesday = 1,
        [Display(Name = "Ср")]
        Wednesday = 2,
        [Display(Name = "Чт")]
        Thursday = 3,
        [Display(Name = "Пт")]
        Friday = 4,
        [Display(Name = "Сб")]
        Saturday = 5,
        [Display(Name = "Вс")]
        Sunday = 6
    }
    #endregion

    public static class RuDayOfWeekExtension
    {
        public static RuDayOfWeek ToRuWeekday(this DayOfWeek day)
        {
            if (Enum.TryParse<RuDayOfWeek>(day.ToString(), true, out RuDayOfWeek ruDay))
            {
                return ruDay;
            }

            throw new InvalidCastException("День не найден");
        }

        public static int ToInt(this RuDayOfWeek day)
        {
            return ((int)day);
        }

        public static RuDayOfWeek ToRuWeekday(this int weekDayCount)
        {
            if (Enum.IsDefined(typeof(RuDayOfWeek), weekDayCount))
            {
                return (RuDayOfWeek)weekDayCount;
            }

            throw new InvalidCastException("День не найден");
        }

        public static string ToStringEnum(this RuDayOfWeek[]? days)
        {
            return string.Join(", ", days.Select(x => x.GetDisplay()));
        }
    }
}