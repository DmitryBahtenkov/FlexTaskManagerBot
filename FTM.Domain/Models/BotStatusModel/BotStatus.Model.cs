using System.ComponentModel.DataAnnotations.Schema;
using FTM.Domain.Models.Base;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.Models.UserModel;

namespace FTM.Domain.Models.BotStatusModel;

public class BotStatus : BaseEntity, IEntityWithUserId
{
    public int? UserId { get; set; }
    public string? Token { get; set; }
    public string ChatId { get; set; }
    [Column(TypeName = "jsonb")]
    public IssueFilter? Filter { get; set; }
    public int? EntityId { get; set; }
    public string? EditingField { get; set; }
    public BotState State { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }
    
    public override BaseData ToData()
    {
        throw new NotImplementedException();
    }
}

public enum BotState
{
    Auth = 0,
    ListenTasks = 1,
    UpdatingTask = 2,
    UpdatingTaskReminder = 3,
    Register = 4,
    RegisterCode = 5,
    Started = 6
}