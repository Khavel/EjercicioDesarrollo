using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
    public enum SchedulerType
    {
        Once,
        Recurring
    }
    public enum FrequencyType
    {
        Daily,
        Weekly,
        Monthly
    }
    public enum MonthlyFrequencyType
    {
        First,
        Second,
        Third,
        Fourth,
        Last
    }
    public enum MonthlyDayType
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday,
        Day,
        Weekday,
        WeekendDay
    }
    public class ConfigurationException : Exception
    {
        public ConfigurationException(string message)
            : base(message)
        {
        }
    }
    public class ScheduleException : Exception
    {
        public ScheduleException(string message)
            : base(message)
        {
        }
    }
}
