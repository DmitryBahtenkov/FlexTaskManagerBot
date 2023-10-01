using FTM.Domain.Helpers;

namespace FTM.Infrastructure.BotImplementation.Services
{
    public class RetryService
    {
        /// <summary>
        /// На основе выбранных дней недели подсчитывает ближайший повтор и сколько до него включительно осталось дней
        /// </summary>
        /// <param name="selected">Выбранные дни недели для повторов</param>
        /// <param name="today">Текущий день недели, от которого нужно подсчитывать ближайший повтор</param>
        /// <returns>retryWeekday - день недели ближайшего повтора, 
        /// nextRetryInDays - сколько дней включительно осталось до ближайшего повтора (retryWeekday),
        /// orderedSelection - выбранные дни недели повтора, сортированные по близости к текущему дню недели today</returns>
        public static (int nextRetryInDays, int retryWeekday, RuDayOfWeek[] orderedSelection) GetRetryPeriods(RuDayOfWeek[] selected, RuDayOfWeek today)
        {
            selected = selected.OrderBy(x => ((int)x)).ToArray();
            var weekdays = selected.Where(x => x > today).Concat(selected.Where(x => x <= today));

            if (weekdays.Where(x=> x!= today).Count() == 0)
            {
                return (7, ((int)today), weekdays.ToArray());
            }

            var nextRetry = (int)weekdays.First();

            if (nextRetry < ((int)today))
            {
                nextRetry += 7;
            }

            var nextRetryInDays = nextRetry - ((int)today);

            return (nextRetryInDays, (int)weekdays.First(), weekdays.ToArray());
        }
    }
}