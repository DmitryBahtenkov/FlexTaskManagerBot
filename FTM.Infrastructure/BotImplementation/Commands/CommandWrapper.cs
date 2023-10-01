using System.Reflection;
using FTM.Domain.Exceptions;
using FTM.Domain.Helpers;
using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.ServiceBus;
using FTM.Domain.Services;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Exceptions;
using FTM.Infrastructure.BotImplementation.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FTM.Infrastructure.BotImplementation.Commands;

public class CommandWrapper
{
    private readonly IUnitOfWork<BotStatus> _unitOfWork;
    private readonly ILogger<CommandWrapper> _logger;
    private readonly ICurrentUserService _currentUserService;
    private readonly MessengerService _messengerService;

    public CommandWrapper(
        IUnitOfWork<BotStatus> unitOfWork, 
        ILogger<CommandWrapper> logger, 
        ICurrentUserService currentUserService,
        MessengerService messengerService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _currentUserService = currentUserService;
        _messengerService = messengerService;
    }

    public async Task HandleCommand(Update update, string name, IHandler<Update, BotStatus> handler)
    {
        try
        {
            var chat = GetChatId(update);

            var status = await _unitOfWork.GetRepository().GetAsync(x => x.ChatId == chat);

            if (status?.User is not null)
            {
                _currentUserService.Set(status.User);
            }

            if (status is null)
            {
                status = new BotStatus
                {
                    ChatId = chat,
                    State = BotState.Started
                };
                await _unitOfWork.GetRepository().AddAsync(status);
            }

            if (!status.UserId.HasValue)
            {
                if (IsAuthRequired(handler))
                {
                    throw new BusinessException("Вы не авторизованы");
                }
            }

            await handler.Handle(update, status);

            await _unitOfWork.SaveChangesAsync();
        }
        catch (BotBusinessException botBusinessException)
        {
            _logger.LogWarning(botBusinessException, "business error handler");
            await _messengerService.SendMessage(GetChatId(update), botBusinessException.Message, botBusinessException.ReplyMarkup);
        }
        catch (BusinessException businessException)
        {
            _logger.LogWarning(businessException, "business error handler");
            await _messengerService.SendMessage(GetChatId(update), businessException.Message);
        }
        catch (ArgumentException argumentException)
        {
            if (argumentException.ParamName != "ChatId")
            {
                throw;
            }
            
            _logger.LogWarning(argumentException, 
                "ChatId not found in update {update}",
                JsonConvert.SerializeObject(update));
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                "Error command {commandType}, chatId: {chatId}",
                name, GetChatId(update));
        }
    }

    private string GetChatId(Update update)
    {
        if (update.Type == UpdateType.Message)
        {
            return update.Message!.Chat.Id.ToString();
        }
        if (update.Type == UpdateType.CallbackQuery)
        {
            return update.CallbackQuery!.Message!.Chat.Id.ToString();
        }

        throw new ArgumentException("Не удалось распарсить идентификатор чата", "ChatId");
    }

    private bool IsAuthRequired(IHandler<Update, BotStatus> handler)
    {
        var attr = handler.GetType().GetCustomAttribute<AnonAttribute>();
        return attr is null;
    }
}