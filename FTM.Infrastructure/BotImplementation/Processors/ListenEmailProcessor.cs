using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using FTM.Domain.Exceptions;
using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.UserModel.DTO;
using FTM.Domain.Services;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Services;
using Telegram.Bot.Types;
using User = FTM.Domain.Models.UserModel.User;

namespace FTM.Infrastructure.BotImplementation.Processors;

public class ListenEmailProcessor : IBotProcessor<Update>
{
    private readonly IEmailService _emailService;
    private readonly MessengerService _messengerService;
    private readonly IRepository<User> _userRepository;

    public ListenEmailProcessor(
        IEmailService emailService,
        MessengerService messengerService, IRepository<User> userRepository)
    {
        _emailService = emailService;
        _messengerService = messengerService;
        _userRepository = userRepository;
    }

    public BotState State => BotState.Register;
    public async Task Process(Update update, BotStatus botStatus)
    {
        var text = update.Message!.Text!;
        var sha = SHA512.Create();
        var data = botStatus.ChatId + text;
        var hashed = Convert.ToBase64String(sha.ComputeHash(Encoding.Default.GetBytes(data)));
        botStatus.Token = hashed;

        User user;
        try
        {
            user = User.CreateSimple(new CreateSimpleUserDto(text), _userRepository.Query(_ => true));
        }
        catch (DuplicateException e)
        {
            user = e.GetExistingObject<User>()!;
            if (user.BotStatuses.All(x => x.Id != botStatus.Id))
            {
                throw new BusinessException("Такой пользователь уже существует");
            }
        }
        
        var emailResult = await _emailService.SendEmail(new EmailRequest(
            $"Ваш уникальный токен: {hashed}", "Регистрация FTM", text));

        if (emailResult.Success)
        {
            botStatus.User = user;
            botStatus.State = BotState.Auth;
            await _messengerService.SendMessage(botStatus.ChatId,
                $"Пожалуйста, введите токен, отправленный на почту {text}. Обязательно проверьте папку 'Спам'!");
        }
        else
        {
            await _messengerService.SendMessage(botStatus.ChatId, "Не удалось отправить сообщение");
            if (emailResult.Exception is not null)
            {
                throw new BusinessException($"Ошибка отправки почты: {emailResult.Error}", emailResult.Exception);
            }

            throw new BusinessException($"Ошибка отправки почты: {emailResult.Error ?? "Неизвестная ошибка"}");
        }
    }
}