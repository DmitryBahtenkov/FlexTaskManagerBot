using FTM.Domain.Helpers;

namespace FTM.Domain.Models.IssueModel
{
    public class RetrySettings
    {
        public RuDayOfWeek[]? RetryWeekDays { get; set; } 
        public int CurrentRetryPeriod { get; set; }
        public int CurrentRetryWeekday {get;set;}
    }
}