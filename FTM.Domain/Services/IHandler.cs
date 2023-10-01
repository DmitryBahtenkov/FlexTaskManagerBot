using FTM.Domain.Models.Base;

namespace FTM.Domain.Services;

public interface IHandler<in TUpdate, TEntity> where TEntity : IEntityModel
{
    public string Command { get; }
    public Task Handle(TUpdate update, TEntity botStatus);
    public bool Contains(string text);
}