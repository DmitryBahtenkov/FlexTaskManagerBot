namespace FTM.Domain.Units;

public interface IPipelineHandler<in TInput>
{
    public Task Handle(TInput input);
}