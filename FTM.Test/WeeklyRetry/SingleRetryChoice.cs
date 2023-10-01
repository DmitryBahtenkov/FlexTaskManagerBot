using FTM.Domain.Helpers;
using FTM.Infrastructure.BotImplementation.Services;

namespace FTM.Test.WeeklyRetry;

public class SingleRetryChoice
{
    [Fact]
    public void SingleChoice_Passed_WeekDay()
    {
        var selectedDays = new[] {RuDayOfWeek.Tuesday};
        var today = RuDayOfWeek.Friday;

        var expectedNextRetryInDays = 4;
        var expectedNextRetryDay = ((int)RuDayOfWeek.Tuesday);

        var result = RetryService.GetRetryPeriods(selectedDays, today);

        Assert.Equal(expectedNextRetryDay, result.retryWeekday);
        Assert.Equal(expectedNextRetryInDays, result.nextRetryInDays);
    }

    [Fact]
    public void SingleChoice_SameWeekDay_as_Today()
    {
        var selectedDays = new[] {RuDayOfWeek.Sunday};
        var today = RuDayOfWeek.Sunday;

        var expectedNextRetryInDays = 7;
        var expectedNextRetryDay = ((int)RuDayOfWeek.Sunday);

        var result = RetryService.GetRetryPeriods(selectedDays, today);

        Assert.Equal(expectedNextRetryDay, result.retryWeekday);
        Assert.Equal(expectedNextRetryInDays, result.nextRetryInDays);
    }

    [Fact]
    public void SingleChoice_Tomorrow()
    {
        var selectedDays = new[] {RuDayOfWeek.Friday};
        var today = RuDayOfWeek.Thursday;

        var expectedNextRetryInDays = 1;
        var expectedNextRetryDay = ((int)RuDayOfWeek.Friday);

        var result = RetryService.GetRetryPeriods(selectedDays, today);

        Assert.Equal(expectedNextRetryDay, result.retryWeekday);
        Assert.Equal(expectedNextRetryInDays, result.nextRetryInDays);
    }

    [Fact]
    public void SingleChoice_ThisWeek()
    {
        var selectedDays = new[] {RuDayOfWeek.Friday};
        var today = RuDayOfWeek.Tuesday;

        var expectedNextRetryInDays = 3;
        var expectedNextRetryDay = ((int)RuDayOfWeek.Friday);

        var result = RetryService.GetRetryPeriods(selectedDays, today);

        Assert.Equal(expectedNextRetryDay, result.retryWeekday);
        Assert.Equal(expectedNextRetryInDays, result.nextRetryInDays);
    }
}