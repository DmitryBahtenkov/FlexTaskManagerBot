using FTM.Domain.Models.Base;
using FTM.Domain.Models.IssueFileModel;

namespace FTM.Domain.Models.IssueModel;

public record IssueData(int Id, string Text, string? Note, int? UserId, DateTime? RemindTime, IssueFile? IssueFile) : BaseData(Id);