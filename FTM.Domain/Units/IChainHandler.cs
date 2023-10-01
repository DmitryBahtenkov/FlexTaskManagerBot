namespace FTM.Domain.Units;

public interface IChainHandler<in TInput>
{
    public IChainHandler<TInput>? Next { get; } 
    public Task Handle(TInput input);
}