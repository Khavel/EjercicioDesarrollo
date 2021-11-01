using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
    public static class Validator
    {
        public static void ValidateBasicConfiguration(SchedulerConfiguration config)
        {
            if (config.Interval < 0)
            {
                throw new ConfigurationException("The specified interval is not valid");
            }
            if (config.DateTimeOnce?.Date == DateTime.MaxValue.Date)
            {
                throw new ConfigurationException("The specified date is not valid");
            }
            if (config.StartDate.Date == DateTime.MaxValue.Date)
            {
                throw new ConfigurationException("The specified start date is not valid");
            }
            if (config.EndDate?.Date == DateTime.MaxValue.Date)
            {
                throw new ConfigurationException("The specified end date is not valid");
            }
            if (config.EndDate.HasValue && config.StartDate >= config.EndDate)
            {
                throw new ConfigurationException("The end date must come after the start date");
            }
            if (config.Type == SchedulerType.Once && config.DateTimeOnce.HasValue == false)
            {
                throw new ConfigurationException("Type \"Once\" was indicated, but not the dateTime");
            }
        }

        public static void ValidateDailyConfiguration(SchedulerConfiguration config)
        {
            if(config.Type == SchedulerType.Recurring && config.DailyFrequency == null)
            {
                throw new ConfigurationException("No daily frequency was specified");
            }

            if (config.DailyFrequency.IsRecurring == true)
            {
                if(config.DailyFrequency.StartTime == null)
                {
                    throw new ConfigurationException("Daily configuration set to recurring, but no start time specified");
                }
                if (config.DailyFrequency.EndTime == null)
                {
                    throw new ConfigurationException("Daily configuration set to recurring, but no end time specified");
                }
            }
        }

        public static void ValidateWeeklyConfiguration(SchedulerConfiguration config)
        {
            if (config.Type == SchedulerType.Recurring && config.Frequency == FrequencyType.Weekly && 
                config.WeeklyFrequency == null)
            {
                throw new ConfigurationException("No weekly frequency was specified");
            }

            if (config.WeeklyFrequency.Occurrence <= 0)
            {
                throw new ConfigurationException("Incorrect weekly frequency");
            }

            if(config.WeeklyFrequency.DaysOfWeek == null || config.WeeklyFrequency.DaysOfWeek.Length == 0)
            {
                throw new ConfigurationException("No valid day of the week was indicated");
            }
        }
    }
}
