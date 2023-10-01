using FTM.Domain.Units;

namespace FTM.Domain.Models.IssueModel.Pipelines;

public class IssuePipeline : IPipeline<ICreateIssuePipelineHandler, Issue>
{
    public IssuePipeline(IEnumerable<ICreateIssuePipelineHandler> pipelineHandlers)
    {
        PipelineHandlers = pipelineHandlers;
    }

    public IEnumerable<ICreateIssuePipelineHandler> PipelineHandlers { get; }
    
    public async Task ExecutePipeline(Issue input)
    {
        foreach (var handler in PipelineHandlers)
        {
            await handler.Handle(input);
        }
    }
}