using FTM.Domain.Helpers;
using FTM.Domain.Models.BotStatusModel;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Commands.Common;

[Anon]
public class HelpCommand : TelegramCommandBase
{
    private readonly MessengerService _messengerService;

    public HelpCommand(MessengerService messengerService)
    {
        _messengerService = messengerService;
    }

    public override string Command => "help";
    public override Task Handle(Update update, BotStatus botStatus)
    {
        return _messengerService.SendMessage(botStatus.ChatId,
            "Привет! Я бот для управления личными задачами в телеграме. " +
            "Я помогу тебе организовать свой день и не забывать важные дела. " +
            "Вот список доступных команд:\n\n/start - начать работу с ботом\n/issues - посмотреть текущие задачи\n/daily - " +
            "посмотреть задачи на сегодня\n/all - посмотреть все задачи, включая выполненные\n/settings - " +
            "настроить параметры бота\n\n" +
            "Кроме того, я советую тебе ознакомиться с этим гайдом: " +
            "https://telegra.ph/Gajd-po-Flex-Task-Manager-04-05. " +
            "Здесь ты найдешь подробную инструкцию и много полезной информации. " + 
            "А ещё ты можешь оставить обратную связь в этой форме  - https://forms.gle/hWw9gcPfjFVYsELi8 \n" +
            "Приятного использования!");
    }

    public override string? Description => "Инструкция к боту";
}