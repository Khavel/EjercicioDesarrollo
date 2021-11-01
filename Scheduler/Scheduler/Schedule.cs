using System;
using System.Linq;

namespace Scheduler
{
    public class Schedule
    {
        private const string ONCE_OCCURRENCE = "Occurs once. Schedule will be used on {0} at {1} starting on {2}";
        private const string DATE_OVER_END = "Will not occur. Schedule will end on {0}";

        private SchedulerConfiguration configuration;
        public Schedule(SchedulerConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public DateTime? GetNextExecutionTime(DateTime currentDate)
        {
            #region Execution time for Once
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
            #endregion
            #region Execution time for Recurring
            DateTime initialDate = currentDate;

            if (currentDate.Date < configuration.StartDate.Date)
            {
                DateTime startDateCurrentTime = new DateTime(configuration.StartDate.Year, configuration.StartDate.Month, configuration.StartDate.Day);
                startDateCurrentTime += currentDate.TimeOfDay;
                return startDateCurrentTime;
            }

            //This part determines the next date of execution
            DateTime returnDate;
            returnDate = addInterval(initialDate);
            if (returnDate > configuration.EndDate)
            {
                return null;
            }
            else
            {
                //This part determines the next time of execution, for daily frequency configurations
                if (this.configuration.DailyFrequency == null)
                {
                    return returnDate;
                }
                else
                {
                    if (configuration.DailyFrequency?.IsRecurring == false)
                    {
                        return returnDate.Date + configuration.DailyFrequency?.Occurrence;
                    }
                    else
                    {
                        //If the current time is higher than the end time, move to next date of execution
                        if (returnDate.TimeOfDay >= configuration.DailyFrequency?.EndTime)
                        {
                            return GetNextExecutionTime(initialDate.Date);
                        }
                        else if (returnDate.TimeOfDay <= configuration.DailyFrequency?.StartTime)
                        {
                            return returnDate.Date + configuration.DailyFrequency?.StartTime;
                        }
                        else
                        {
                            return getNextTimeDaily(returnDate);
                        }
                    }
                }
            }
            #endregion
        }

        private DateTime getNextTimeDaily(DateTime theDate)
        {
            return theDate.Date + configuration.DailyFrequency.DailyExecutionTimes.First(T => T > theDate.TimeOfDay);
        }

        private DateTime getNextDateWeekly(DateTime theDate)
        {
            DateTime theDateAux = theDate;
            while (theDateAux.DayOfWeek != DayOfWeek.Sunday)
            {
                theDateAux = theDateAux.AddDays(1);
                if (configuration.WeeklyFrequency.DaysOfWeek.Any(T => T == theDateAux.DayOfWeek))
                {
                    return theDateAux;
                }
            }
            theDateAux = theDateAux.AddDays(((configuration.WeeklyFrequency.Occurrence-1)*7)+1);
            if (configuration.WeeklyFrequency.DaysOfWeek.Any(T => T == theDateAux.DayOfWeek))
            {
                return theDateAux;
            }
            return getNextDateWeekly(theDateAux);
        }

        private DateTime addInterval(DateTime currentDate)
        {
            DateTime returnDate;
            switch (configuration.Frequency)
            {
                case FrequencyType.Daily:
                    returnDate = currentDate.AddDays(configuration.Interval);
                    break;
                case FrequencyType.Weekly:
                    if (configuration.WeeklyFrequency.DaysOfWeek.Any(T => T == currentDate.DayOfWeek))
                    {
                        returnDate = currentDate;
                    }
                    returnDate = getNextDateWeekly(currentDate);
                    break;
                default:
                    throw new ScheduleException("No valid frequency was specified");
            }
            return returnDate;
        }

        public string GetDescription(DateTime currentDate)
        {
            DateTime? nextExecution = GetNextExecutionTime(currentDate);

            if (nextExecution.HasValue == false)
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
                        return $"Occurs every {configuration.Interval} days {configuration.DailyFrequency.Description} starting on {configuration.StartDate.ToString("dd/MM/yyyy")}";
                    case FrequencyType.Weekly:
                        return configuration.WeeklyFrequency.Description + configuration.DailyFrequency.Description +
                            $" starting on {configuration.StartDate.ToString("dd/MM/yyyy")}";
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