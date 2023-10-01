using FTM.Domain.Models.IssueModel.DTO;
using FTM.Domain.Units;

namespace FTM.Domain.Services.Issues;

public class IssueDateTimeChainHandler : IChainHandler<CreateIssueResult>
{
    public IChainHandler<CreateIssueResult>? Next { get; private set; }

    private readonly TextParserChainHandler _textParserChainHandler;
    
    public IssueDateTimeChainHandler(
        TextParserChainHandler textParserChainHandler)
    {
        _textParserChainHandler = textParserChainHandler;
    }
    
    public Task Handle(CreateIssueResult input)
    {
        Next = _textParserChainHandler;
        
        return Task.CompletedTask;
    }
}