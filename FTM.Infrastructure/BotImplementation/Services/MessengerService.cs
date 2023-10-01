using FTM.Domain.Exceptions;
using FTM.Domain.Helpers;
using FTM.Domain.Models.IssueFileModel;
using Npgsql;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace FTM.Infrastructure.BotImplementation.Services;

public class MessengerService
{
    private readonly BotClientService _botClientService;

    public MessengerService(BotClientService botClientService)
    {
        _botClientService = botClientService;
    }

    public async Task<Message> SendMessage(string chatId, string message) => await _botClientService.Client.SendTextMessageAsync(chatId, message);

    public async Task<Message> SendMessage(string chatId, string message, IReplyMarkup markup) => await _botClientService.Client.SendTextMessageAsync(chatId, message, replyMarkup: markup);

    public async Task<Message> SendAttachment(string chatId, IssueFile file, string caption = null)
    {
        var attachment = new InputOnlineFile(file.FileId!);
        caption ??= "Приложение к задаче";

        Message message = null;

        switch (file.Type)
        {
            case MediaType.Photo:
                message = await _botClientService.Client.SendPhotoAsync(chatId, attachment, caption);
                break;

            case MediaType.Video:
                message = await _botClientService.Client.SendVideoAsync(chatId, attachment, caption: caption);
                break;

            case MediaType.Document:
                message = await _botClientService.Client.SendDocumentAsync(chatId, attachment, caption: caption);
                break;

            case MediaType.Audio:
                message = await _botClientService.Client.SendAudioAsync(chatId, attachment, caption);
                break;
            default:
                throw new BusinessException($"Отправка типа вложения {file.Type} не поддерживается");
        }

        return message!;
    }

    public async Task<Message> EditMessage(string chatId, int messageId, string message, InlineKeyboardMarkup? markup = null) => await _botClientService.Client.EditMessageTextAsync(chatId, messageId, message, replyMarkup: markup);

    public async Task DeleteMessage(string chatId, int messageId) => await _botClientService.Client.DeleteMessageAsync(chatId, messageId);
}