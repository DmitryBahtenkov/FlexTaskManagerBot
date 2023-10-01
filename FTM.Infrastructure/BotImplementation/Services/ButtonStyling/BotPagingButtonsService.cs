using FTM.Domain.Models.IssueModel;
using FTM.Domain.Units;
using Telegram.Bot.Types.ReplyMarkups;

namespace FTM.Infrastructure.BotImplementation.Services.ButtonStyling;

class BotPagingButtons
{
    #region Оффсет от нечетной страницы до первой в панели

    // Ранк нечетной страницы в панели (с 6ю страничными кнопками)
    private const double middleOddPage = 11 % 6;
    private const double lastOddPage = 13 % 6;

    // Оффсет ранка до начала в панели(с 6ю страничными кнопками)
    private const int firstRankOffset = 0;
    private const int middleRankOffset = 2;
    private const int lastRankOffset = 4;

    #endregion

    public static InlineKeyboardMarkup GetForPaging(PagingResult<Issue> issues, InlineKeyboardButton? backButton = null)
    {
        var buttons = new List<List<InlineKeyboardButton>>();

        foreach (var issue in issues.Items)
        {
            var issueName = issue.Status == IssueStatus.Finished ? "✅ " + issue.Text : issue.Text;
            var infoButton = InlineKeyboardButton.WithCallbackData(issueName, "info#" + issue.Id);
            var doneButton = issue.Status is not IssueStatus.Finished
                ? InlineKeyboardButton.WithCallbackData("✅", "done#" + issue.Id)
                : null;
            var editButton = InlineKeyboardButton.WithCallbackData("✏", "edit#" + issue.Id);
            var deleteButton = InlineKeyboardButton.WithCallbackData("❌", "delete#" + issue.Id);

            buttons.Add(new List<InlineKeyboardButton> { infoButton });
            buttons.Add(doneButton is null
                ? new List<InlineKeyboardButton> { editButton, deleteButton }
                : new List<InlineKeyboardButton> { doneButton, editButton, deleteButton });
        }

        if (backButton is not null)
        {
            buttons.Add(new List<InlineKeyboardButton>(1)
            {
                backButton
            });
        }

        if (issues.TotalPages > 1)
        {
            var paging = new List<InlineKeyboardButton>();

            var panelStartPage = 1;
            var panelMaxBtnCount = 7; // На 1ой панели _7_ кнопок страниц и кнопка перехода вперед (опционально)
            var panelLastPage = Math.Min(panelMaxBtnCount, issues.TotalPages);

            if (issues.CurrentPage > 7)
            {
                var isOddPage = 1;

                // На следующих панелях (от 8 страницы и т.д.) _6_ кнопок страниц, т.к. появляются кнопки назад и вперед (опционально)
                panelMaxBtnCount = 6;

                panelStartPage = issues.CurrentPage;
                if (issues.CurrentPage % 2 == 0)
                {
                    panelStartPage++;
                    isOddPage--;
                }

                var offsetTillStart = firstRankOffset;
                var pageRank = panelStartPage % panelMaxBtnCount; 

                if (pageRank == middleOddPage)
                {
                    offsetTillStart = middleRankOffset;
                }
                else if (pageRank == lastOddPage)
                {
                    offsetTillStart = lastRankOffset;
                }

                panelStartPage = issues.CurrentPage - isOddPage - offsetTillStart;

                panelLastPage = Math.Min(issues.TotalPages, panelStartPage + panelMaxBtnCount - 1);

                // Новая текущая страница после перехода по кнопке назад - 1 страница на предыдущей панели
                var downPagePanel = panelStartPage - panelMaxBtnCount;

                // Проверка 1 кнопки на текущей панели, на случай перехода с 2й панели (8-14 стр.) на 1ую (1-7 стр.)
                if (panelStartPage is 8)
                {
                    downPagePanel--;
                }

                paging.Add(InlineKeyboardButton.WithCallbackData("⬅", $"taskpaging#{downPagePanel}"));
            }


            for (var i = panelStartPage; i <= panelLastPage; i++)
            {
                if (issues.CurrentPage != i)
                {
                    paging.Add(InlineKeyboardButton.WithCallbackData((i).ToString(), $"taskpaging#{i}"));
                }
                else
                {
                    paging.Add(InlineKeyboardButton.WithCallbackData("👀", $"taskpaging#{i}"));
                }
            }

            if (issues.TotalPages - panelLastPage > 0)
            {
                paging.Add(InlineKeyboardButton.WithCallbackData("➡", $"taskpaging#{panelLastPage + 1}"));
            }

            buttons.Add(paging);
        }

        return new InlineKeyboardMarkup(buttons);
    }
}