using System.Diagnostics.CodeAnalysis;
using FTM.Domain.Models.IssueModel.DTO;

namespace FTM.Domain.Services.TextParser;

public interface ITextParser
{
    public DateParseResult ParseDate(string text, ParserSettings settings = default);
}

public record struct ParserSettings(bool IgnoreSingleTokens = false);

public record DateParseResult(DateParserStatus Status,
    List<DateVariant> Variants)
{
    public static DateParseResult None => new(DateParserStatus.None, new List<DateVariant>(0));
}

public enum DateParserStatus
{
    Found,
    None
}
