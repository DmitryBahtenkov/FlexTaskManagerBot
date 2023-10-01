namespace FTM.Domain.Models.IssueModel.Pipelines;

public class FolderIssuePipelineHandler : ICreateIssuePipelineHandler
{
    public Task Handle(Issue input)
    {
        var text = input.Text;
        var splitted = text.Split(':');
        var folder = "Без категории";

        if (text.Contains(':') && splitted.Any())
        {
            folder = splitted.First();
            text = text.Replace(folder + ":", string.Empty).Trim();
        }

        input.Text = text;
        input.Folder = folder;
        
        return Task.CompletedTask;
    }
}