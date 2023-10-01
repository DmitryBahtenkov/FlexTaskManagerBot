using FTM.Domain.Events.Bot;
using FTM.Domain.ServiceBus;
using FTM.Infrastructure.Services;

namespace FTM.Infrastructure.BotImplementation.Consumers;

public class TelegramUpdateConsumer : IConsumer<RawUpdateEvent>
{
    private readonly TelegramService _telegramService;

    public TelegramUpdateConsumer(TelegramService telegramService)
    {
        _telegramService = telegramService;
    }

    public override async Task Consume(RawUpdateEvent data)
    {
        if (data.Source != Source.Telegram)
        {
            return;
        }

        await _telegramService.FromRaw(data.UpdateRaw);
    }
}