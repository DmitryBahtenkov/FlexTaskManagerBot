using FTM.Domain.Models.Base;
using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.Models.SettingsModel;

namespace FTM.Domain.Models.UserModel;

public partial class User : BaseEntity
{
    public string Email { get; set; }
    public string? Token { get; set; }
    public string? UserName { get; set; }
    public string? PasswordHash { get; set; }
    public string? PasswordSalt { get; set; }
    public virtual ICollection<Issue> Issues { get; set; }
    public virtual ICollection<Settings> Settings { get; set; }
    public virtual ICollection<BotStatus> BotStatuses { get; set; }

    public override BaseData ToData()
    {
        return new UserData(Id, Email, UserName);
    }
}