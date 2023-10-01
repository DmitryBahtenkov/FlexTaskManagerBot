using FTM.Domain.Models.IssueModel;
using FTM.Domain.Models.IssueModel.DTO;
using FTM.Domain.Units;

namespace FTM.Domain.Services.Issues;

public class IssueFolderChainHandler : IChainHandler<CreateIssueResult>
{
    public IChainHandler<CreateIssueResult>? Next { get; }

    public Task Handle(CreateIssueResult input)
    {
        var text = input.Issue.Text;
        var splitted = text.Split(':');
        var folder = "Без категории";

        if (text.Contains(':') && splitted.Any())
        {
            folder = splitted.First();
            text = text.Replace(folder + ":", string.Empty).Trim();
        }

        input.Issue.Text = text;
        input.Issue.Folder = folder;
        
        return Task.CompletedTask;
    }
}