using FTM.Domain.Exceptions;
using Telegram.Bot.Types.ReplyMarkups;

namespace FTM.Infrastructure.BotImplementation.Exceptions;

public class BotBusinessException : BusinessException
{
    public IReplyMarkup ReplyMarkup { get; }
    
    public BotBusinessException(string mes, IReplyMarkup replyMarkup) : base(mes)
    {
        ReplyMarkup = replyMarkup;
    }

    public BotBusinessException(string mes, Exception inner, IReplyMarkup replyMarkup) : base(mes, inner)
    {
        ReplyMarkup = replyMarkup;
    }
}