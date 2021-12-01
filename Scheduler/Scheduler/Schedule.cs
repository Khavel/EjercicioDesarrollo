using System;
using System.Linq;

namespace Scheduler
{
    public static class Schedule
    {
        private const string ONCE_OCCURRENCE = "Occurs once. Schedule will be used on {0} at {1}";
        private const string DATE_OVER_END = "Will not occur. Schedule will end on {0}";

        public static DateTime? GetNextExecutionTime(DateTime currentDate, SchedulerConfiguration configuration)
        {
            if (configuration.Type == SchedulerType.Once)
            {
                return GetNextExecutionTimeOnce(currentDate, configuration);
            }
            return GetNextExecutionTimeRecurring(currentDate, configuration);
        }

        private static DateTime? GetNextExecutionTimeRecurring(DateTime currentDate, SchedulerConfiguration configuration)
        {
            #region Validations
            if (configuration.StartDate.Date == DateTime.MaxValue.Date)
            {
                throw new ConfigurationException("The specified start date is not valid");
            }
            if (configuration.EndDate?.Date == DateTime.MaxValue.Date)
            {
                throw new ConfigurationException("The specified end date is not valid");
            }
            if (configuration.EndDate.HasValue && configuration.StartDate >= configuration.EndDate)
            {
                throw new ConfigurationException("The end date must come after the start date");
            }
            if (configuration.Type == SchedulerType.Recurring && configuration.DailyFrequency == null)
            {
                throw new ConfigurationException("No daily frequency was specified");
            }
            if (configuration.Type == SchedulerType.Recurring && configuration.Frequency == FrequencyType.Weekly &&
                configuration.WeeklyFrequency == null)
            {
                throw new ConfigurationException("No weekly frequency was specified");
            }
            #endregion
            DateTime dateTimeAux = currentDate;

            if (currentDate.Date < configuration.StartDate.Date)
            {
                dateTimeAux = new DateTime(configuration.StartDate.Year, configuration.StartDate.Month, configuration.StartDate.Day);
            }

            switch (configuration.Frequency)
            {
                case FrequencyType.Daily:
                    dateTimeAux = getNextDateDaily(dateTimeAux, configuration);
                    break;
                case FrequencyType.Weekly:
                    dateTimeAux = getNextDateWeekly(dateTimeAux, configuration);
                    break;
            }

            //This part determines the next date of execution
            DateTime returnDate = dateTimeAux;
            //returnDate = addInterval(dateTimeAux, configuration);
            if (returnDate >= configuration.EndDate)
            {
                return null;
            }
            else
            {
                //This part determines the next time of execution, for daily frequency configurations
                if (configuration.DailyFrequency?.IsRecurring == false)
                {
                    if (returnDate.TimeOfDay >= configuration.DailyFrequency?.Occurrence)
                    {
                        return GetNextExecutionTime(dateTimeAux.Date.AddDays(1), configuration);
                    }

                    return returnDate.Date + configuration.DailyFrequency?.Occurrence;
                }
                else
                {
                    //If the current time is higher than the end time, move to next date of execution
                    if (returnDate.TimeOfDay >= configuration.DailyFrequency?.EndTime)
                    {
                        return GetNextExecutionTime(dateTimeAux.Date.AddDays(1), configuration);
                    }
                    else if (returnDate.TimeOfDay < configuration.DailyFrequency?.StartTime)
                    {
                        return returnDate.Date + configuration.DailyFrequency?.StartTime;
                    }
                    else
                    {
                        return getNextTimeDaily(returnDate, configuration);
                    }
                }

            }
        }

        private static DateTime getNextDateDaily(DateTime initialDate, SchedulerConfiguration configuration)
        {
            if (initialDate.Date >= configuration.StartDate.Date)
            {
                return initialDate;
            }
            else
            {
                return initialDate.AddDays(1);
            }

        }

        public static DateTime?[] GetMultipleNextExecutionTimes(DateTime currentDate, SchedulerConfiguration configuration, int numberOfTimes)
        {
            DateTime?[] executionTimes = new DateTime?[numberOfTimes];
            if (numberOfTimes == 0)
            {
                return executionTimes;
            }
            executionTimes[0] = GetNextExecutionTime(currentDate, configuration);
            if (numberOfTimes == 1)
            {
                return executionTimes;
            }
            for (int i = 1; i < numberOfTimes; i++)
            {
                executionTimes[i] = executionTimes[i - 1].HasValue
                    ? GetNextExecutionTime(executionTimes[i - 1].Value, configuration)
                    : null;
            }
            return executionTimes;
        }

        private static DateTime GetNextExecutionTimeOnce(DateTime currentDate, SchedulerConfiguration configuration)
        {
            if (configuration.DateTimeOnce.HasValue == false || configuration.DateTimeOnce?.Date == DateTime.MaxValue.Date)
            {
                throw new ConfigurationException("The specified date is not valid");
            }

            if (currentDate > configuration.DateTimeOnce)
            {
                return currentDate;
            }
            else
            {
                return configuration.DateTimeOnce.Value;
            }
        }

        private static DateTime? getNextTimeDaily(DateTime theDate, SchedulerConfiguration configuration)
        {
            return theDate.Date + configuration.DailyFrequency.DailyExecutionTimes.First(T => T > theDate.TimeOfDay);
        }

        private static DateTime getNextDateWeekly(DateTime theDate, SchedulerConfiguration configuration)
        {

            if (configuration.WeeklyFrequency.Occurrence <= 0)
            {
                throw new ConfigurationException("Incorrect weekly frequency");
            }

            if (configuration.WeeklyFrequency.DaysOfWeek == null || configuration.WeeklyFrequency.DaysOfWeek.Length == 0)
            {
                throw new ConfigurationException("No valid day of the week was indicated");
            }

            if (configuration.WeeklyFrequency.DaysOfWeek.Any(T => T == theDate.DayOfWeek))
            {
                return theDate;
            }

            DateTime theDateAux = theDate.AddDays(1);
            while (theDateAux.DayOfWeek != theDate.DayOfWeek)
            {
                if (configuration.WeeklyFrequency.DaysOfWeek.Any(T => T == theDateAux.DayOfWeek))
                {
                    return theDateAux;
                }
                theDateAux = theDateAux.AddDays(1);
            }
            theDateAux = theDateAux.AddDays(((configuration.WeeklyFrequency.Occurrence - 1) * 7) + 1);
            if (configuration.WeeklyFrequency.DaysOfWeek.Any(T => T == theDateAux.DayOfWeek))
            {
                return theDateAux;
            }
            return getNextDateWeekly(theDateAux, configuration);
        }

        public static string GetDescription(DateTime currentDate, SchedulerConfiguration configuration)
        {
            DateTime? nextExecution = GetNextExecutionTime(currentDate, configuration);

            if (nextExecution.HasValue == false)
            {
                return string.Format(DATE_OVER_END, configuration.EndDate.Value.ToShortDateString());
            }

            if (configuration.Type == SchedulerType.Once)
            {
                return string.Format(ONCE_OCCURRENCE, nextExecution.Value.ToShortDateString(), nextExecution.Value.ToShortTimeString());
            }
            else
            {
                switch (configuration.Frequency)
                {
                    case FrequencyType.Daily:
                        return $"Occurs everyday {configuration.DailyFrequency.Description} starting on {configuration.StartDate.ToString("dd/MM/yyyy")}";
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