using FTM.Domain.Models.IssueModel;
using FTM.Domain.Models.IssueModel.DTO;
using FTM.Domain.Models.IssueModel.Pipelines;
using FTM.Domain.Units;

namespace FTM.Domain.Services.Issues;

public class CreateIssueCommand
{
    private readonly IUnitOfWork<Issue> _unitOfWork;
    private readonly IssueDateTimeChainHandler _issueDateTimeChainHandler;
    
    public CreateIssueCommand(
        IUnitOfWork<Issue> unitOfWork,
        IssueDateTimeChainHandler issueDateTimeChainHandler)
    {
        _unitOfWork = unitOfWork;
        _issueDateTimeChainHandler = issueDateTimeChainHandler;
    }

    public async Task<CreateIssueResult> Create(CreateIssueDto createIssueDto, int userId, string? key = null)
    {
        var issue = Issue.Create(createIssueDto, userId);

        var chainExecutor = new ChainExecutor<CreateIssueResult>(_issueDateTimeChainHandler);
        var toProcess = new CreateIssueResult
        {
            Issue = issue,
            IssueFile = createIssueDto.IssueFile,
            TempIssueId = key
        };
        await chainExecutor.Execute(toProcess);

        if (toProcess.NeedForSave)
        {
            var entity = await _unitOfWork.GetRepository().AddAsync(toProcess.Issue);
            toProcess.Issue = entity;
            await _unitOfWork.SaveChangesAsync();
        }

        return toProcess;
    }
}