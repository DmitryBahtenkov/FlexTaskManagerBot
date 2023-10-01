namespace FTM.Domain.Models.Base;

public interface IEntityWithUserId : IEntityModel
{
    public int? UserId { get; set; }
}