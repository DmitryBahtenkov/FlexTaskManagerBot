using FTM.Domain.Models.IssueModel;
using FTM.Domain.Models.IssueModel.DTO;
using FTM.Domain.Services.TextParser;
using FTM.Domain.Units;

namespace FTM.Domain.Services.Issues;

public class TextParserChainHandler : IChainHandler<CreateIssueResult>
{
    public IChainHandler<CreateIssueResult>? Next { get; private set; }
    
    private readonly ITextParser _textParser;
    private readonly ICurrentUserService _currentUserService;
    
    private readonly IssueFolderChainHandler _issueFolderChainHandler;
    private readonly ICacheService _cacheService;

    public TextParserChainHandler(
        ITextParser textParser,
        ICurrentUserService currentUserService, 
        IssueFolderChainHandler issueFolderChainHandler, 
        ICacheService cacheService)
    {
        _textParser = textParser;
        _currentUserService = currentUserService;
        _issueFolderChainHandler = issueFolderChainHandler;
        _cacheService = cacheService;
    }

    public async Task Handle(CreateIssueResult input)
    {
        var textResult = _textParser.ParseDate(input.Issue.Text);
        if (textResult.Status == DateParserStatus.None)
        {
            return;
        }

        var tempDto = new TempIssueDto()
        {
            Variants = textResult.Variants,
            IssueForSaving = input.Issue,
            IssueFile = input.IssueFile,
            // todo: всё упадёт ну да пофиг пока что
            Id = input.TempIssueId ?? Guid.NewGuid().ToString()
        };
        
        input.NeedForSave = false;
        input.Variants = textResult.Variants;
        
        await _cacheService.CreateEntry(
            tempDto.Id,
            () => Task.FromResult(tempDto),
            TimeSpan.FromMinutes(30));
    }
}