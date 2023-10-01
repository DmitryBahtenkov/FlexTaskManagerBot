using FTM.Domain.Models.SettingsModel;
using FTM.Domain.Models.UserModel;
using FTM.Domain.Services;

namespace FTM.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    public User? User { get; private set; }
    public int? UserId { get; private set; }
    public int? Timezone { get; private set; }

    public void Set(User user)
    {
        User = user;
        UserId = user.Id;
        Timezone = user.Settings.First().Timezone;
    }

    public Settings? GetSettings()
    {
        return User?.Settings.FirstOrDefault();
    }
}