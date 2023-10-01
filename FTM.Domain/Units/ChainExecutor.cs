namespace FTM.Domain.Units;

public class ChainExecutor<TInput>
{
    private readonly IChainHandler<TInput> _initialHandler;

    public ChainExecutor(IChainHandler<TInput> initialHandler)
    {
        _initialHandler = initialHandler;
    }

    public Task Execute(TInput input)
    {
        return ExecuteInternal(_initialHandler, input);
    }

    private async Task ExecuteInternal(IChainHandler<TInput> handler, TInput input)
    {
        await handler.Handle(input);

        if (handler.Next is not null)
        {
            await ExecuteInternal(handler.Next, input);
        }
    }
}