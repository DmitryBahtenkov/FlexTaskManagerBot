using System.Security.Policy;
using FTM.Domain.Helpers;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FTM.Infrastructure.Extensions;

public static class TelegramExtensions
{
    public static string GetCallbackData(this Update update)
    {
        return update.CallbackQuery?.Data ?? throw new ArgumentNullException();
    }

    public static int GetMessageId(this Update update)
    {
        if (update.Type == UpdateType.Message)
        {
            return update.Message!.MessageId;
        }

        if (update.Type == UpdateType.CallbackQuery)
        {
            return update.CallbackQuery!.Message!.MessageId;
        }

        throw new ArgumentNullException();
    }

    public static string GetMessageText(this Update update)
    {
        if (update.Type == UpdateType.CallbackQuery)
        {
            return update.CallbackQuery!.Message!.Text!;
        }

        return update.Message!.Text! ?? update.Message!.Caption!;
    }

    public static bool HasAttachment(this Update update)
    {
        return update.Message!.Caption is not null || Enum.GetNames<MediaType>().Any(x=> typeof(Message).GetProperty(x)?.GetValue(update.Message) is not null);
    }
}