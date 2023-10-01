using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using FTM.Domain.Helpers;
using FTM.Domain.Models.Base;
using FTM.Domain.Models.IssueModel;

namespace FTM.Domain.Models.IssueFileModel
{
    public class IssueFile
    {
        public string FileId { get; set; }
        public MediaType Type { get; set; }

    }
}