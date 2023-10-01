using FTM.Domain.Models.Base;

namespace FTM.Domain.Models.UserModel;

public record UserData(int Id, string Email, string UserName) : BaseData(Id); 