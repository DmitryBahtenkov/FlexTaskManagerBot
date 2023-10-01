using FTM.Domain.Models.IssueFileModel;

namespace FTM.Domain.Models.IssueModel.DTO;

public class CreateIssueResult
{
    public Issue Issue { get; set; }
    public IssueFile? IssueFile { get; set; }
    public string? TempIssueId { get; set; }
    public List<DateVariant> Variants { get; set; } = new(0);

    public IEnumerable<DateVariant> GetVariants(int timezone)
    {
        return Variants.Select(variant => variant with { Date = variant.Date.AddHours(timezone) });
    }

    public bool NeedForSave { get; set; } = true;
}