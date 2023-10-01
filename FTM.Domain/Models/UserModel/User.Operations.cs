using FTM.Domain.Exceptions;
using FTM.Domain.Models.Base;
using FTM.Domain.Models.SettingsModel;
using FTM.Domain.Models.UserModel.DTO;
using Mapster;

namespace FTM.Domain.Models.UserModel;

public partial class User
{
    public static User Create(CreateUserDto createUserDto, IQueryable<User> users)
    {
        createUserDto.Validate();
        Deduplicate(createUserDto, users);
        return createUserDto.Adapt<User>();
    }

    public static User CreateSimple(CreateSimpleUserDto createSimpleUserDto, IQueryable<User> users)
    {
        ((IValidatable)createSimpleUserDto).Validate();
        Deduplicate(createSimpleUserDto, users);

        return new User
        {
            Email = createSimpleUserDto.Email,
            Settings = new List<Settings>(1) {Models.SettingsModel.Settings.Default}
        };
    }

    private static void Deduplicate(CreateUserDto createUserDto, IQueryable<User> users)
    {
        var user = users.FirstOrDefault(x => x.Email == createUserDto.Email || x.UserName == createUserDto.UserName);
        if (user is not null)
        {
            throw new DuplicateException(user, "Такой пользователь уже существует");
        }
    }
    
    private static void Deduplicate(CreateSimpleUserDto createUserDto, IQueryable<User> users)
    {
        var user = users.FirstOrDefault(x => x.Email == createUserDto.Email);
        if (user is not null)
        {
            throw new DuplicateException(user, "Такой пользователь уже существует");
        }
    }

    public void CreateToken(params string[] parts)
    {
        var data = string.Join('#', parts);
    }
}