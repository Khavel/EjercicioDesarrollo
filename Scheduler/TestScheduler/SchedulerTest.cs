using System;
using Xunit;
using Scheduler;
using FluentAssertions;

namespace TestScheduler
{
    public class SchedulerTest
    {
        [Fact]
        public void SchedulerDateTimeOnceNullValidation()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Once;
            configuration.DateTimeOnce = null;

            Assert.Throws<ConfigurationException>(() => Schedule.GetNextExecutionTime(DateTime.Today, configuration));
        }

        [Fact]
        public void SchedulerDateTimeIsMaxValueNullValidation()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Once;
            configuration.DateTimeOnce = DateTime.MaxValue;

            Assert.Throws<ConfigurationException>(() => Schedule.GetNextExecutionTime(DateTime.Today,configuration));
        }

        [Fact]
        public void ScheduleNextExecution_Once()
        {
            DateTime dateTime = new DateTime(2020,01,08);


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Once;
            configuration.DateTimeOnce = dateTime;


            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 01), configuration).Should().Be(new DateTime(2020, 01, 08));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 05), configuration).Should().Be(new DateTime(2020, 01, 08));
            Schedule.GetNextExecutionTime(new DateTime(2020, 01, 10), configuration).Should().Be(new DateTime(2020, 01, 10));
        }

        [Fact]
        public void ScheduleRecurringStartDateMaxValueValidation()
        {
            DateTime startDate = DateTime.MaxValue;
            DateTime currentDate = new DateTime(2020, 01, 01);


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;


            Assert.Throws<ConfigurationException>(() => Schedule.GetNextExecutionTime(currentDate, configuration));
        }

        [Fact]
        public void ScheduleRecurringStartEndDateMaxValueValidation()
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
        public void ScheduleRecurringStartEndGreaterThanEndDateValidation()
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
        public void ScheduleRecurringIntervalLessThanOneValidation()
        {
            DateTime startDate = new DateTime(2020, 01, 10);
            DateTime endDate = new DateTime(2020, 01, 01);
            DateTime currentDate = new DateTime(2020, 01, 01);
            int interval = -1;


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.EndDate = endDate;
            configuration.Interval = interval;


            Assert.Throws<ConfigurationException>(() => Schedule.GetNextExecutionTime(currentDate, configuration));
        }

        [Fact]
        public void ScheduleRecurringDailyNullValidation()
        {
            DateTime startDate = new DateTime(2020, 01, 10);
            DateTime endDate = new DateTime(2020, 01, 01);
            DateTime currentDate = new DateTime(2020, 01, 01);
            int interval = -1;


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.StartDate = startDate;
            configuration.EndDate = endDate;
            configuration.Interval = interval;


            Assert.Throws<ConfigurationException>(() => Schedule.GetNextExecutionTime(currentDate, configuration));
        }

        [Fact]
        public void ScheduleWeeklyConfigurationNullValidation()
        {
            DateTime currentDate = new DateTime(2020,01,01,4,15,0);
            DateTime startDate = new DateTime(2020, 01, 01);
            DateTime endDate = new DateTime(2020, 02, 01);


            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.Frequency = FrequencyType.Weekly;
            configuration.Interval = 1;
            configuration.StartDate = startDate;
            configuration.EndDate = endDate;

            Assert.Throws<ConfigurationException>(() => Schedule.GetNextExecutionTime(currentDate, configuration));
        }

        [Theory]
        [InlineData(FrequencyType.Weekly, 2, 0, new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
        "02:00:00", "00:00:00", "12:00:00", "01/01/2019 00:00:00", "01/01/2020", "01/02/2020")]
        [InlineData(FrequencyType.Weekly, 2, -1, new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
        "02:00:00", "00:00:00", "12:00:00", "01/01/2019 00:00:00", "01/01/2020", "01/02/2020")]
        [InlineData(FrequencyType.Weekly, 2, 2, new DayOfWeek[] { },
        "02:00:00", "00:00:00", "12:00:00", "01/01/2019 00:00:00", "01/01/2020", "01/02/2020")]
        [InlineData(FrequencyType.Weekly, 2, 2, null,
        "02:00:00", "00:00:00", "12:00:00", "01/01/2019 00:00:00", "01/01/2020", "01/02/2020")]

        public void ScheduleWeeklyConfigurationValidation(FrequencyType frequency, int interval, int occurrenceWeekly,
            DayOfWeek[] daysOfWeek, string occurrence, string startTimeStr,
            string endTimeStr, string currentDateStr, string startDateStr, string endDateStr)
        {
            DateTime startDate = DateTime.ParseExact(startDateStr, "d", null);
            DateTime currentDate = DateTime.ParseExact(currentDateStr, "dd/MM/yyyy hh:mm:ss", null);
            TimeSpan timespanOccurrence = TimeSpan.ParseExact(occurrence, @"hh\:mm\:ss", null);
            TimeSpan startTime = TimeSpan.ParseExact(startTimeStr, @"hh\:mm\:ss", null);
            TimeSpan endTime = TimeSpan.ParseExact(endTimeStr, @"hh\:mm\:ss", null);

            DateTime? endDate = null;
            if (string.IsNullOrEmpty(endDateStr) == false)
            {
                endDate = DateTime.ParseExact(endDateStr, "d", null);
            }

            WeeklyFrequency weeklyFreq = new WeeklyFrequency
            {
                DaysOfWeek = daysOfWeek,
                Occurrence = occurrenceWeekly
            };

            DailyFrequency dailyFreq = new DailyFrequency
            {
                Occurrence = timespanOccurrence,
                IsRecurring = true,
                StartTime = startTime,
                EndTime = endTime
            };

            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.Frequency = frequency;
            configuration.Interval = interval;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = weeklyFreq;
            configuration.StartDate = startDate;
            configuration.EndDate = endDate;

            Assert.Throws<ConfigurationException>(() => Schedule.GetNextExecutionTime(currentDate, configuration));
        }



        [Theory]
        [InlineData(FrequencyType.Daily, 1, "02:00:00", "04:00:00", "", "01/01/2020 04:15:00", "01/01/2020", "01/02/2020")]
        [InlineData(FrequencyType.Daily, 1, "02:00:00", "", "08:00:00", "01/01/2020 08:00:00", "01/01/2020", "01/02/2020")]
        public void ScheduleDailyConfigurationValidation(FrequencyType frequency, int interval, string occurrence, string startTimeStr, string endTimeStr, string currentDateStr, string startDateStr, string endDateStr)
        {
            DateTime startDate = DateTime.ParseExact(startDateStr, "d", null);
            DateTime currentDate = DateTime.ParseExact(currentDateStr, "dd/MM/yyyy hh:mm:ss", null);
            TimeSpan timespanOccurrence = TimeSpan.ParseExact(occurrence, @"hh\:mm\:ss", null);
            TimeSpan? startTime = null;
            if (string.IsNullOrEmpty(startTimeStr) == false)
            {
                startTime = TimeSpan.ParseExact(startTimeStr, @"hh\:mm\:ss", null);
            }
            TimeSpan? endTime = null;
            if (string.IsNullOrEmpty(endTimeStr) == false)
            {
                endTime = TimeSpan.ParseExact(endTimeStr, @"hh\:mm\:ss", null);
            }

            DateTime? endDate = null;
            if (string.IsNullOrEmpty(endDateStr) == false)
            {
                endDate = DateTime.ParseExact(endDateStr, "d", null);
            }

            DailyFrequency dailyFreq = new DailyFrequency
            {
                Occurrence = timespanOccurrence,
                IsRecurring = true,
                StartTime = startTime,
                EndTime = endTime
            };

            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.Frequency = frequency;
            configuration.Interval = interval;
            configuration.DailyFrequency = dailyFreq;
            configuration.StartDate = startDate;
            configuration.EndDate = endDate;

            Assert.Throws<ConfigurationException>(() => Schedule.GetNextExecutionTime(currentDate, configuration));
        }

        //[Theory]
        //[InlineData("08/01/2020 14:00", "01/01/2020", "04/01/2020", "Occurs once. Schedule will be used on 08/01/2020 at 14:00 starting on 01/01/2020")]
        //public void ScheduleDescription_Once(string dateTimeStr, string startDateStr, string currentDateStr, string expected)
        //{
        //    DateTime dateTime = DateTime.ParseExact(dateTimeStr, "g", null);
        //    DateTime startDate = DateTime.ParseExact(startDateStr, "d", null);
        //    DateTime currentDate = DateTime.ParseExact(currentDateStr, "d", null);

        //    SchedulerConfiguration configuration = new SchedulerConfiguration();
        //    configuration.Type = SchedulerType.Once;
        //    configuration.Frequency = FrequencyType.Daily;
        //    configuration.DateTimeOnce = dateTime;
        //    configuration.Interval = 0;
        //    configuration.StartDate = startDate;

        //    Assert.Equal(expected, Schedule.GetDescription(currentDate, configuration));
        //}

        [Theory]
        [InlineData(FrequencyType.Daily, 1, "00:00:00", "04/01/2020", "01/01/2020", "01/02/2020", "05/01/2020 00:00:00")]
        [InlineData(FrequencyType.Daily, 1, "11:30:00", "05/01/2020", "01/01/2020", "01/02/2020", "06/01/2020 11:30:00")]
        [InlineData(FrequencyType.Daily, 1, "00:50:00", "06/01/2020", "01/01/2020", "01/02/2020", "07/01/2020 00:50:00")]
        public void ScheduleNextExecution_RecurringWithDailyFrequencyOnce(FrequencyType frequency, int interval, string occurrence, string currentDateStr, string startDateStr, string endDateStr, string expected)
        {
            DateTime startDate = DateTime.ParseExact(startDateStr, "d", null);
            DateTime currentDate = DateTime.ParseExact(currentDateStr, "d", null);
            TimeSpan timespanOccurrence = TimeSpan.ParseExact(occurrence, @"hh\:mm\:ss", null);
            DateTime? endDate = null;
            if (string.IsNullOrEmpty(endDateStr) == false)
            {
                endDate = DateTime.ParseExact(endDateStr, "d", null);
            }

            DateTime? expectedDate = null;
            if (string.IsNullOrEmpty(expected) == false)
            {
                expectedDate = DateTime.ParseExact(expected, "dd/MM/yyyy hh:mm:ss", null);
            }

            DailyFrequency dailyFreq = new DailyFrequency
            {
                Occurrence = timespanOccurrence,
                IsRecurring = false
            };

            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.Frequency = frequency;
            configuration.Interval = interval;
            configuration.DailyFrequency = dailyFreq;
            configuration.StartDate = startDate;
            configuration.EndDate = endDate;

            Assert.Equal(expectedDate, Schedule.GetNextExecutionTime(currentDate, configuration));
        }

        [Theory]
        [InlineData(FrequencyType.Daily, 1, "02:00:00", "00:00:00", "12:00:00", "01/01/2020 00:00:00", "01/01/2020", "01/02/2020", "02/01/2020 00:00:00")]
        [InlineData(FrequencyType.Daily, 1, "02:00:00", "04:00:00", "08:00:00", "01/01/2020 04:15:00", "01/01/2020", "01/02/2020", "02/01/2020 06:00:00")]
        [InlineData(FrequencyType.Daily, 1, "02:00:00", "04:00:00", "08:00:00", "01/01/2020 08:00:00", "01/01/2020", "01/02/2020", "02/01/2020 04:00:00")]
        public void ScheduleNextExecution_RecurringWithDailyFrequencyRecurring(FrequencyType frequency, int interval, string occurrence, string startTimeStr, string endTimeStr, string currentDateStr, string startDateStr, string endDateStr, string expected)
        {
            DateTime startDate = DateTime.ParseExact(startDateStr, "d", null);
            DateTime currentDate = DateTime.ParseExact(currentDateStr, "dd/MM/yyyy hh:mm:ss", null);
            TimeSpan timespanOccurrence = TimeSpan.ParseExact(occurrence, @"hh\:mm\:ss", null);
            TimeSpan startTime = TimeSpan.ParseExact(startTimeStr, @"hh\:mm\:ss", null);
            TimeSpan endTime = TimeSpan.ParseExact(endTimeStr, @"hh\:mm\:ss", null);

            DateTime? endDate = null;
            if (string.IsNullOrEmpty(endDateStr) == false)
            {
                endDate = DateTime.ParseExact(endDateStr, "d", null);
            }

            DateTime? expectedDate = null;
            if (string.IsNullOrEmpty(expected) == false)
            {
                expectedDate = DateTime.ParseExact(expected, "dd/MM/yyyy hh:mm:ss", null);
            }

            DailyFrequency dailyFreq = new DailyFrequency
            {
                Occurrence = timespanOccurrence,
                IsRecurring = true,
                StartTime = startTime,
                EndTime = endTime
            };

            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.Frequency = frequency;
            configuration.Interval = interval;
            configuration.DailyFrequency = dailyFreq;
            configuration.StartDate = startDate;
            configuration.EndDate = endDate;

            Assert.Equal(expectedDate, Schedule.GetNextExecutionTime(currentDate, configuration));
        }

        [Theory]
        [InlineData(FrequencyType.Weekly, 1, 2, new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
            "02:00:00", "04:00:00", "08:00:00", "01/01/2019 00:00:00", "01/01/2020", "01/02/2020", "01/01/2020 04:00:00")]
        [InlineData(FrequencyType.Weekly, 1, 2, new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
            "02:00:00", "04:00:00", "08:00:00", "01/01/2019 04:15:00", "01/01/2020", "01/02/2020", "01/01/2020 04:00:00")]
        [InlineData(FrequencyType.Weekly, 1, 2, new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
            "02:00:00", "04:00:00", "08:00:00", "01/01/2019 08:00:00", "01/01/2020", "01/02/2020", "01/01/2020 04:00:00")]
        [InlineData(FrequencyType.Weekly, 1, 2, new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
            "02:00:00", "04:00:00", "08:00:00", "02/01/2020 08:00:00", "01/01/2020", "01/02/2020", "03/01/2020 04:00:00")]
        [InlineData(FrequencyType.Weekly, 1, 2, new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
            "02:00:00", "04:00:00", "08:00:00", "03/01/2020 08:00:00", "01/01/2020", "01/02/2020", "13/01/2020 04:00:00")]
        [InlineData(FrequencyType.Weekly, 1, 2, new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Thursday, DayOfWeek.Friday },
            "02:00:00", "04:00:00", "08:00:00", "13/01/2020 06:00:00", "01/01/2020", "01/02/2020", "16/01/2020 04:00:00")]
        [InlineData(FrequencyType.Weekly, 1, 2, new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Thursday, DayOfWeek.Friday },
            "02:00:00", "04:00:00", "08:00:00", "16/01/2020 04:00:00", "01/01/2020", "01/02/2020", "17/01/2020 04:00:00")]

        public void ScheduleNextExecution_RecurringWithWeeklyFrequencyRecurring(FrequencyType frequency, int interval, int occurrenceWeekly,
            DayOfWeek[] daysOfWeek, string occurrence, string startTimeStr, string endTimeStr, string currentDateStr,
            string startDateStr, string endDateStr, string expected)
        {
            DateTime startDate = DateTime.ParseExact(startDateStr, "d", null);
            DateTime currentDate = DateTime.ParseExact(currentDateStr, "dd/MM/yyyy hh:mm:ss", null);
            TimeSpan timespanOccurrence = TimeSpan.ParseExact(occurrence, @"hh\:mm\:ss", null);
            TimeSpan startTime = TimeSpan.ParseExact(startTimeStr, @"hh\:mm\:ss", null);
            TimeSpan endTime = TimeSpan.ParseExact(endTimeStr, @"hh\:mm\:ss", null);

            DateTime? endDate = null;
            if (string.IsNullOrEmpty(endDateStr) == false)
            {
                endDate = DateTime.ParseExact(endDateStr, "d", null);
            }

            DateTime? expectedDate = null;
            if (string.IsNullOrEmpty(expected) == false)
            {
                expectedDate = DateTime.ParseExact(expected, "dd/MM/yyyy hh:mm:ss", null);
            }

            WeeklyFrequency weeklyFreq = new WeeklyFrequency
            {
                DaysOfWeek = daysOfWeek,
                Occurrence = occurrenceWeekly
            };

            DailyFrequency dailyFreq = new DailyFrequency
            {
                Occurrence = timespanOccurrence,
                IsRecurring = true,
                StartTime = startTime,
                EndTime = endTime
            };

            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.Frequency = frequency;
            configuration.Interval = interval;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = weeklyFreq;
            configuration.StartDate = startDate;
            configuration.EndDate = endDate;

            Assert.Equal(expectedDate, Schedule.GetNextExecutionTime(currentDate, configuration));
        }


        [Theory]
        [InlineData(FrequencyType.Daily, 2, "02:00:00", "00:00:00", "12:00:00", "01/01/2020 00:00:00", "01/01/2020",
            "01/02/2020", "Occurs every 2 days every 2 hours between 00:00:00 and 12:00:00 starting on 01/01/2020")]
        [InlineData(FrequencyType.Daily, 2, "00:10:00", "00:00:00", "12:00:00", "01/01/2020 00:00:00", "01/01/2020",
            "01/02/2020", "Occurs every 2 days every 10 minutes between 00:00:00 and 12:00:00 starting on 01/01/2020")]
        [InlineData(FrequencyType.Daily, 2, "00:00:10", "00:00:00", "12:00:00", "01/01/2020 00:00:00", "01/01/2020",
            "01/02/2020", "Occurs every 2 days every 10 seconds between 00:00:00 and 12:00:00 starting on 01/01/2020")]
        public void ScheduleDescription_RecurringDaily(FrequencyType frequency, int interval, string occurrence, string startTimeStr,
            string endTimeStr, string currentDateStr, string startDateStr, string endDateStr, string expected)
        {
            DateTime startDate = DateTime.ParseExact(startDateStr, "d", null);
            DateTime currentDate = DateTime.ParseExact(currentDateStr, "dd/MM/yyyy hh:mm:ss", null);
            TimeSpan timespanOccurrence = TimeSpan.ParseExact(occurrence, @"hh\:mm\:ss", null);
            TimeSpan startTime = TimeSpan.ParseExact(startTimeStr, @"hh\:mm\:ss", null);
            TimeSpan endTime = TimeSpan.ParseExact(endTimeStr, @"hh\:mm\:ss", null);

            DateTime? endDate = null;
            if (string.IsNullOrEmpty(endDateStr) == false)
            {
                endDate = DateTime.ParseExact(endDateStr, "d", null);
            }

            DailyFrequency dailyFreq = new DailyFrequency
            {
                Occurrence = timespanOccurrence,
                IsRecurring = true,
                StartTime = startTime,
                EndTime = endTime
            };

            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.Frequency = frequency;
            configuration.Interval = interval;
            configuration.DailyFrequency = dailyFreq;
            configuration.StartDate = startDate;
            configuration.EndDate = endDate;

            Assert.Equal(expected, Schedule.GetDescription(currentDate, configuration));
        }

        [Theory]
        [InlineData(FrequencyType.Weekly, 2, 2, new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
            "02:00:00", "00:00:00", "12:00:00", "01/01/2019 00:00:00", "01/01/2020", "01/02/2020",
            "Occurs every 2 weeks on monday, wednesday and friday every 2 hours between 00:00:00 and 12:00:00 starting on 01/01/2020")]
        [InlineData(FrequencyType.Weekly, 2, 4, new DayOfWeek[] { DayOfWeek.Friday },
            "02:00:00", "00:00:00", "12:00:00", "01/01/2019 04:15:00", "01/01/2020", "01/02/2020",
            "Occurs every 4 weeks on friday every 2 hours between 00:00:00 and 12:00:00 starting on 01/01/2020")]
        [InlineData(FrequencyType.Weekly, 2, 2, new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday },
            "02:00:00", "00:00:00", "12:00:00", "01/01/2019 08:00:00", "01/01/2020", "01/02/2020",
            "Occurs every 2 weeks on monday and wednesday every 2 hours between 00:00:00 and 12:00:00 starting on 01/01/2020")]
        public void ScheduleDescription_RecurringWeekly(FrequencyType frequency, int interval, int occurrenceWeekly,
            DayOfWeek[] daysOfWeek, string occurrence, string startTimeStr,
            string endTimeStr, string currentDateStr, string startDateStr, string endDateStr, string expected)
        {
            DateTime startDate = DateTime.ParseExact(startDateStr, "d", null);
            DateTime currentDate = DateTime.ParseExact(currentDateStr, "dd/MM/yyyy hh:mm:ss", null);
            TimeSpan timespanOccurrence = TimeSpan.ParseExact(occurrence, @"hh\:mm\:ss", null);
            TimeSpan startTime = TimeSpan.ParseExact(startTimeStr, @"hh\:mm\:ss", null);
            TimeSpan endTime = TimeSpan.ParseExact(endTimeStr, @"hh\:mm\:ss", null);

            DateTime? endDate = null;
            if (string.IsNullOrEmpty(endDateStr) == false)
            {
                endDate = DateTime.ParseExact(endDateStr, "d", null);
            }

            WeeklyFrequency weeklyFreq = new WeeklyFrequency
            {
                DaysOfWeek = daysOfWeek,
                Occurrence = occurrenceWeekly
            };

            DailyFrequency dailyFreq = new DailyFrequency
            {
                Occurrence = timespanOccurrence,
                IsRecurring = true,
                StartTime = startTime,
                EndTime = endTime
            };

            SchedulerConfiguration configuration = new SchedulerConfiguration();
            configuration.Type = SchedulerType.Recurring;
            configuration.Frequency = frequency;
            configuration.Interval = interval;
            configuration.DailyFrequency = dailyFreq;
            configuration.WeeklyFrequency = weeklyFreq;
            configuration.StartDate = startDate;
            configuration.EndDate = endDate;

            Assert.Equal(expected, Schedule.GetDescription(currentDate, configuration));
        }


    }
}
