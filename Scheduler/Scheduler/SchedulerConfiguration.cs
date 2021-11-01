using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
    public class SchedulerConfiguration
    {
        public SchedulerConfiguration(SchedulerType type, FrequencyType frequency, DateTime? dateTimeOnce, int interval, DailyFrequency dailyFrequency, WeeklyFrequency weeklyFrequency, DateTime startDate, DateTime? endDate)
        {
            this.Type = type;
            this.DateTimeOnce = dateTimeOnce;
            this.Frequency = frequency;
            this.Interval = interval;
            this.DailyFrequency = dailyFrequency;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.WeeklyFrequency = weeklyFrequency;
        }
        #region Properties
        public SchedulerType Type { get; }
        public DateTime? DateTimeOnce { get; }
        public FrequencyType Frequency { get; }
        public int Interval { get; }
        public DailyFrequency DailyFrequency { get; }
        public WeeklyFrequency WeeklyFrequency { get; }
        public DateTime StartDate { get; }
        public DateTime? EndDate { get; }
        #endregion
    }

    public enum SchedulerType
    {
        Once,
        Recurring
    }
    public enum FrequencyType
    {
        Daily,
        Weekly
    }
    public class ConfigurationException : Exception
    {
        public ConfigurationException(string message)
            : base(message)
        {
        }
    }
}
