using System;
using Xunit;
using Scheduler;

namespace TestScheduler
{
    public class SchedulerTest
    {
        [Theory]
        [InlineData(SchedulerType.Once, -1, FrequencyType.Daily, "01/01/2020", "01/01/2020", "01/02/2020")]
        [InlineData(SchedulerType.Once, 2, FrequencyType.Daily, "01/01/2020", "01/01/2020", "01/02/2019")]
        [InlineData(SchedulerType.Once, 2, FrequencyType.Daily, "31/12/9999", "01/01/2020", "01/01/2021")]
        [InlineData(SchedulerType.Once, 2, FrequencyType.Daily, "01/01/2020", "31/12/9999", "01/01/2021")]
        public void SchedulerConfigurationValidation(SchedulerType type, int interval, FrequencyType frequency, string dateTimeStr, string startDateStr, string endDateStr)
        {
            DateTime dateTime = DateTime.ParseExact(dateTimeStr, "dd/MM/yyyy", null);
            DateTime startDate = DateTime.ParseExact(startDateStr, "dd/MM/yyyy", null);
            DateTime endDate = DateTime.ParseExact(endDateStr, "dd/MM/yyyy", null);

            SchedulerConfiguration configuration = new SchedulerConfiguration(
                   type,
                   frequency,
                   dateTime,
                   interval,
                   null,
                   null,
                   startDate,
                   endDate);

            Assert.Throws<ConfigurationException>(() => Validator.ValidateBasicConfiguration(configuration));
        }

        [Theory]
        [InlineData("08/01/2020 14:00", "01/01/2020", "04/01/2020", "08/01/2020 14:00")]
        public void ScheduleNextExecution_Once(string dateTimeStr, string startDateStr, string currentDateStr, string expected)
        {
            DateTime dateTime = DateTime.ParseExact(dateTimeStr, "g", null);
            DateTime startDate = DateTime.ParseExact(startDateStr, "d", null);
            DateTime currentDate = DateTime.ParseExact(currentDateStr, "d", null);
            DateTime expectedDate = DateTime.ParseExact(expected, "g", null);


            SchedulerConfiguration configuration = new SchedulerConfiguration(
                   SchedulerType.Once,
                   FrequencyType.Daily,
                   dateTime,
                   0,
                   null,
                   null,
                   startDate,
                   null);
            Schedule sched = new Schedule(configuration);


            Assert.Equal(expectedDate, sched.GetNextExecutionTime(currentDate));
        }

        [Theory]
        [InlineData("08/01/2020 14:00", "01/01/2020", "04/01/2020", "Occurs once. Schedule will be used on 08/01/2020 at 14:00 starting on 01/01/2020")]
        public void ScheduleDescription_Once(string dateTimeStr, string startDateStr, string currentDateStr, string expected)
        {
            DateTime dateTime = DateTime.ParseExact(dateTimeStr, "g", null);
            DateTime startDate = DateTime.ParseExact(startDateStr, "d", null);
            DateTime currentDate = DateTime.ParseExact(currentDateStr, "d", null);

            SchedulerConfiguration configuration = new SchedulerConfiguration(
                   SchedulerType.Once,
                   FrequencyType.Daily,
                   dateTime,
                   0,
                   null,
                   null,
                   startDate,
                   null);
            Schedule sched = new Schedule(configuration);


            Assert.Equal(expected, sched.GetDescription(currentDate));
        }

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

            SchedulerConfiguration configuration = new SchedulerConfiguration(
                   SchedulerType.Recurring,
                   frequency,
                   null,
                   interval,
                   dailyFreq,
                   null,
                   startDate,
                   endDate);
            Schedule sched = new Schedule(configuration);


            Assert.Equal(expectedDate, sched.GetNextExecutionTime(currentDate));
        }

        [Theory]
        [InlineData(FrequencyType.Daily, 1, "02:00:00", "00:00:00", "12:00:00", "01/01/2020 00:00:00", "01/01/2020", "01/02/2020", "02/01/2020 00:00:00")]
        [InlineData(FrequencyType.Daily, 1, "02:00:00", "04:00:00", "08:00:00", "01/01/2020 04:15:00", "01/01/2020", "01/02/2020", "02/01/2020 06:00:00")]
        [InlineData(FrequencyType.Daily, 1, "02:00:00", "04:00:00", "08:00:00", "01/01/2020 08:00:00", "01/01/2020", "01/02/2020", "02/01/2020 04:00:00")]
        public void ScheduleNextExecution_RecurringWithDailyFrequencyRecurring(FrequencyType frequency,int interval, string occurrence, string startTimeStr, string endTimeStr, string currentDateStr, string startDateStr, string endDateStr, string expected)
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

            SchedulerConfiguration configuration = new SchedulerConfiguration(
                   SchedulerType.Recurring,
                   frequency,
                   null,
                   interval,
                   dailyFreq,
                   null,
                   startDate,
                   endDate);
            Schedule sched = new Schedule(configuration);


