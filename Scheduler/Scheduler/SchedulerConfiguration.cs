using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
    public class SchedulerConfiguration
    {
        public SchedulerConfiguration() { }
        #region Properties
        public SchedulerType Type { get; set; }
        public DateTime? DateTimeOnce { get; set; }
        public FrequencyType Frequency { get; set; }
        public int Interval { get; set; }
        public DailyFrequency DailyFrequency { get; set; }
        public WeeklyFrequency WeeklyFrequency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
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
