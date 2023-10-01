using System.Text.Json.Serialization;

namespace FTM.Telegram;

public record CheckResult(bool Ok, DateTime Date)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Error { get; init; }
}