            Assert.Equal(expectedDate, sched.GetNextExecutionTime(currentDate));
        }

        [Theory]
        [InlineData(FrequencyType.Weekly, 1, 2, new DayOfWeek[] {DayOfWeek.Monday,DayOfWeek.Wednesday, DayOfWeek.Friday},
            "02:00:00", "04:00:00", "08:00:00", "01/01/2019 00:00:00", "01/01/2020", "01/02/2020", "01/01/2020 00:00:00")]
        [InlineData(FrequencyType.Weekly, 1, 2, new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
            "02:00:00", "04:00:00", "08:00:00", "01/01/2019 04:15:00", "01/01/2020", "01/02/2020", "01/01/2020 04:15:00")]
        [InlineData(FrequencyType.Weekly, 1, 2, new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
            "02:00:00", "04:00:00", "08:00:00", "01/01/2019 08:00:00", "01/01/2020", "01/02/2020", "01/01/2020 08:00:00")]
        [InlineData(FrequencyType.Weekly, 1, 2, new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
            "02:00:00", "04:00:00", "08:00:00", "02/01/2020 08:00:00", "01/01/2020", "01/02/2020", "03/01/2020 04:00:00")]
        [InlineData(FrequencyType.Weekly, 1, 2, new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
            "02:00:00", "04:00:00", "08:00:00", "03/01/2020 08:00:00", "01/01/2020", "01/02/2020", "13/01/2020 04:00:00")]
        [InlineData(FrequencyType.Weekly, 1, 2, new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Thursday, DayOfWeek.Friday },
            "02:00:00", "04:00:00", "08:00:00", "13/01/2020 06:00:00", "01/01/2020", "01/02/2020", "16/01/2020 08:00:00")]
        [InlineData(FrequencyType.Weekly, 1, 2, new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Thursday, DayOfWeek.Friday },
            "02:00:00", "04:00:00", "08:00:00", "16/01/2020 04:00:00", "01/01/2020", "01/02/2020", "17/01/2020 04:00:00")]

        public void ScheduleNextExecution_RecurringWithWeeklyFrequencyRecurring(FrequencyType frequency,int interval, int occurrenceWeekly,
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

            SchedulerConfiguration configuration = new SchedulerConfiguration(
                   SchedulerType.Recurring,
                   frequency,
                   null,
                   interval,
                   dailyFreq,
                   weeklyFreq,
                   startDate,
                   endDate);
            Schedule sched = new Schedule(configuration);


            Assert.Equal(expectedDate, sched.GetNextExecutionTime(currentDate));
        }


        [Theory]
        [InlineData(FrequencyType.Daily, 2, "02:00:00", "00:00:00", "12:00:00", "01/01/2020 00:00:00", "01/01/2020",
            "01/02/2020", "Occurs every 2 days every 2 hours between 00:00:00 and 12:00:00 starting on 01/01/2020")]
        [InlineData(FrequencyType.Daily, 2, "00:10:00", "00:00:00", "12:00:00", "01/01/2020 00:00:00", "01/01/2020",
            "01/02/2020", "Occurs every 2 days every 10 minutes between 00:00:00 and 12:00:00 starting on 01/01/2020")]
        [InlineData(FrequencyType.Daily, 2, "00:00:10", "00:00:00", "12:00:00", "01/01/2020 00:00:00", "01/01/2020",
            "01/02/2020", "Occurs every 2 days every 10 seconds between 00:00:00 and 12:00:00 starting on 01/01/2020")]
        public void ScheduleDescription_RecurringDaily(FrequencyType frequency,int interval , string occurrence, string startTimeStr,
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

            SchedulerConfiguration configuration = new SchedulerConfiguration(
                   SchedulerType.Recurring,
                   frequency,
                   null,
                   interval,
                   dailyFreq,
                   null,
                   startDate,
                   endDate);
            Schedule sched = new Schedule(configuration);

            Assert.Equal(expected, sched.GetDescription(currentDate));
        }

        [Theory]
        [InlineData(FrequencyType.Weekly, 2, 2, new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
            "02:00:00", "00:00:00", "12:00:00", "01/01/2019 00:00:00", "01/01/2020", "01/02/2020",
            "Occurs every 2 weeks on monday, wednesday and friday every 2 hours between 00:00:00 and 12:00:00 starting on 01/01/2020")]
        [InlineData(FrequencyType.Weekly, 2, 4, new DayOfWeek[] { DayOfWeek.Friday },
            "02:00:00", "00:00:00", "12:00:00", "01/01/2019 04:15:00", "01/01/2020", "01/02/2020",
            "Occurs every 4 weeks on friday every 2 hours between 00:00:00 and 12:00:00 starting on 01/01/2020")]
        [InlineData(FrequencyType.Weekly, 2, 2, new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday},
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

            SchedulerConfiguration configuration = new SchedulerConfiguration(
                   SchedulerType.Recurring,
                   frequency,
                   null,
                   interval,
                   dailyFreq,
                   weeklyFreq,
                   startDate,
                   endDate);
            Schedule sched = new Schedule(configuration);


            Assert.Equal(expected, sched.GetDescription(currentDate));
        }

    }
}
