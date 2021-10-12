using System;
using Xunit;
using Scheduler;

namespace TestScheduler
{
    public class SchedulerTest
    {
        [Theory]
        [InlineData(SchedulerType.Once, FrequencyType.Daily, "01/01/2020", -1, "01/01/2020", "01/02/2020")]
        [InlineData(SchedulerType.Once, FrequencyType.Daily, "01/01/2020", 1, "01/01/2020", "01/02/2019")]
        [InlineData(SchedulerType.Once, FrequencyType.Daily, "31/12/9999", 1, "01/01/2020", "01/01/2021")]
        [InlineData(SchedulerType.Once, FrequencyType.Daily, "01/01/2020", 1, "31/12/9999", "01/01/2021")]
        public void SchedulerConfigurationValidation(SchedulerType type,FrequencyType frequency, string dateTimeStr, int interval,string startDateStr, string endDateStr)
        {
            DateTime dateTime = DateTime.ParseExact(dateTimeStr,"dd/MM/yyyy",null);
            DateTime startDate = DateTime.ParseExact(startDateStr, "dd/MM/yyyy", null);
            DateTime endDate = DateTime.ParseExact(endDateStr, "dd/MM/yyyy", null);

            SchedulerConfiguration configuration = new SchedulerConfiguration(
                   type,
                   frequency,
                   dateTime,
                   interval,
                   startDate,
                   endDate);

            Assert.Throws<ConfigurationException>(() => configuration.validateConfiguration());
        }

        [Theory]
        [InlineData("08/01/2020 14:00", "01/01/2020", "04/01/2020","08/01/2020 14:00")]
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
                   startDate,
                   null);
            Schedule sched = new Schedule(configuration);


            Assert.Equal(expected, sched.GetDescription(currentDate));
        }

        [Theory]
        [InlineData(FrequencyType.Daily, 1, "04/01/2020", "01/01/2020", "01/02/2020", "05/01/2020")]
        [InlineData(FrequencyType.Daily, 1, "05/01/2020", "01/01/2020", "01/02/2020", "06/01/2020")]
        [InlineData(FrequencyType.Daily, 1, "06/01/2020", "01/01/2020", "01/02/2020", "07/01/2020")]
        [InlineData(FrequencyType.Weekly, 1, "01/01/2020", "01/01/2020", "01/01/2021", "08/01/2020")]
        [InlineData(FrequencyType.Weekly, 2, "01/01/2020", "01/01/2020", "01/01/2021", "15/01/2020")]
        [InlineData(FrequencyType.Monthly, 1, "01/01/2020", "01/01/2020", "01/01/2021", "01/02/2020")]
        [InlineData(FrequencyType.Monthly, 2, "01/12/2020", "01/01/2020", "01/01/2021", null)]
        [InlineData(FrequencyType.Yearly, 1, "01/01/2020", "01/01/2020", "01/01/2021", "01/01/2021")]
        [InlineData(FrequencyType.Yearly, 2, "01/12/2020", "01/01/2020", "01/01/2021", null)]
        [InlineData(FrequencyType.Yearly, 2, "01/12/2020", "01/01/2020", null, "01/12/2022")]
        public void ScheduleNextExecution_Recurring(FrequencyType frequency, int interval, string currentDateStr, string startDateStr, string endDateStr, string expected)
        {
            DateTime startDate = DateTime.ParseExact(startDateStr, "d", null);
            DateTime currentDate = DateTime.ParseExact(currentDateStr, "d", null);
            DateTime? endDate = null;
            if (string.IsNullOrEmpty(endDateStr) == false)
            {
                endDate = DateTime.ParseExact(endDateStr, "d", null);
            }
            
            DateTime? expectedDate = null;
            if (string.IsNullOrEmpty(expected) == false)
            {
                expectedDate = DateTime.ParseExact(expected, "d", null);
            }


            SchedulerConfiguration configuration = new SchedulerConfiguration(
                   SchedulerType.Recurring,
                   frequency,
                   null,
                   interval,
                   startDate,
                   endDate);
            Schedule sched = new Schedule(configuration);


            Assert.Equal(expectedDate, sched.GetNextExecutionTime(currentDate));
        }

        [Theory]
        [InlineData(FrequencyType.Daily, 1, "04/01/2020", "01/01/2020", "01/02/2020", "Occurs every day. Schedule will be used on 05/01/2020 starting on 01/01/2020")]
        [InlineData(FrequencyType.Daily, 1, "05/01/2020", "01/01/2020", "01/02/2020", "Occurs every day. Schedule will be used on 06/01/2020 starting on 01/01/2020")]
        [InlineData(FrequencyType.Daily, 1, "06/01/2020", "01/01/2020", "01/02/2020", "Occurs every day. Schedule will be used on 07/01/2020 starting on 01/01/2020")]
        [InlineData(FrequencyType.Weekly, 1, "01/01/2020", "01/01/2020", "01/01/2021", "Occurs every week. Schedule will be used on 08/01/2020 starting on 01/01/2020")]
        [InlineData(FrequencyType.Monthly, 1, "01/01/2020", "01/01/2020", "01/01/2021", "Occurs every month. Schedule will be used on 01/02/2020 starting on 01/01/2020")]
        [InlineData(FrequencyType.Yearly, 1, "01/01/2020", "01/01/2020", "01/02/2021", "Occurs every year. Schedule will be used on 01/01/2021 starting on 01/01/2020")]
        [InlineData(FrequencyType.Daily, 1, "01/01/2020", "01/01/2020", "01/01/2020", "Will not occur. Schedule will end on 01/01/2020")]
        [InlineData(FrequencyType.Weekly, 1, "01/01/2020", "01/01/2020", "05/01/2020", "Will not occur. Schedule will end on 05/01/2020")]
        [InlineData(FrequencyType.Monthly, 1, "01/01/2020", "01/01/2020", "25/01/2020", "Will not occur. Schedule will end on 25/01/2020")]
        [InlineData(FrequencyType.Yearly, 5, "01/01/2020", "01/01/2020", "01/02/2021", "Will not occur. Schedule will end on 01/02/2021")]
        public void ScheduleDescription_Recurring(FrequencyType frequency, int interval, string currentDateStr, string startDateStr, string endDateStr, string expected)
        {
            DateTime startDate = DateTime.ParseExact(startDateStr, "d", null);
            DateTime currentDate = DateTime.ParseExact(currentDateStr, "d", null);
            DateTime endDate = DateTime.ParseExact(endDateStr, "d", null);


            SchedulerConfiguration configuration = new SchedulerConfiguration(
                   SchedulerType.Recurring,
                   frequency,
                   null,
                   interval,
                   startDate,
                   endDate);
            Schedule sched = new Schedule(configuration);


            Assert.Equal(expected, sched.GetDescription(currentDate));
        }
    }
}
