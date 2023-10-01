using Telegram.Bot;

namespace FTM.Infrastructure.BotImplementation.Services;

public class BotClientService
{
    private static ITelegramBotClient _telegramBotClient;

    public BotClientService()
    {
    }

    public ITelegramBotClient Client => _telegramBotClient;

    public static async Task Initialize(BotOptions options, BotCommandsService botCommandsService)
    {
        var client = new TelegramBotClient(new TelegramBotClientOptions(options.AccessKey));
        await client.SetWebhookAsync(options.CallbackUrl);
        await client.SetMyCommandsAsync(botCommandsService.GetCommands());

        _telegramBotClient = client;
    }
}

public class BotOptions
{
    public string AccessKey { get; set; }
    public string CallbackUrl { get; set; }
}