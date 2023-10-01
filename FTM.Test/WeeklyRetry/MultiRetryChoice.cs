using FTM.Domain.Helpers;
using FTM.Infrastructure.BotImplementation.Services;

namespace FTM.Test.WeeklyRetry
{
    public class MultiRetryChoice
    {
        [Fact]
        public void MultiChoice_OnlyPassed()
        {
            var selectedDays = new[] { RuDayOfWeek.Tuesday, RuDayOfWeek.Wednesday, RuDayOfWeek.Monday };
            var today = RuDayOfWeek.Friday;

            var expectedNextRetryInDays = 3;
            var expectedNextRetryDay = ((int)RuDayOfWeek.Monday);

            var result = RetryService.GetRetryPeriods(selectedDays, today);

            Assert.Equal(expectedNextRetryDay, result.retryWeekday);
            Assert.Equal(expectedNextRetryInDays, result.nextRetryInDays);
        }

        [Fact]
        public void MultiChoice_ThisWeek()
        {
            var selectedDays = new[] { RuDayOfWeek.Tuesday, RuDayOfWeek.Wednesday, RuDayOfWeek.Sunday};
            var today = RuDayOfWeek.Friday;

            var expectedNextRetryInDays = 2;
            var expectedNextRetryDay = ((int)RuDayOfWeek.Sunday);

            var result = RetryService.GetRetryPeriods(selectedDays, today);

            Assert.Equal(expectedNextRetryDay, result.retryWeekday);
            Assert.Equal(expectedNextRetryInDays, result.nextRetryInDays);
        }

        [Fact]
        public void MultiChoice_Everyday()
        {
            var selectedDays = Enum.GetValues<RuDayOfWeek>();
            var today = RuDayOfWeek.Thursday;

            var expectedNextRetryInDays = 1;
            var expectedNextRetryDay = ((int)RuDayOfWeek.Friday);

            var result = RetryService.GetRetryPeriods(selectedDays, today);

            Assert.Equal(expectedNextRetryDay, result.retryWeekday);
            Assert.Equal(expectedNextRetryInDays, result.nextRetryInDays);
        }

        [Fact]
        public void MultiChoice_Nearest_is_SameWeekDay_as_Today()
        {
            var selectedDays = new[] { RuDayOfWeek.Sunday };
            var today = RuDayOfWeek.Sunday;

            var expectedNextRetryInDays = 7;
            var expectedNextRetryDay = ((int)RuDayOfWeek.Sunday);

            var result = RetryService.GetRetryPeriods(selectedDays, today);

            Assert.Equal(expectedNextRetryDay, result.retryWeekday);
            Assert.Equal(expectedNextRetryInDays, result.nextRetryInDays);
        }

        [Fact]
        public void MultiChoice_has_SameWeekDay_as_Today()
        {
            var selectedDays = new[] { RuDayOfWeek.Sunday, RuDayOfWeek.Tuesday, RuDayOfWeek.Friday };
            var today = RuDayOfWeek.Sunday;

            var expectedNextRetryInDays = 2;
            var expectedNextRetryDay = ((int)RuDayOfWeek.Tuesday);

            var result = RetryService.GetRetryPeriods(selectedDays, today);

            Assert.Equal(expectedNextRetryDay, result.retryWeekday);
            Assert.Equal(expectedNextRetryInDays, result.nextRetryInDays);
        }
    }
}