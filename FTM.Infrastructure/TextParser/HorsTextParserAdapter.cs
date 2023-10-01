using FTM.Domain.Models.IssueModel.DTO;
using FTM.Domain.Services;
using FTM.Domain.Services.TextParser;
using Hors;
using Hors.Models;
using Microsoft.Extensions.Logging;

namespace FTM.Infrastructure.TextParser;

public class HorsTextParserAdapter : ITextParser
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<HorsTextParserAdapter> _logger;

    public HorsTextParserAdapter(
        ICurrentUserService currentUserService, 
        ILogger<HorsTextParserAdapter> logger)
    {
        _currentUserService = currentUserService;
        _logger = logger;
    }

    public DateParseResult ParseDate(string text,  ParserSettings settings = default)
    {
        var timeZone = _currentUserService.User.Settings.First().Timezone ?? 3;
        var hors = new HorsTextParser();
        var parseResult = hors.Parse(text, DateTime.UtcNow, ignoreSingleTokens: settings.IgnoreSingleTokens);
        if (!parseResult.Dates.Any())
        {
            return DateParseResult.None;
        }

        var variants = new List<DateVariant>(parseResult.Dates.Count);
        for (var i = 0; i < parseResult.Dates.Count; i++)
        {
            var variant = CreateVariant(parseResult, parseResult.Dates[i], i, timeZone);
            if (variant is not null)
            {
                variants.Add(variant);
            }
        }

        return new DateParseResult(DateParserStatus.Found, variants);
    }

    private DateVariant? CreateVariant(HorsParseResult parseResult, DateTimeToken token, int index, int timeZone)
    {
        try
        {
            if (!token.HasTime)
            {
                // пока что всегда на 9 утра
                token.DateFrom = token.DateFrom.AddHours(9);
            }
            return new DateVariant(parseResult.CreateText(index), token.DateFrom.AddHours(-timeZone), true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to parse date for token {token}", token);
            return default;
        }
    }
}