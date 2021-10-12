using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
    public class SchedulerConfiguration
    {
        public SchedulerConfiguration(SchedulerType type, FrequencyType frequency, DateTime? dateTimeOnce, int interval, DateTime startDate, DateTime? endDate)
        {
            this.Type = type;
            this.DateTimeOnce = dateTimeOnce;
            this.Frequency = frequency;
            this.Interval = interval;
            this.StartDate = startDate;
            this.EndDate = endDate;
        }

        #region Methods
        public void validateConfiguration()
        {
            if (Interval <= 0)
            {
                throw new ConfigurationException("The specified interval is not valid");
            }
            if (DateTimeOnce?.Date == DateTime.MaxValue.Date)
            {
                throw new ConfigurationException("The specified date is not valid");
            }
            if (StartDate.Date== DateTime.MaxValue.Date)
            {
                throw new ConfigurationException("The specified start date is not valid");
            }
            if (EndDate?.Date == DateTime.MaxValue.Date)
            {
                throw new ConfigurationException("The specified end date is not valid");
            }
            if(EndDate.HasValue && StartDate >= EndDate)
            {
                throw new ConfigurationException("The end date must come after the start date");
            }
            if(Type == SchedulerType.Once && DateTimeOnce.HasValue == false)
            {
                throw new ConfigurationException("Type \"Once\" was indicated, but not the dateTime");
            }
        }
        #endregion

        #region Properties
        public SchedulerType Type { get;}
        public DateTime? DateTimeOnce { get;}
        public FrequencyType Frequency { get;}
        public int Interval { get;}
        public DateTime StartDate { get; }
        public DateTime? EndDate { get;}
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
        Weekly,
        Monthly,
        Yearly
    }
    public class ConfigurationException : Exception
    {
        public ConfigurationException(string message)
            : base(message)
        {
        }
    }
}
