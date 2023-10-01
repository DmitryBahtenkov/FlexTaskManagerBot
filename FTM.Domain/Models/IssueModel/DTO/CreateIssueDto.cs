using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using FTM.Domain.Models.Base;
using FTM.Domain.Models.IssueFileModel;

namespace FTM.Domain.Models.IssueModel.DTO;

public record CreateIssueDto(
    [Required] string Text,
    string? Note,
    DateTime? Deadline,
    DateTime? RemindTime,
    int? RetryDaysCount,
    Step[] Steps,
    string Folder) : IValidatable
{
    public IssueFile? IssueFile { get; set; }

    public static CreateIssueDto TextOnly(string text)
    {
        var folder = "Без категории";

        return new(text, null, null, null, null, Array.Empty<Step>(), folder);
    }
}