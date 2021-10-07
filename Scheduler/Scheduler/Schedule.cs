using System;

namespace Scheduler
{
    public class Schedule
    {
        private const string ONCE_OCCURRENCE = "Occurs once. Schedule will be used on {0} at {1} starting on {2}";
        private const string DAILY_OCCURRENCE = "Occurs every day. Schedule will be used on {0} at {1} starting on {2}";
        private SchedulerConfig configuration;
        private DateTime latestExecutionTime;
        public Schedule(SchedulerConfig configuration)
        {
            this.configuration = configuration;
        }

        public DateTime GetNextExecutionTime()
        {
            DateTime latestDate = latestExecutionTime;
            if(latestDate == null)
            {
                latestDate = configuration.InitialDateTime > configuration.StartDate
                    ? configuration.InitialDateTime
                    : configuration.StartDate;
            }
            //Only available frequency is daily for now.
            return latestDate.AddDays(configuration.Interval);
        }

        public string GetDescription()
        {
            DateTime nextExecution = GetNextExecutionTime();
            if(configuration.Type == SchedulerType.Once)
            {
                return string.Format(ONCE_OCCURRENCE, nextExecution.Date, nextExecution.Date.TimeOfDay, configuration.StartDate.Date);
            } else
            {
                return string.Format(DAILY_OCCURRENCE, nextExecution.Date, nextExecution.Date.TimeOfDay, configuration.StartDate.Date);
            }
        }
    }

    public struct SchedulerConfig
    {
        public SchedulerType Type { get; set; }
        public DateTime InitialDateTime { get; set; }
        public FrequencyType Frequency { get; set; }
        public int Interval { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public enum SchedulerType
    {
        Once,
        Recurring
    }
    public enum FrequencyType
    {
        Daily
    }
}