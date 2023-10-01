using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Services;
using FTM.Infrastructure.BotImplementation.Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FTM.Infrastructure.BotImplementation.Processors;

public class AuthProcessor : IBotProcessor<Update>
{
    private readonly MessengerService _messengerService;

    public AuthProcessor(MessengerService messengerService)
    {
        _messengerService = messengerService;
    }

    public BotState State => BotState.Auth;

    public async Task Process(Update update, BotStatus botStatus)
    {
        var text = update.Message!.Text;
        if (botStatus.Token != text)
        {
            await _messengerService.SendMessage(botStatus.ChatId, "Неверный токен. Попробуйте ещё раз");
        }
        else
        {
            botStatus.State = BotState.ListenTasks;
            
            await _messengerService.SendMessage(botStatus.ChatId, "Успешная авторизация!\n" +
                                                                  "Теперь вы можете ввести текст для вашей задачи, например 'Сегодня в 10 уборка' " +
                                                                  "или воспользоваться различными командами из списка команд.");
        }
    }
}