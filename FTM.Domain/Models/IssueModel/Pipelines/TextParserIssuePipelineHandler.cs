using FTM.Domain.Services;
using FTM.Domain.Services.TextParser;

namespace FTM.Domain.Models.IssueModel.Pipelines;

public class TextParserIssuePipelineHandler : ICreateIssuePipelineHandler
{
    private readonly ITextParser _textParser;
    private readonly ICurrentUserService _currentUserService;

    public TextParserIssuePipelineHandler(ITextParser textParser, ICurrentUserService currentUserService)
    {
        _textParser = textParser;
        _currentUserService = currentUserService;
    }

    public Task Handle(Issue input)
    {

        return Task.CompletedTask;
    }
}