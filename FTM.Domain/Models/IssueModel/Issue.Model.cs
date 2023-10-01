using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using FTM.Domain.Models.Base;
using FTM.Domain.Models.IssueFileModel;
using FTM.Domain.Models.IssueModel.FieldSetters;
using FTM.Domain.Models.UserModel;
using FTM.Domain.Units;

namespace FTM.Domain.Models.IssueModel;

public partial class Issue : BaseEntity, IEntityWithUserId
{
    public string Text { get; set; }
    public string? Note { get; set; }
    public IssueStatus Status { get; set; }
    [FieldSetter(typeof(RemindTimeFieldSetter))]
    [Column(TypeName = "timestamp without time zone")]
    public DateTime? RemindTime { get; set; }
    public string Folder { get; set; }
    [Column(TypeName = "jsonb")] 
    public RetrySettings? RetrySettings{get;set;} 
    public int? UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    [JsonIgnore]
    public virtual User User { get; set; }
     [Column(TypeName = "jsonb")] 
    public IssueFile? IssueFile { get; set; }
    
    public override BaseData ToData()
    {
        return new IssueData(Id, Text, Note, UserId, RemindTime, IssueFile);
    }
}

public class Step
{
    public bool Done { get; set; }
    public string Text { get; set; }
}

public enum IssueStatus
{
    [Display(Name = "Не выполнено")]
    Started = 0,
    [Display(Name = "Выполнено")]
    Finished = 1
}