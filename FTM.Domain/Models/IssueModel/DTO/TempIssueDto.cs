using FTM.Domain.Models.IssueFileModel;

namespace FTM.Domain.Models.IssueModel.DTO;

public class TempIssueDto
{
    public string Id { get; set; }
    public Issue IssueForSaving { get; set; }
    public List<DateVariant> Variants { get; set; } = new(0);
    public IssueFile? IssueFile {get;set;}
}

public record DateVariant(string Text, DateTime Date, bool HasTime);