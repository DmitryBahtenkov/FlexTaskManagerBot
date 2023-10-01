using FTM.Domain.Events.Statistics;
using FTM.Domain.Factories;
using FTM.Domain.Models.StatisticModel;
using FTM.Domain.ServiceBus;
using FTM.Domain.Services;
using FTM.Infrastructure.BotImplementation.Commands;
using FTM.Infrastructure.Extensions;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FTM.Infrastructure.Services;

public class TelegramService
{
    private readonly BotCommandFactory<Update> _botCommandFactory;
    private readonly BotHandlerFactory<Update> _botHandlerFactory;
    private readonly CommandWrapper _commandWrapper;
    private readonly IPublisherService _publisherService;

    public TelegramService(BotCommandFactory<Update> botCommandFactory,
        BotHandlerFactory<Update> botHandlerFactory,
        CommandWrapper commandWrapper,
        IPublisherService publisherService)
    {
        _botCommandFactory = botCommandFactory;
        _botHandlerFactory = botHandlerFactory;
        _commandWrapper = commandWrapper;
        _publisherService = publisherService;
    }

    public async Task FromRaw(string json)
    {
        var update = JsonConvert.DeserializeObject<Update>(json);
        if (update.Type == UpdateType.CallbackQuery)
        {
            await SendEvent(update.CallbackQuery!.Data!, update.Type);
            var handler = _botHandlerFactory.Get(update.CallbackQuery!.Data!);
            if (handler is not null)
            {
                await _commandWrapper.HandleCommand(update, handler.Command, handler);
            }
        }
        else if (update.Type == UpdateType.Message)
        {
            var text = update.GetMessageText();

            if (update.HasAttachment())
            {
                text = string.Empty;
            }

            var command = _botCommandFactory.Get(text);

            if (!command.ContainsAttribute<WithoutStatisticAttribute>())
            {
                await SendEvent(text, UpdateType.Message);
            }

            await _commandWrapper.HandleCommand(update, command.Command, command);
        }
    }

    private Task SendEvent(string key, UpdateType type)
    {
        object? data = null;
        if (type is UpdateType.CallbackQuery)
        {
            var splitted = key.Split("#");
            key = splitted.First();
            data = new TelegramHandlerInfo(splitted);
        }

        return _publisherService.Publish(new StatisticEntryEvent
        {
            Key = key,
            Data = data
        });
    }

    private record TelegramHandlerInfo(string[] Parameters);
}