namespace FTM.Domain.Units;

public interface IPipeline<THandler, TInput> where THandler : IPipelineHandler<TInput>
{
    public IEnumerable<THandler> PipelineHandlers { get; }

    public Task ExecutePipeline(TInput input);
}