using FTM.Domain.Helpers;
using FTM.Domain.Models.IssueFileModel;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.Models.IssueModel.DTO;
using FTM.Domain.Models.SettingsModel;
using FTM.Domain.Units;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FTM.Infrastructure.BotImplementation.Services;

public static class BotButtons
{
    public static InlineKeyboardMarkup Settings()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new [] {InlineKeyboardButton.WithCallbackData("🕑Настройки временной зоны", "timezonepage#")},
            new []{InlineKeyboardButton.WithCallbackData("🔈Настройки ежедневного списка задач", "dailypage")}
        });
    }

    public static InlineKeyboardMarkup ForTimezone(Settings settings)
    {
        var timezone = settings.Timezone ?? 3;

        var buttons = new List<InlineKeyboardButton>(3);

        if (timezone != -12)
        {
            buttons.Add(InlineKeyboardButton.WithCallbackData("<", $"settimezone#{timezone - 1}"));
        }

        buttons.Add(InlineKeyboardButton.WithCallbackData(timezone.ToString(), $"settimezone#{timezone}"));

        if (timezone != 12)
        {
            buttons.Add(InlineKeyboardButton.WithCallbackData(">", $"settimezone#{timezone + 1}"));
        }

        var back = InlineKeyboardButton.WithCallbackData("Назад", "backtosettings#");

        return new InlineKeyboardMarkup(new[] { buttons, new List<InlineKeyboardButton>(1) { back } });
    }

    public static InlineKeyboardMarkup GetForIssue(Issue issue, int? attachmentMsgId = null)
    {
        InlineKeyboardButton doneButton = issue.Status switch
        {
            IssueStatus.Started => InlineKeyboardButton.WithCallbackData("✅", $"done#{issue.Id}"),
            IssueStatus.Finished => InlineKeyboardButton.WithCallbackData("🔁", $"restore#{issue.Id}")
        };

        var buttons = new List<InlineKeyboardButton>
        {
            doneButton,
            InlineKeyboardButton.WithCallbackData("✏", $"edit#{issue.Id}"),
            InlineKeyboardButton.WithCallbackData("❌", $"delete#{issue.Id}"),
        };

        if (issue.IssueFile is not null)
        {
            if (attachmentMsgId is not null)
            {
                buttons.Add(InlineKeyboardButton.WithCallbackData("📄", $"get-attach#{issue.Id}#{attachmentMsgId}"));
            }
            else
            {
                buttons.Add(InlineKeyboardButton.WithCallbackData("📎", $"get-attach#{issue.Id}"));
            }
        }

        return new InlineKeyboardMarkup(buttons);
    }

    public static InlineKeyboardMarkup GetForConfirmation(string tempIssueId, IEnumerable<DateVariant> variants)
    {
        return new InlineKeyboardMarkup(variants.Select((x, i) =>
            new[] { InlineKeyboardButton.WithCallbackData($"{x.Text}: {x.Date.ToBotFormatWithTime()}", $"confirm#{tempIssueId}#{i}") })
            .Append(new[] { InlineKeyboardButton.WithCallbackData("Сохранить без даты", $"confirm#{tempIssueId}#{-1}") }));
    }

    public static InlineKeyboardButton[] GetForIssue(int id, bool hasAttachment = false, int? attachmentMsgId = null)
    {
        var buttons = new List<InlineKeyboardButton>
        {
            InlineKeyboardButton.WithCallbackData("✅", $"done#{id}"),
            InlineKeyboardButton.WithCallbackData("✏", $"edit#{id}"),
            InlineKeyboardButton.WithCallbackData("❌", $"delete#{id}")
        };

        if (hasAttachment)
        {
            if (attachmentMsgId is not null)
            {
                buttons.Add(InlineKeyboardButton.WithCallbackData("📄", $"get-attach#{id}#{attachmentMsgId}"));
            }
            else
            {
                buttons.Add(InlineKeyboardButton.WithCallbackData("📎", $"get-attach#{id}"));
            }
        }

        return buttons.ToArray();
    }

    public static InlineKeyboardMarkup GetFolderButtons(IEnumerable<string> folders, InlineKeyboardButton? back = null)
    {
        var result = folders.Select(x => new[] { InlineKeyboardButton.WithCallbackData(x, $"fromfolder#{x}") });

        if (back is not null)
        {
            result = result.Append(new[] { back });
        }

        return new InlineKeyboardMarkup(result);
    }

    public static InlineKeyboardMarkup GetForEditIssue(Issue issue)
    {
        var buttons = new List<InlineKeyboardButton[]>()
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("Изменить текст",
                    $"editfield#{nameof(issue.Text)}")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Добавить вложение",
                    $"editfield#{nameof(issue.IssueFile)}")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Изменить вложение",
                    $"editfield#{nameof(issue.IssueFile)}")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Изменить заметку задачи",
                    $"editfield#{nameof(issue.Note)}")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Изменить категорию",
                    $"editfield#{nameof(issue.Folder)}")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Изменить напоминания",
                    $"editfield#{nameof(issue.RemindTime)}")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Изменить повтор задачи", $"editretry#{issue.Id}")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Назад",
                    $"backtoissue#{issue.Id}")
            },
        };

        if (issue.RemindTime is null)
        {
            buttons.RemoveAt(buttons.Count - 2);
        }

        if (issue.IssueFile is null)
        {
            buttons.RemoveAt(2);
        }
        else
        {
            buttons.RemoveAt(1);
        }

        return new InlineKeyboardMarkup(buttons);
    }

    public static InlineKeyboardMarkup ForRetry(int issueId)
    {
        var buttons = new List<IEnumerable<InlineKeyboardButton>>
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("Ежедневно", $"setretry#{issueId}#1")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Еженедельно", $"editretry-weekly#{issueId}")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Каждые 30 дней", $"setretry#{issueId}#30")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Отключить повтор", $"removeretry#{issueId}")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Назад", $"backtoissue#{issueId}")
            }
        };

        return new InlineKeyboardMarkup(buttons);
    }

    public static InlineKeyboardMarkup ForDone(Issue issue)
    {
        var buttons = new List<IEnumerable<InlineKeyboardButton>>
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("Вернуть в работу", $"restore#{issue.Id}")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("К списку задач", $"issues#")
            }
        };

        return new InlineKeyboardMarkup(buttons);
    }

    public static InlineKeyboardMarkup ForDelete()
    {
        return new[]
        {
            InlineKeyboardButton.WithCallbackData("К списку задач", $"issues#")
        };
    }

    public static InlineKeyboardMarkup ForUpdateValidation(Issue issue)
    {
        var buttons = new List<IEnumerable<InlineKeyboardButton>>
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("Вернуться к задаче", $"backtoissue#{issue.Id}")
            }
        };

        return new InlineKeyboardMarkup(buttons);
    }
}