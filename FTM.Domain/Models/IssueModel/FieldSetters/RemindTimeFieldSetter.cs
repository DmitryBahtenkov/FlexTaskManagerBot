using System.Globalization;
using FTM.Domain.Exceptions;
using FTM.Domain.Models.UserModel;
using FTM.Domain.Services;
using FTM.Domain.Services.TextParser;
using FTM.Domain.Units;

namespace FTM.Domain.Models.IssueModel.FieldSetters;

public class RemindTimeFieldSetter : IFieldSetter<Issue>
{
    private readonly ITextParser _textParser;
    private readonly ICurrentUserService _currentUserService;

    public RemindTimeFieldSetter(ITextParser textParser, ICurrentUserService currentUserService)
    {
        _textParser = textParser;
        _currentUserService = currentUserService;
    }

    public void Set(Issue entity, string fieldName, object value, Dictionary<string, object>? parameters = null)
    {
        if (value is not string str)
        {
            return;
        }

        // если смогли распарсить дату в обычном формате - сохраняем
        if (DateTime.TryParseExact(
                str,
                "dd.MM.yyyy HH:mm",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var resultDate))
        {
            var tz = _currentUserService.Timezone ?? 3;
            entity.RemindTime = resultDate.AddHours(-tz);
            return;
        }

        var date = _textParser.ParseDate(str, new ParserSettings(true));

        if (date.Status == DateParserStatus.None)
        {
            throw new BusinessException("Не удалось распарсить дату. Введите значение снова");
        }

        var variant = date.Variants.First();
        
        if (!variant.HasTime)
        {
            throw new BusinessException("Не удалось распарсить время. Введите значение снова");
        }
        
        entity.RemindTime = variant.Date;
    }
}