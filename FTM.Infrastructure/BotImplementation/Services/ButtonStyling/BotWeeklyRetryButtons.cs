using FTM.Domain.Helpers;
using Telegram.Bot.Types.ReplyMarkups;

namespace FTM.Infrastructure.BotImplementation.Services.ButtonStyling
{
    class BotWeeklyRetryButtons
    {
        public static InlineKeyboardMarkup ForWeeklyRetry(int issueId, IEnumerable<RuDayOfWeek>? checkedDays = null)
        {
            var page = new List<List<InlineKeyboardButton>>();
            var buttons = new List<InlineKeyboardButton>();
            var checkUps = new List<InlineKeyboardButton>();
            var saveStateButtons = new List<InlineKeyboardButton>();

            // массив выбранных дней недели по дефолту: понедельник  
            if (checkedDays is null)
            {
                checkedDays = new List<RuDayOfWeek> { DateTime.UtcNow.DayOfWeek.ToRuWeekday() };
            }

            for (int i = 0; i < 7; i++)
            {
                var day = i.ToRuWeekday();

                var dayButton = InlineKeyboardButton.WithCallbackData($"{day.GetDisplay()}");
                var checkUp = InlineKeyboardButton.WithCallbackData("✖️", $"editretry-weekly#{issueId}#");

                // checkUp кнопки содержат в коллбеке массив выбранных дней недели с изменениями, которые произойдут после нажатия:
                // в невыбранных днях ✖️- в массив будет добавлен выбранный день
                // в уже выбранных днях ✔️- из массива вычтется выбранный день
                var setChecked = checkedDays;
                if (checkedDays.Contains(day))
                {
                    checkUp.Text = "✔️";
                    setChecked = setChecked.Where(x => x != day);
                }
                else
                {
                    setChecked = setChecked.Append(day);
                }

                setChecked = setChecked.Distinct();
                var checkedParam = string.Join('-', setChecked.Select(x => x.ToInt()));
                checkUp.CallbackData += checkedParam;

                buttons.Add(dayButton);
                checkUps.Add(checkUp);
            }

            saveStateButtons.Add(
                InlineKeyboardButton.WithCallbackData("Отмена", $"backtoissue#{issueId}")
            );

            saveStateButtons.Add(
            // кнопка готово содержит текущий массив выбранных дней
                InlineKeyboardButton.WithCallbackData("Готово", $"setretry-weekly#{issueId}#{string.Join('-', checkedDays.Select(x => x.ToInt()))}")
            );

            page.Add(buttons);
            page.Add(checkUps);
            page.Add(saveStateButtons);


            return new InlineKeyboardMarkup(page);
        }
    }
}