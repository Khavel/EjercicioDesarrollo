using System;

namespace Scheduler
{
    public class Schedule
    {
        private const string ONCE_OCCURRENCE = "Occurs once. Schedule will be used on {0} at {1} starting on {2}";
        private const string RECURRING_OCCURRENCE = "Occurs {0}. Schedule will be used on {1} starting on {2}";
        private const string DATE_OVER_END = "Will not occur. Schedule will end on {0}";

        private SchedulerConfiguration configuration;
        public Schedule(SchedulerConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public DateTime? GetNextExecutionTime(DateTime currentDate)
        {
            if (this.configuration.Type == SchedulerType.Once)
            {
                if (currentDate > configuration.DateTimeOnce)
                {
                    return currentDate;
                }
                else
                {
                    return configuration.DateTimeOnce.Value;
                }
            }

            if (currentDate.Date < configuration.StartDate.Date)
            {
                DateTime startDateCurrentTime = new DateTime(configuration.StartDate.Year, configuration.StartDate.Month, configuration.StartDate.Day);
                startDateCurrentTime += currentDate.TimeOfDay;
                return startDateCurrentTime;
            }

            DateTime returnDate;
            switch (configuration.Frequency)
            {
                case FrequencyType.Daily:
                    returnDate = currentDate.AddDays(configuration.Interval);
                    break;
                case FrequencyType.Weekly:
                    returnDate = currentDate.AddDays(configuration.Interval * 7);
                    break;
                case FrequencyType.Monthly:
                    returnDate = currentDate.AddMonths(configuration.Interval);
                    break;
                case FrequencyType.Yearly:
                    returnDate = currentDate.AddYears(configuration.Interval);
                    break;
                default:
                    throw new ScheduleException("No valid frequency was specified");
            }
            if(returnDate > configuration.EndDate)
            {
                return null;
            }
            else
            {
                return returnDate;
            }

        }

        public string GetDescription(DateTime currentDate)
        {
            DateTime? nextExecution = GetNextExecutionTime(currentDate);

            if(nextExecution.HasValue == false)
            {
                return string.Format(DATE_OVER_END, configuration.EndDate.Value.ToShortDateString());
            }

            if (configuration.Type == SchedulerType.Once)
            {
                return string.Format(ONCE_OCCURRENCE, nextExecution.Value.ToShortDateString(), nextExecution.Value.ToShortTimeString(), configuration.StartDate.ToShortDateString());
            }
            else
            {
                switch (configuration.Frequency)
                {
                    case FrequencyType.Daily:
                        return string.Format(RECURRING_OCCURRENCE,"every day", nextExecution.Value.ToShortDateString(), configuration.StartDate.ToShortDateString());

                    case FrequencyType.Weekly:
                        return string.Format(RECURRING_OCCURRENCE,"every week", nextExecution.Value.ToShortDateString(), configuration.StartDate.ToShortDateString());

                    case FrequencyType.Monthly:
                        return string.Format(RECURRING_OCCURRENCE,"every month", nextExecution.Value.ToShortDateString(), configuration.StartDate.ToShortDateString());

                    case FrequencyType.Yearly:
                        return string.Format(RECURRING_OCCURRENCE,"every year", nextExecution.Value.ToShortDateString(), configuration.StartDate.ToShortDateString());

                    default:
                        throw new ScheduleException("No valid frequency was specified");
                }
            }
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