using System;
using Xunit;
using Scheduler;
using FluentAssertions;

namespace TestScheduler
{
    public class SchedulerTest
    {
        #region Validations
        [Fact]
        public void Schedule_DateTimeOnceNullValidation()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Once;
            configuration.DateTimeOnce = null;

            Assert.Throws<ConfigurationException>(() => Schedule.GetNextExecutionTime(DateTime.Today, configuration));
        }

        [Fact]
        public void Schedule_DateTimeIsMaxValueValidation()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Once;
            configuration.DateTimeOnce = DateTime.MaxValue;

            Assert.Throws<ConfigurationException>(() => Schedule.GetNextExecutionTime(DateTime.Today, configuration));
        }

        [Fact]
        public void Schedule_NextExecution_Once()
        {
            DateTime dateTime = new DateTime(2020, 01, 08);


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Once;
            configuration.DateTimeOnce = dateTime;


            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 01), configuration).Should().Be(new DateTime(2020, 01, 08));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 05), configuration).Should().Be(new DateTime(2020, 01, 08));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 10), configuration).Should().Be(new DateTime(2020, 01, 10));
            Schedule.GetDescription(new DateTime(2020, 01, 01, 12, 55, 00), configuration).Should()
                .Be("Occurs once. Schedule will be used on 08/01/2020 at 0:00");
        }

        [Fact]
        public void Schedule_Recurring_StartDateMaxValueValidation()
        {
            DateTime startDate = DateTime.MaxValue;
            DateTime currentDate = new DateTime(2020, 01, 01);


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;


            Assert.Throws<ConfigurationException>(() => Schedule.GetNextExecutionTime(currentDate, configuration));
        }

        [Fact]
        public void Schedule_Recurring_StartEndDateMaxValueValidation()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DateTime endDate = DateTime.MaxValue;
            DateTime currentDate = new DateTime(2020, 01, 01);


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.EndDate = endDate;


            Assert.Throws<ConfigurationException>(() => Schedule.GetNextExecutionTime(currentDate, configuration));
        }

        [Fact]
        public void Schedule_Recurring_StartEndGreaterThanEndDateValidation()
        {
            DateTime startDate = new DateTime(2020, 01, 10);
            DateTime endDate = new DateTime(2020, 01, 01);
            DateTime currentDate = new DateTime(2020, 01, 01);


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.EndDate = endDate;


            Assert.Throws<ConfigurationException>(() => Schedule.GetNextExecutionTime(currentDate, configuration));
        }

        [Fact]
        public void Schedule_Recurring_IntervalLessThanZeroValidation()
        {
            DateTime startDate = new DateTime(2020, 01, 10);
            DateTime endDate = new DateTime(2020, 01, 01);
            DateTime currentDate = new DateTime(2020, 01, 01);


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.EndDate = endDate;


            Assert.Throws<ConfigurationException>(() => Schedule.GetNextExecutionTime(currentDate, configuration));
        }

        [Fact]
        public void Schedule_Recurring_Daily_NullValidation()
        {
            DateTime startDate = new DateTime(2020, 01, 10);
            DateTime endDate = new DateTime(2020, 01, 01);
            DateTime currentDate = new DateTime(2020, 01, 01);


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.EndDate = endDate;


            Assert.Throws<ConfigurationException>(() => Schedule.GetNextExecutionTime(currentDate, configuration));
        }

        [Fact]
        public void ScheduleWeeklyConfiguration_NullValidation()
        {
            DateTime currentDate = new DateTime(2020, 01, 01, 4, 15, 0);
            DateTime startDate = new DateTime(2020, 01, 01);
            DateTime endDate = new DateTime(2020, 02, 01);


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.Frequency = FrequencyType.Weekly;
            configuration.StartDate = startDate;
            configuration.EndDate = endDate;

            Assert.Throws<ConfigurationException>(() => Schedule.GetNextExecutionTime(currentDate, configuration));
        }

        [Fact]
        public void ScheduleWeeklyConfiguration_NoDaysSpecified()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(2, 0, 0)
            };

            WeeklyFrequency weeklyFreq = new WeeklyFrequency()
            {
                DaysOfWeek = new DayOfWeek[] {},
                Occurrence = 1
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Weekly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = weeklyFreq;

            Assert.Throws<ConfigurationException>(() => Schedule.GetNextExecutionTime(new DateTime(2020,01,01), configuration));
        }

        [Fact]
        public void ScheduleWeeklyConfiguration_NoValidOccurrence()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(2, 0, 0)
            };

            WeeklyFrequency weeklyFreq = new WeeklyFrequency()
            {
                DaysOfWeek = new DayOfWeek[] {DayOfWeek.Monday},
                Occurrence = 0
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Weekly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = weeklyFreq;

            Assert.Throws<ConfigurationException>(() => Schedule.GetNextExecutionTime(new DateTime(2020, 01, 01), configuration));
        }

        [Fact]
        public void ScheduleWeeklyConfiguration_NoWeeklyFrequencySpecified()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(2, 0, 0)
            };

            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Weekly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = null;

            Assert.Throws<ConfigurationException>(() => Schedule.GetNextExecutionTime(new DateTime(2020, 01, 01), configuration));
        }

        [Fact]
        public void ScheduleMonthlyConfiguration_NoMonthlyFrequencySpecified()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(2, 0, 0)
            };

            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = null;

            Assert.Throws<ConfigurationException>(() => Schedule.GetNextExecutionTime(new DateTime(2020, 01, 01), configuration));
        }

        [Fact]
        public void ScheduleMonthlyConfiguration_NoValidDayNumber()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(2, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = true,
                DayNumber = 0
            };

            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;

            Assert.Throws<ConfigurationException>(() => Schedule.GetNextExecutionTime(new DateTime(2020, 01, 01), configuration));
        }

        [Fact]
        public void ScheduleMonthlyConfiguration_NoValidInterval()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(2, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 0,
                DayType = MonthlyDayType.Day,
                Frequency = MonthlyFrequencyType.First
            };

            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;

            Assert.Throws<ConfigurationException>(() => Schedule.GetNextExecutionTime(new DateTime(2020, 01, 01), configuration));
        }

        [Fact]
        public void ScheduleMonthlyConfiguration_NoValidFrequency()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(2, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 1,
                DayType = MonthlyDayType.Day
            };

            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;

            Assert.Throws<ConfigurationException>(() => Schedule.GetNextExecutionTime(new DateTime(2020, 01, 01), configuration));
        }

        [Fact]
        public void ScheduleMonthlyConfiguration_NoValidDay()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(2, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 1,
                Frequency = MonthlyFrequencyType.First
            };

            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;

            Assert.Throws<ConfigurationException>(() => Schedule.GetNextExecutionTime(new DateTime(2020, 01, 01), configuration));
        }


        [Fact]
        public void ScheduleMultipleExecutionTimes_NoNumberOfTimes()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(2, 0, 0)
            };

            WeeklyFrequency weeklyFreq = new WeeklyFrequency()
            {
                DaysOfWeek = new DayOfWeek[] { DayOfWeek.Monday },
                Occurrence = 1
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Weekly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = weeklyFreq;

            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 08, 04, 0, 10), configuration, 0);

            executionTimes.Length.Should().Be(0);
        }

        [Fact]
        public void ScheduleMultipleExecutionTimes_OneNumberOfTimes()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(2, 0, 0)
            };

            WeeklyFrequency weeklyFreq = new WeeklyFrequency()
            {
                DaysOfWeek = new DayOfWeek[] { DayOfWeek.Monday },
                Occurrence = 1
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Weekly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = weeklyFreq;

            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 08, 04, 0, 10), configuration, 1);

            executionTimes.Length.Should().Be(1);
        }
        #endregion
        #region Daily
        [Fact]
        public void ScheduleNextExecution_Recurring_Daily_NonRecurring()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                IsRecurring = false,
                Occurrence = new TimeSpan(21, 15, 15)
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;



            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 08, 04, 0, 10), configuration, 6);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 08, 21, 15, 15));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 09, 21, 15, 15));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 10, 21, 15, 15));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 11, 21, 15, 15));
            executionTimes[4].Should().Be(new DateTime(2020, 01, 12, 21, 15, 15));
            executionTimes[5].Should().Be(new DateTime(2020, 01, 13, 21, 15, 15));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs everyday once at 21:15:15 starting on 08/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Daily_WithSecondsBeforeStart()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(8, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(0, 0, 10)
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;



            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 01, 0, 0, 0), configuration).Should().Be(new DateTime(2020, 01, 08, 4, 0, 0));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 01, 10, 0, 0), configuration).Should().Be(new DateTime(2020, 01, 08, 4, 0, 0));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 01,0,10,0), configuration).Should().Be(new DateTime(2020, 01, 08, 4, 0, 0));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 01, 0, 0, 10), configuration).Should().Be(new DateTime(2020, 01, 08, 4, 0, 0));
            Schedule.GetDescription(new DateTime(2020, 01, 01, 08, 21, 15), configuration).Should()
                .Be("Occurs everyday every 10 seconds between 04:00:00 and 08:00:00 starting on 08/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Daily_WithSecondsAfterStart()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(8, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(0, 0, 10)
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 08, 04, 0, 10),configuration,6);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 08, 4, 0, 20));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 08, 4, 0, 30));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 08, 4, 0, 40));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 08, 4, 0, 50));
            executionTimes[4].Should().Be(new DateTime(2020, 01, 08, 4, 1, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 01, 08, 4, 1, 10));

            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 08, 04, 0, 10), configuration).Should().Be(new DateTime(2020, 01, 08, 4, 0, 20));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 08, 04, 0, 25), configuration).Should().Be(new DateTime(2020, 01, 08, 4, 0, 30));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 08, 04, 10, 0), configuration).Should().Be(new DateTime(2020, 01, 08, 4, 10, 10));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 08, 04, 10, 15), configuration).Should().Be(new DateTime(2020, 01, 08, 4, 10, 20));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 20, 04, 0, 10), configuration).Should().Be(new DateTime(2020, 01, 20, 4, 0, 20));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 20, 04, 0, 25), configuration).Should().Be(new DateTime(2020, 01, 20, 4, 0, 30));
            Schedule.GetDescription(new DateTime(2020, 01, 01, 08, 21, 15), configuration).Should()
                .Be("Occurs everyday every 10 seconds between 04:00:00 and 08:00:00 starting on 08/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Daily_WithMinutesBeforeStart()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(8, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(0, 20, 0)
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;



            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 01, 0, 0, 0), configuration).Should().Be(new DateTime(2020, 01, 08, 4, 0, 0));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 01, 10, 0, 0), configuration).Should().Be(new DateTime(2020, 01, 08, 4, 0, 0));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 01, 0, 10, 0), configuration).Should().Be(new DateTime(2020, 01, 08, 4, 0, 0));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 01, 0, 0, 10), configuration).Should().Be(new DateTime(2020, 01, 08, 4, 0, 0));
            Schedule.GetDescription(new DateTime(2020, 01, 01, 08, 21, 15), configuration).Should()
                .Be("Occurs everyday every 20 minutes between 04:00:00 and 08:00:00 starting on 08/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Daily_WithMinutesAfterStart()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(8, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(0, 20, 0)
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 08, 04, 0, 0), configuration, 6);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 08, 4, 20, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 08, 4, 40, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 08, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 08, 5, 20, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 01, 08, 5, 40, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 01, 08, 6, 0, 0));

            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 08, 04, 0, 0), configuration).Should().Be(new DateTime(2020, 01, 08, 4, 20, 0));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 08, 04, 40, 0), configuration).Should().Be(new DateTime(2020, 01, 08, 5, 0, 0));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 08, 04, 10, 0), configuration).Should().Be(new DateTime(2020, 01, 08, 4, 20, 0));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 20, 04, 0, 0), configuration).Should().Be(new DateTime(2020, 01, 20, 4, 20, 0));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 20, 04, 0, 15), configuration).Should().Be(new DateTime(2020, 01, 20, 4, 20, 0));
            Schedule.GetDescription(new DateTime(2020, 01, 01, 08, 21, 15), configuration).Should()
                .Be("Occurs everyday every 20 minutes between 04:00:00 and 08:00:00 starting on 08/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Daily_WithHoursBeforeStart()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(8, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;


            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 01, 0, 0, 0), configuration).Should().Be(new DateTime(2020, 01, 08, 4, 0, 0));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 06, 10, 0, 0), configuration).Should().Be(new DateTime(2020, 01, 08, 4, 0, 0));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 05, 0, 10, 0), configuration).Should().Be(new DateTime(2020, 01, 08, 4, 0, 0));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 04, 0, 0, 10), configuration).Should().Be(new DateTime(2020, 01, 08, 4, 0, 0));
            Schedule.GetDescription(new DateTime(2020, 01, 01, 08, 21, 15), configuration).Should()
                .Be("Occurs everyday every 1 hour between 04:00:00 and 08:00:00 starting on 08/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Daily_WithHoursAfterStart()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(10, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 08, 04, 0, 0), configuration, 6);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 08, 5, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 08, 6, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 08, 7, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 08, 8, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 01, 08, 9, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 01, 08, 10, 0, 0));

            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 08, 04, 0, 0), configuration).Should().Be(new DateTime(2020, 01, 08, 5, 0, 0));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 08, 04, 40, 0), configuration).Should().Be(new DateTime(2020, 01, 08, 5, 0, 0));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 08, 04, 10, 0), configuration).Should().Be(new DateTime(2020, 01, 08, 5, 0, 0));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 20, 08, 0, 0), configuration).Should().Be(new DateTime(2020, 01, 20, 9, 0, 0));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 20, 09, 59, 15), configuration).Should().Be(new DateTime(2020, 01, 20, 10, 0, 0));
            Schedule.GetDescription(new DateTime(2020, 01, 01, 08, 21, 15), configuration).Should()
                .Be("Occurs everyday every 1 hour between 04:00:00 and 10:00:00 starting on 08/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Daily_OverEndTime()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(8, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 08, 06, 0, 0), configuration, 6);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 08, 7, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 08, 8, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 09, 4, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 09, 5, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 01, 09, 6, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 01, 09, 7, 0, 0));

            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 08, 04, 0, 0), configuration).Should().Be(new DateTime(2020, 01, 08, 5, 0, 0));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 08, 04, 40, 0), configuration).Should().Be(new DateTime(2020, 01, 08, 5, 0, 0));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 31, 12, 10, 0), configuration).Should().Be(new DateTime(2020, 02, 01, 4, 0, 0));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 20, 14, 55, 0), configuration).Should().Be(new DateTime(2020, 01, 21, 4, 0, 0));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 20, 08, 0, 01), configuration).Should().Be(new DateTime(2020, 01, 21, 4, 0, 0));
            Schedule.GetDescription(new DateTime(2020, 01, 01, 08, 21, 15), configuration).Should()
                .Be("Occurs everyday every 1 hour between 04:00:00 and 08:00:00 starting on 08/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Daily_OverEndDate()
        {
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(8, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = new DateTime(2020, 01, 08);
            configuration.EndDate = new DateTime(2020, 01, 09);
            configuration.DailyFrequency = dailyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 08, 06, 0, 0), configuration, 6);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 08, 7, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 08, 8, 0, 0));
            executionTimes[2].Should().Be(null);
            executionTimes[3].Should().Be(null);
            executionTimes[4].Should().Be(null);
            executionTimes[5].Should().Be(null);
            Schedule.GetDescription(new DateTime(2020, 01, 01, 08, 21, 15), configuration).Should()
                .Be("Occurs everyday every 1 hour between 04:00:00 and 08:00:00 starting on 08/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Daily_NonRecurring_USLocale()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                IsRecurring = false,
                Occurrence = new TimeSpan(21, 15, 15)
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.Culture = "EN-US";


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 08, 04, 0, 10), configuration, 6);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 08, 21, 15, 15));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 09, 21, 15, 15));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 10, 21, 15, 15));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 11, 21, 15, 15));
            executionTimes[4].Should().Be(new DateTime(2020, 01, 12, 21, 15, 15));
            executionTimes[5].Should().Be(new DateTime(2020, 01, 13, 21, 15, 15));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs everyday once at 21:15:15 starting on 01/08/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Daily_OverEndTime_USLocale()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(8, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.Culture = "EN-US";


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 08, 06, 0, 0), configuration, 6);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 08, 7, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 08, 8, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 09, 4, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 09, 5, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 01, 09, 6, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 01, 09, 7, 0, 0));

            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 08, 04, 0, 0), configuration).Should().Be(new DateTime(2020, 01, 08, 5, 0, 0));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 08, 04, 40, 0), configuration).Should().Be(new DateTime(2020, 01, 08, 5, 0, 0));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 31, 12, 10, 0), configuration).Should().Be(new DateTime(2020, 02, 01, 4, 0, 0));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 20, 14, 55, 0), configuration).Should().Be(new DateTime(2020, 01, 21, 4, 0, 0));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 20, 08, 0, 01), configuration).Should().Be(new DateTime(2020, 01, 21, 4, 0, 0));
            Schedule.GetDescription(new DateTime(2020, 01, 01, 08, 21, 15), configuration).Should()
                .Be("Occurs everyday every 1 hour between 04:00:00 and 08:00:00 starting on 01/08/2020");
        }
        #endregion
        #region Weekly
        [Fact]
        public void ScheduleNextExecution_Recurring_Weekly_AllWeekDays_SameDay()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(8, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(0, 20, 0)
            };

            WeeklyFrequency weeklyFreq = new WeeklyFrequency()
            {
                DaysOfWeek = new DayOfWeek[] {DayOfWeek.Monday, DayOfWeek.Tuesday , DayOfWeek.Wednesday , DayOfWeek.Thursday,
                                              DayOfWeek.Friday,DayOfWeek.Saturday,DayOfWeek.Sunday},
                Occurrence = 1
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Weekly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = weeklyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01, 12, 15, 10), configuration, 6);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 08, 4, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 08, 4, 20, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 08, 4, 40, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 08, 5, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 01, 08, 5, 20, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 01, 08, 5, 40, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs every 1 week on monday, tuesday, wednesday, thursday, friday, saturday and sunday every 20 minutes between 04:00:00 and 08:00:00 starting on 08/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Weekly_AllWeekDays_ChangingDay()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(2, 0, 0)
            };

            WeeklyFrequency weeklyFreq = new WeeklyFrequency()
            {
                DaysOfWeek = new DayOfWeek[] {DayOfWeek.Monday, DayOfWeek.Tuesday , DayOfWeek.Wednesday , DayOfWeek.Thursday,
                                              DayOfWeek.Friday,DayOfWeek.Saturday,DayOfWeek.Sunday},
                Occurrence = 1
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Weekly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = weeklyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01, 12, 15, 10), configuration, 6);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 08, 4, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 08, 6, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 09, 4, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 09, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 01, 10, 4, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 01, 10, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs every 1 week on monday, tuesday, wednesday, thursday, friday, saturday and sunday every 2 hours between 04:00:00 and 06:00:00 starting on 08/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Weekly_AllWeekDaysExceptOne_ChangingDay()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(2, 0, 0)
            };

            WeeklyFrequency weeklyFreq = new WeeklyFrequency()
            {
                DaysOfWeek = new DayOfWeek[] {DayOfWeek.Monday, DayOfWeek.Tuesday , DayOfWeek.Wednesday ,
                                              DayOfWeek.Friday,DayOfWeek.Saturday,DayOfWeek.Sunday},
                Occurrence = 1
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Weekly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = weeklyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01, 12, 15, 10), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 08, 4, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 08, 6, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 10, 4, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 10, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 01, 11, 4, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 01, 11, 6, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 01, 12, 4, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 01, 12, 6, 0, 0));
            Schedule.GetDescription(executionTimes[7].Value, configuration).Should()
                .Be("Occurs every 1 week on monday, tuesday, wednesday, friday, saturday and sunday every 2 hours between 04:00:00 and 06:00:00 starting on 08/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Weekly_OnlyMonday()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(2, 0, 0)
            };

            WeeklyFrequency weeklyFreq = new WeeklyFrequency()
            {
                DaysOfWeek = new DayOfWeek[] {DayOfWeek.Monday},
                Occurrence = 1
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Weekly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = weeklyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01, 12, 15, 10), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 13, 4, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 13, 6, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 20, 4, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 20, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 01, 27, 4, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 01, 27, 6, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 02, 03, 4, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 02, 03, 6, 0, 0));
            Schedule.GetDescription(executionTimes[7].Value, configuration).Should()
                .Be("Occurs every 1 week on monday every 2 hours between 04:00:00 and 06:00:00 starting on 08/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Weekly_OnlyTuesday()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(2, 0, 0)
            };

            WeeklyFrequency weeklyFreq = new WeeklyFrequency()
            {
                DaysOfWeek = new DayOfWeek[] { DayOfWeek.Tuesday },
                Occurrence = 1
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Weekly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = weeklyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01, 12, 15, 10), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 14, 4, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 14, 6, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 21, 4, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 21, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 01, 28, 4, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 01, 28, 6, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 02, 04, 4, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 02, 04, 6, 0, 0));
            Schedule.GetDescription(executionTimes[7].Value, configuration).Should()
                .Be("Occurs every 1 week on tuesday every 2 hours between 04:00:00 and 06:00:00 starting on 08/01/2020");

        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Weekly_OnlyWednesday()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(2, 0, 0)
            };

            WeeklyFrequency weeklyFreq = new WeeklyFrequency()
            {
                DaysOfWeek = new DayOfWeek[] { DayOfWeek.Wednesday },
                Occurrence = 1
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Weekly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = weeklyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01, 12, 15, 10), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 08, 4, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 08, 6, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 15, 4, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 15, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 01, 22, 4, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 01, 22, 6, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 01, 29, 4, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 01, 29, 6, 0, 0));
            Schedule.GetDescription(executionTimes[7].Value, configuration).Should()
                .Be("Occurs every 1 week on wednesday every 2 hours between 04:00:00 and 06:00:00 starting on 08/01/2020");

        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Weekly_OnlyThursday()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(2, 0, 0)
            };

            WeeklyFrequency weeklyFreq = new WeeklyFrequency()
            {
                DaysOfWeek = new DayOfWeek[] { DayOfWeek.Thursday },
                Occurrence = 1
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Weekly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = weeklyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01, 12, 15, 10), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 09, 4, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 09, 6, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 16, 4, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 16, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 01, 23, 4, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 01, 23, 6, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 01, 30, 4, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 01, 30, 6, 0, 0));
            Schedule.GetDescription(executionTimes[7].Value, configuration).Should()
                .Be("Occurs every 1 week on thursday every 2 hours between 04:00:00 and 06:00:00 starting on 08/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Weekly_OnlyFriday()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(2, 0, 0)
            };

            WeeklyFrequency weeklyFreq = new WeeklyFrequency()
            {
                DaysOfWeek = new DayOfWeek[] { DayOfWeek.Friday },
                Occurrence = 1
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Weekly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = weeklyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01, 12, 15, 10), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 10, 4, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 10, 6, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 17, 4, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 17, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 01, 24, 4, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 01, 24, 6, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 01, 31, 4, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 01, 31, 6, 0, 0));
            Schedule.GetDescription(executionTimes[7].Value, configuration).Should()
                .Be("Occurs every 1 week on friday every 2 hours between 04:00:00 and 06:00:00 starting on 08/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Weekly_OnlySaturday()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(2, 0, 0)
            };

            WeeklyFrequency weeklyFreq = new WeeklyFrequency()
            {
                DaysOfWeek = new DayOfWeek[] { DayOfWeek.Saturday },
                Occurrence = 1
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Weekly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = weeklyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01, 12, 15, 10), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 11, 4, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 11, 6, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 18, 4, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 18, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 01, 25, 4, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 01, 25, 6, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 02, 01, 4, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 02, 01, 6, 0, 0));
            Schedule.GetDescription(executionTimes[7].Value, configuration).Should()
                .Be("Occurs every 1 week on saturday every 2 hours between 04:00:00 and 06:00:00 starting on 08/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Weekly_OnlySunday()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(2, 0, 0)
            };

            WeeklyFrequency weeklyFreq = new WeeklyFrequency()
            {
                DaysOfWeek = new DayOfWeek[] { DayOfWeek.Sunday },
                Occurrence = 1
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Weekly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = weeklyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01, 12, 15, 10), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 12, 4, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 12, 6, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 19, 4, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 19, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 01, 26, 4, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 01, 26, 6, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 02, 02, 4, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 02, 02, 6, 0, 0));
            Schedule.GetDescription(executionTimes[7].Value, configuration).Should()
                .Be("Occurs every 1 week on sunday every 2 hours between 04:00:00 and 06:00:00 starting on 08/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Weekly_OnlyMondayThreeWeeks()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(2, 0, 0)
            };

            WeeklyFrequency weeklyFreq = new WeeklyFrequency()
            {
                DaysOfWeek = new DayOfWeek[] { DayOfWeek.Monday },
                Occurrence = 3
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Weekly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = weeklyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01, 12, 15, 10), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 27, 4, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 27, 6, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 02, 17, 4, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 02, 17, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 03, 09, 4, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 03, 09, 6, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 03, 30, 4, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 03, 30, 6, 0, 0));
            Schedule.GetDescription(executionTimes[7].Value, configuration).Should()
                .Be("Occurs every 3 weeks on monday every 2 hours between 04:00:00 and 06:00:00 starting on 08/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Weekly_OnlySundayTwoWeeks()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(2, 0, 0)
            };

            WeeklyFrequency weeklyFreq = new WeeklyFrequency()
            {
                DaysOfWeek = new DayOfWeek[] { DayOfWeek.Sunday },
                Occurrence = 2
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Weekly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = weeklyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01, 12, 15, 10), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 12, 4, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 12, 6, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 26, 4, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 26, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 02, 09, 4, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 02, 09, 6, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 02, 23, 4, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 02, 23, 6, 0, 0));
            Schedule.GetDescription(executionTimes[7].Value, configuration).Should()
                .Be("Occurs every 2 weeks on sunday every 2 hours between 04:00:00 and 06:00:00 starting on 08/01/2020");
        }


        #endregion
        #region Monthly
        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Daily_SameMonth()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(8, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(0, 20, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = true,
                Interval = 2,
                DayNumber = 5
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01, 12, 15, 10), configuration, 6);

            executionTimes[0].Should().Be(new DateTime(2020, 03, 05, 4, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 03, 05, 4, 20, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 03, 05, 4, 40, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 03, 05, 5, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 03, 05, 5, 20, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 03, 05, 5, 40, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the 5 of every 2 months every 20 minutes between 04:00:00 and 08:00:00 starting on 08/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Daily_NextMonth()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = true,
                Interval = 2,
                DayNumber = 5
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01, 12, 15, 10), configuration, 6);

            executionTimes[0].Should().Be(new DateTime(2020, 03, 05, 4, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 03, 05, 5, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 03, 05, 6, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 05, 05, 4, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 05, 05, 5, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 05, 05, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the 5 of every 2 months every 1 hour between 04:00:00 and 06:00:00 starting on 08/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_SecondMonday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.Second,
                DayType = MonthlyDayType.Monday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 13, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 13, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 13, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 13, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 13, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 13, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 13, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 13, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Second Monday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_SecondTuesday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.Second,
                DayType = MonthlyDayType.Tuesday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 14, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 14, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 14, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 14, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 14, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 14, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 14, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 14, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Second Tuesday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_SecondWednesday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.Second,
                DayType = MonthlyDayType.Wednesday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 08, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 08, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 08, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 08, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 08, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 08, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 08, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 08, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Second Wednesday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_SecondFriday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.Second,
                DayType = MonthlyDayType.Friday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 10, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 10, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 10, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 10, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 10, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 10, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 10, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 10, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Second Friday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }
        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_SecondSaturday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.Second,
                DayType = MonthlyDayType.Saturday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 11, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 11, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 11, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 11, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 11, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 11, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 11, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 11, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Second Saturday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_SecondSunday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.Second,
                DayType = MonthlyDayType.Sunday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 12, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 12, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 12, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 12, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 12, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 12, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 12, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 12, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Second Sunday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_FirstThursday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.First,
                DayType = MonthlyDayType.Thursday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 02, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 02, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 02, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 02, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 02, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 02, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 02, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 02, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the First Thursday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_FirstMonday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.First,
                DayType = MonthlyDayType.Monday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 06, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 06, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 06, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 06, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 06, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 06, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 06, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 06, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the First Monday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_FirstTuesday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.First,
                DayType = MonthlyDayType.Tuesday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 07, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 07, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 07, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 07, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 07, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 07, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 07, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 07, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the First Tuesday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_FirstWednesday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.First,
                DayType = MonthlyDayType.Wednesday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 01, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 01, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 01, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 01, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 01, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 01, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 01, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 01, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the First Wednesday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_FirstFriday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.First,
                DayType = MonthlyDayType.Friday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 03, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 03, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 03, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 03, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 03, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 03, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 03, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 03, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the First Friday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }
        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_FirstSaturday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.First,
                DayType = MonthlyDayType.Saturday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 04, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 04, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 04, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 04, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 04, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 04, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 04, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 04, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the First Saturday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_FirstSunday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.First,
                DayType = MonthlyDayType.Sunday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 05, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 05, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 05, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 05, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 05, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 05, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 05, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 05, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the First Sunday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }
        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_ThirdMonday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.Third,
                DayType = MonthlyDayType.Monday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 20, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 20, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 20, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 20, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 20, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 20, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 20, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 20, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Third Monday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_ThirdTuesday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.Third,
                DayType = MonthlyDayType.Tuesday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 21, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 21, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 21, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 21, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 21, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 21, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 21, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 21, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Third Tuesday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_ThirdWednesday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.Third,
                DayType = MonthlyDayType.Wednesday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 15, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 15, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 15, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 15, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 15, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 15, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 15, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 15, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Third Wednesday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_ThirdFriday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.Third,
                DayType = MonthlyDayType.Friday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 17, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 17, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 17, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 17, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 17, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 17, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 17, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 17, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Third Friday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }
        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_ThirdSaturday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.Third,
                DayType = MonthlyDayType.Saturday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 18, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 18, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 18, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 18, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 18, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 18, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 18, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 18, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Third Saturday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_ThirdSunday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.Third,
                DayType = MonthlyDayType.Sunday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 19, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 19, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 19, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 19, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 19, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 19, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 19, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 19, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Third Sunday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_FourthMonday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.Fourth,
                DayType = MonthlyDayType.Monday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 27, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 27, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 27, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 27, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 27, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 27, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 27, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 27, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Fourth Monday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_FourthTuesday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.Fourth,
                DayType = MonthlyDayType.Tuesday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 28, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 28, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 28, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 28, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 28, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 28, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 28, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 28, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Fourth Tuesday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_FourthWednesday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.Fourth,
                DayType = MonthlyDayType.Wednesday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 22, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 22, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 22, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 22, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 22, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 22, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 22, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 22, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Fourth Wednesday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_FourthFriday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.Fourth,
                DayType = MonthlyDayType.Friday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 24, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 24, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 24, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 24, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 24, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 24, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 24, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 24, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Fourth Friday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }
        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_FourthSaturday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.Fourth,
                DayType = MonthlyDayType.Saturday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 25, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 25, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 25, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 25, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 25, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 25, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 25, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 25, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Fourth Saturday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_FourthSunday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.Fourth,
                DayType = MonthlyDayType.Sunday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 26, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 26, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 26, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 26, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 26, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 26, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 26, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 26, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Fourth Sunday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_LastMonday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.Last,
                DayType = MonthlyDayType.Monday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 27, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 27, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 27, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 27, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 27, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 27, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 27, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 27, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Last Monday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_LastTuesday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.Last,
                DayType = MonthlyDayType.Tuesday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 28, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 28, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 28, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 28, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 28, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 28, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 28, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 28, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Last Tuesday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_LastWednesday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.Last,
                DayType = MonthlyDayType.Wednesday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 29, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 29, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 29, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 29, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 29, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 29, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 29, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 29, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Last Wednesday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_LastFriday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.Last,
                DayType = MonthlyDayType.Friday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 31, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 31, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 31, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 31, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 24, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 24, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 24, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 24, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Last Friday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }
        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_LastSaturday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.Last,
                DayType = MonthlyDayType.Saturday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 25, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 25, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 25, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 25, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 25, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 25, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 25, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 25, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Last Saturday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_LastSunday()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 3,
                Frequency = MonthlyFrequencyType.Last,
                DayType = MonthlyDayType.Sunday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 8);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 26, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 26, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 26, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 26, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 04, 26, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 04, 26, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 04, 26, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 04, 26, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Last Sunday of every 3 months every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_FirstWeekDay()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 1,
                Frequency = MonthlyFrequencyType.First,
                DayType = MonthlyDayType.Weekday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 12);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 01, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 01, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 01, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 01, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 02, 03, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 02, 03, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 02, 03, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 02, 03, 6, 0, 0));
            executionTimes[8].Should().Be(new DateTime(2020, 03, 02, 3, 0, 0));
            executionTimes[9].Should().Be(new DateTime(2020, 03, 02, 4, 0, 0));
            executionTimes[10].Should().Be(new DateTime(2020, 03, 02, 5, 0, 0));
            executionTimes[11].Should().Be(new DateTime(2020, 03, 02, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the First Weekday of every 1 month every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_SecondWeekDay()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 1,
                Frequency = MonthlyFrequencyType.Second,
                DayType = MonthlyDayType.Weekday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 12);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 02, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 02, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 02, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 02, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 02, 04, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 02, 04, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 02, 04, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 02, 04, 6, 0, 0));
            executionTimes[8].Should().Be(new DateTime(2020, 03, 03, 3, 0, 0));
            executionTimes[9].Should().Be(new DateTime(2020, 03, 03, 4, 0, 0));
            executionTimes[10].Should().Be(new DateTime(2020, 03, 03, 5, 0, 0));
            executionTimes[11].Should().Be(new DateTime(2020, 03, 03, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Second Weekday of every 1 month every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_ThirdWeekDay()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 1,
                Frequency = MonthlyFrequencyType.Third,
                DayType = MonthlyDayType.Weekday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 12);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 03, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 03, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 03, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 03, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 02, 05, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 02, 05, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 02, 05, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 02, 05, 6, 0, 0));
            executionTimes[8].Should().Be(new DateTime(2020, 03, 04, 3, 0, 0));
            executionTimes[9].Should().Be(new DateTime(2020, 03, 04, 4, 0, 0));
            executionTimes[10].Should().Be(new DateTime(2020, 03, 04, 5, 0, 0));
            executionTimes[11].Should().Be(new DateTime(2020, 03, 04, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Third Weekday of every 1 month every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_FourthWeekDay()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 1,
                Frequency = MonthlyFrequencyType.Fourth,
                DayType = MonthlyDayType.Weekday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 12);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 06, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 06, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 06, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 06, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 02, 06, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 02, 06, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 02, 06, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 02, 06, 6, 0, 0));
            executionTimes[8].Should().Be(new DateTime(2020, 03, 05, 3, 0, 0));
            executionTimes[9].Should().Be(new DateTime(2020, 03, 05, 4, 0, 0));
            executionTimes[10].Should().Be(new DateTime(2020, 03, 05, 5, 0, 0));
            executionTimes[11].Should().Be(new DateTime(2020, 03, 05, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Fourth Weekday of every 1 month every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_LastWeekDay()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 1,
                Frequency = MonthlyFrequencyType.Last,
                DayType = MonthlyDayType.Weekday
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 12);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 31, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 31, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 31, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 31, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 02, 28, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 02, 28, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 02, 28, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 02, 28, 6, 0, 0));
            executionTimes[8].Should().Be(new DateTime(2020, 03, 31, 3, 0, 0));
            executionTimes[9].Should().Be(new DateTime(2020, 03, 31, 4, 0, 0));
            executionTimes[10].Should().Be(new DateTime(2020, 03, 31, 5, 0, 0));
            executionTimes[11].Should().Be(new DateTime(2020, 03, 31, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Last Weekday of every 1 month every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_FirstWeekendDay()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 1,
                Frequency = MonthlyFrequencyType.First,
                DayType = MonthlyDayType.WeekendDay
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 12);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 04, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 04, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 04, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 04, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 02, 01, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 02, 01, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 02, 01, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 02, 01, 6, 0, 0));
            executionTimes[8].Should().Be(new DateTime(2020, 03, 01, 3, 0, 0));
            executionTimes[9].Should().Be(new DateTime(2020, 03, 01, 4, 0, 0));
            executionTimes[10].Should().Be(new DateTime(2020, 03, 01, 5, 0, 0));
            executionTimes[11].Should().Be(new DateTime(2020, 03, 01, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the First WeekendDay of every 1 month every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_SecondWeekendDay()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 1,
                Frequency = MonthlyFrequencyType.Second,
                DayType = MonthlyDayType.WeekendDay
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 12);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 05, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 05, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 05, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 05, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 02, 02, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 02, 02, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 02, 02, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 02, 02, 6, 0, 0));
            executionTimes[8].Should().Be(new DateTime(2020, 03, 07, 3, 0, 0));
            executionTimes[9].Should().Be(new DateTime(2020, 03, 07, 4, 0, 0));
            executionTimes[10].Should().Be(new DateTime(2020, 03, 07, 5, 0, 0));
            executionTimes[11].Should().Be(new DateTime(2020, 03, 07, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Second WeekendDay of every 1 month every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_ThirdWeekendDay()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 1,
                Frequency = MonthlyFrequencyType.Third,
                DayType = MonthlyDayType.WeekendDay
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 12);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 11, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 11, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 11, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 11, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 02, 08, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 02, 08, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 02, 08, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 02, 08, 6, 0, 0));
            executionTimes[8].Should().Be(new DateTime(2020, 03, 08, 3, 0, 0));
            executionTimes[9].Should().Be(new DateTime(2020, 03, 08, 4, 0, 0));
            executionTimes[10].Should().Be(new DateTime(2020, 03, 08, 5, 0, 0));
            executionTimes[11].Should().Be(new DateTime(2020, 03, 08, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Third WeekendDay of every 1 month every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_FourthWeekendDay()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 1,
                Frequency = MonthlyFrequencyType.Fourth,
                DayType = MonthlyDayType.WeekendDay
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 12);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 12, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 12, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 12, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 12, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 02, 09, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 02, 09, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 02, 09, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 02, 09, 6, 0, 0));
            executionTimes[8].Should().Be(new DateTime(2020, 03, 14, 3, 0, 0));
            executionTimes[9].Should().Be(new DateTime(2020, 03, 14, 4, 0, 0));
            executionTimes[10].Should().Be(new DateTime(2020, 03, 14, 5, 0, 0));
            executionTimes[11].Should().Be(new DateTime(2020, 03, 14, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Fourth WeekendDay of every 1 month every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_LastWeekendDay()
        {
            DateTime startDate = new DateTime(2020, 01, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 1,
                Frequency = MonthlyFrequencyType.Last,
                DayType = MonthlyDayType.WeekendDay
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 12);

            executionTimes[0].Should().Be(new DateTime(2020, 01, 26, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 01, 26, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 01, 26, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 01, 26, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 02, 29, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 02, 29, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 02, 29, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 02, 29, 6, 0, 0));
            executionTimes[8].Should().Be(new DateTime(2020, 03, 29, 3, 0, 0));
            executionTimes[9].Should().Be(new DateTime(2020, 03, 29, 4, 0, 0));
            executionTimes[10].Should().Be(new DateTime(2020, 03, 29, 5, 0, 0));
            executionTimes[11].Should().Be(new DateTime(2020, 03, 29, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the Last WeekendDay of every 1 month every 1 hour between 03:00:00 and 06:00:00 starting on 01/01/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Daily_ChangeYear()
        {
            DateTime startDate = new DateTime(2020, 11, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = true,
                DayNumber = 1,
                Interval = 1
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 12);

            executionTimes[0].Should().Be(new DateTime(2020, 11, 01, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 11, 01, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 11, 01, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 11, 01, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 12, 01, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 12, 01, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 12, 01, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 12, 01, 6, 0, 0));
            executionTimes[8].Should().Be(new DateTime(2021, 01, 01, 3, 0, 0));
            executionTimes[9].Should().Be(new DateTime(2021, 01, 01, 4, 0, 0));
            executionTimes[10].Should().Be(new DateTime(2021, 01, 01, 5, 0, 0));
            executionTimes[11].Should().Be(new DateTime(2021, 01, 01, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the 1 of every 1 month every 1 hour between 03:00:00 and 06:00:00 starting on 01/11/2020");
        }

        [Fact]
        public void ScheduleNextExecution_Recurring_Monthly_Recurring_ChangeYear()
        {
            DateTime startDate = new DateTime(2020, 11, 01);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(3, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };

            MonthlyFrequency monthlyFreq = new MonthlyFrequency()
            {
                IsDaily = false,
                Interval = 1,
                Frequency = MonthlyFrequencyType.First,
                DayType = MonthlyDayType.WeekendDay
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Monthly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.MonthlyFrequency = monthlyFreq;


            DateTime?[] executionTimes = Schedule.GetMultipleNextExecutionTimes(new DateTime(2020, 01, 01), configuration, 12);

            executionTimes[0].Should().Be(new DateTime(2020, 11, 01, 3, 0, 0));
            executionTimes[1].Should().Be(new DateTime(2020, 11, 01, 4, 0, 0));
            executionTimes[2].Should().Be(new DateTime(2020, 11, 01, 5, 0, 0));
            executionTimes[3].Should().Be(new DateTime(2020, 11, 01, 6, 0, 0));
            executionTimes[4].Should().Be(new DateTime(2020, 12, 05, 3, 0, 0));
            executionTimes[5].Should().Be(new DateTime(2020, 12, 05, 4, 0, 0));
            executionTimes[6].Should().Be(new DateTime(2020, 12, 05, 5, 0, 0));
            executionTimes[7].Should().Be(new DateTime(2020, 12, 05, 6, 0, 0));
            executionTimes[8].Should().Be(new DateTime(2021, 01, 02, 3, 0, 0));
            executionTimes[9].Should().Be(new DateTime(2021, 01, 02, 4, 0, 0));
            executionTimes[10].Should().Be(new DateTime(2021, 01, 02, 5, 0, 0));
            executionTimes[11].Should().Be(new DateTime(2021, 01, 02, 6, 0, 0));
            Schedule.GetDescription(executionTimes[5].Value, configuration).Should()
                .Be("Occurs the First WeekendDay of every 1 month every 1 hour between 03:00:00 and 06:00:00 starting on 01/11/2020");
        }

        #endregion
        #region Descriptions
        [Fact]
        public void Schedule_Description_NotOccurring()
        {
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(8, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(1, 0, 0)
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = new DateTime(2020, 01, 01);
            configuration.EndDate = new DateTime(2020, 02, 01);
            configuration.DailyFrequency = dailyFreq;


            Schedule.GetDescription(new DateTime(2020, 03, 01, 08, 21, 15), configuration).Should()
                .Be("Will not occur. Schedule will end on 01/02/2020");
        }

        [Fact]
        public void Schedule_Description_Recurring_Weekly_TwoDays()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(0, 50, 0)
            };

            WeeklyFrequency weeklyFreq = new WeeklyFrequency()
            {
                DaysOfWeek = new DayOfWeek[] { DayOfWeek.Monday,DayOfWeek.Wednesday},
                Occurrence = 1
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Weekly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = weeklyFreq;


            Schedule.GetDescription(new DateTime(2020, 03, 01, 08, 21, 15), configuration).Should()
                .Be("Occurs every 1 week on monday and wednesday every 50 minutes between 04:00:00 and 06:00:00 starting on 08/01/2020");
        }

        [Fact]
        public void Schedule_Description_Recurring_Weekly_ThreeDaysNoOrder()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(0, 50, 0)
            };

            WeeklyFrequency weeklyFreq = new WeeklyFrequency()
            {
                DaysOfWeek = new DayOfWeek[] { DayOfWeek.Wednesday, DayOfWeek.Sunday, DayOfWeek.Monday },
                Occurrence = 1
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Weekly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = weeklyFreq;


            Schedule.GetDescription(new DateTime(2020, 03, 01, 08, 21, 15), configuration).Should()
                .Be("Occurs every 1 week on monday, wednesday and sunday every 50 minutes between 04:00:00 and 06:00:00 starting on 08/01/2020");
        }


        [Fact]
        public void Schedule_Description_Recurring_Weekly_Everyday()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(0, 50, 0)
            };

            WeeklyFrequency weeklyFreq = new WeeklyFrequency()
            {
                DaysOfWeek = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday, 
                        DayOfWeek.Tuesday,DayOfWeek.Friday,DayOfWeek.Saturday,DayOfWeek.Sunday },
                Occurrence = 1
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Weekly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = weeklyFreq;


            Schedule.GetDescription(new DateTime(2020, 03, 01, 08, 21, 15), configuration).Should()
                .Be("Occurs every 1 week on monday, tuesday, wednesday, friday, saturday and sunday every 50 minutes between 04:00:00 and 06:00:00 starting on 08/01/2020");
        }

        [Fact]
        public void Schedule_Description_Recurring_Weekly_EverydayThreeWeeks()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(0, 50, 0)
            };

            WeeklyFrequency weeklyFreq = new WeeklyFrequency()
            {
                DaysOfWeek = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday,
                        DayOfWeek.Tuesday,DayOfWeek.Friday,DayOfWeek.Saturday,DayOfWeek.Sunday },
                Occurrence = 3
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Weekly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = weeklyFreq;


            Schedule.GetDescription(new DateTime(2020, 03, 01, 08, 21, 15), configuration).Should()
                .Be("Occurs every 3 weeks on monday, tuesday, wednesday, friday, saturday and sunday every 50 minutes between 04:00:00 and 06:00:00 starting on 08/01/2020");
        }

        [Fact]
        public void Schedule_Description_Recurring_Weekly_EverydayThreeWeeks_Spanish()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(0, 50, 0)
            };

            WeeklyFrequency weeklyFreq = new WeeklyFrequency()
            {
                DaysOfWeek = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday,
                        DayOfWeek.Tuesday,DayOfWeek.Friday,DayOfWeek.Saturday,DayOfWeek.Sunday },
                Occurrence = 3
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Weekly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = weeklyFreq;
            configuration.Culture = "ES-ES";


            Schedule.GetDescription(new DateTime(2020, 03, 01, 08, 21, 15), configuration).Should()
                .Be("Ocurre cada 3 semanas el lunes, martes, miércoles, viernes, sábado y domingo cada 50 minutos entre las 04:00:00 y las 06:00:00 empezando el 08/01/2020");
        }

        [Fact]
        public void Schedule_Description_Recurring_Weekly_EverydayThreeWeeks_EnglishUK()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(0, 50, 0)
            };

            WeeklyFrequency weeklyFreq = new WeeklyFrequency()
            {
                DaysOfWeek = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday,
                        DayOfWeek.Tuesday,DayOfWeek.Friday,DayOfWeek.Saturday,DayOfWeek.Sunday },
                Occurrence = 3
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Weekly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = weeklyFreq;
            configuration.Culture = "EN-UK";


            Schedule.GetDescription(new DateTime(2020, 03, 01, 08, 21, 15), configuration).Should()
                .Be("Occurs every 3 weeks on monday, tuesday, wednesday, friday, saturday and sunday every 50 minutes between 04:00:00 and 06:00:00 starting on 08/01/2020");
        }

        [Fact]
        public void Schedule_Description_Recurring_Weekly_EverydayThreeWeeks_EnglishUS()
        {
            DateTime startDate = new DateTime(2020, 01, 08);
            DailyFrequency dailyFreq = new DailyFrequency()
            {
                StartTime = new TimeSpan(4, 0, 0),
                EndTime = new TimeSpan(6, 0, 0),
                IsRecurring = true,
                Occurrence = new TimeSpan(0, 50, 0)
            };

            WeeklyFrequency weeklyFreq = new WeeklyFrequency()
            {
                DaysOfWeek = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday,
                        DayOfWeek.Tuesday,DayOfWeek.Friday,DayOfWeek.Saturday,DayOfWeek.Sunday },
                Occurrence = 3
            };


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Frequency = FrequencyType.Weekly;
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = weeklyFreq;
            configuration.Culture = "EN-US";


            Schedule.GetDescription(new DateTime(2020, 03, 01, 08, 21, 15), configuration).Should()
                .Be("Occurs every 3 weeks on monday, tuesday, wednesday, friday, saturday and sunday every 50 minutes between 04:00:00 and 06:00:00 starting on 01/08/2020");
        }
        #endregion
    }
}
