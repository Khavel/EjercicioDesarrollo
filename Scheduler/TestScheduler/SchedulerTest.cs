using System;
using Xunit;
using Scheduler;
using FluentAssertions;

namespace TestScheduler
{
    public class SchedulerTest
    {
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
        }


        #endregion
        #region Descriptions
        [Fact]
        public void Schedule_Description_Once()
        {
            DateTime dateTime = new DateTime(2020, 01, 08,12,55,15);


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Once;
            configuration.DateTimeOnce = dateTime;


            Schedule.GetDescription(new DateTime(2020, 01, 01,08,21,15), configuration).Should()
                .Be("Occurs once. Schedule will be used on 08/01/2020 at 12:55");
        }

        [Fact]
        public void Schedule_Description_Recurring_Daily_NonRecurring()
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



            Schedule.GetDescription(new DateTime(2020, 01, 01, 08, 21, 15), configuration).Should()
                .Be("Occurs everyday once at 21:15:15 starting on 08/01/2020");
        }

        [Fact]
        public void Schedule_Description_Recurring_Daily_WithSeconds()
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



            Schedule.GetDescription(new DateTime(2020, 01, 01, 08, 21, 15), configuration).Should()
                .Be("Occurs everyday every 10 seconds between 04:00:00 and 08:00:00 starting on 08/01/2020");
        }

        [Fact]
        public void Schedule_Description_Recurring_Daily_WithMinutes()
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


            Schedule.GetDescription(new DateTime(2020, 01, 01, 08, 21, 15), configuration).Should()
                .Be("Occurs everyday every 20 minutes between 04:00:00 and 08:00:00 starting on 08/01/2020");
        }


        [Fact]
        public void Schedule_Description_Recurring_Daily_WithHours()
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


            Schedule.GetDescription(new DateTime(2020, 01, 01, 08, 21, 15), configuration).Should()
                .Be("Occurs everyday every 1 hour between 04:00:00 and 08:00:00 starting on 08/01/2020");
        }

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
        #endregion
    }
}
