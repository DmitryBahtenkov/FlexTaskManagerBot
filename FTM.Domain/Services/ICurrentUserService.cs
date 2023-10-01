using FTM.Domain.Models.SettingsModel;
using FTM.Domain.Models.UserModel;

namespace FTM.Domain.Services;

public interface ICurrentUserService
{
    public User User { get; }
    public int? UserId { get; }
    public int? Timezone { get; }
    public void Set(User user);
    public Settings? GetSettings();
}