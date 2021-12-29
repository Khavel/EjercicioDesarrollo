using System;
using System.Linq;
using System.Threading;

namespace Scheduler
{
    public static class Schedule
    {
        private static TextManager textManager;
        public static DateTime? GetNextExecutionTime(DateTime currentDate, SchedulerConfiguration configuration)
        {
            string culture = string.IsNullOrEmpty(configuration.Culture) ? "EN-US" : configuration.Culture;
            textManager = new TextManager(culture);
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);
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
                throw new ConfigurationException(textManager.GetText("INVALID_STARTDATE"));
            }
            if (configuration.EndDate?.Date == DateTime.MaxValue.Date)
            {
                throw new ConfigurationException(textManager.GetText("INVALID_ENDDATE"));
            }
            if (configuration.EndDate.HasValue && configuration.StartDate >= configuration.EndDate)
            {
                throw new ConfigurationException(textManager.GetText("END_DATE_AFTER_START"));
            }
            if (configuration.Type == SchedulerType.Recurring && configuration.DailyFrequency == null)
            {
                throw new ConfigurationException(textManager.GetText("NO_DAILY_FREQUENCY"));
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
                    return getNextExecutionTimeDaily(dateTimeAux, configuration);

                case FrequencyType.Weekly:
                    return getNextExecutionTimeWeekly(dateTimeAux, configuration);

                case FrequencyType.Monthly:
                    return getNextExecutionTimeMonthly(dateTimeAux, configuration);

                default:
                    throw new ConfigurationException(textManager.GetText("NO_VALID_FREQUENCY"));
            }

            
        }

        private static DateTime? getNextExecutionTimeDaily(DateTime initialDate, SchedulerConfiguration configuration)
        {
            DateTime returnDate;
            if (initialDate.Date >= configuration.StartDate.Date)
            {
                returnDate = initialDate;
            }
            else
            {
                returnDate = initialDate.AddDays(1);
            }

            if (returnDate >= configuration.EndDate)
            {
                return null;
            }
            else
            {
                if (configuration.DailyFrequency?.IsRecurring == false)
                {
                    if (returnDate.TimeOfDay >= configuration.DailyFrequency?.Occurrence)
                    {
                        return GetNextExecutionTime(returnDate.Date.AddDays(1), configuration);
                    }
                    return returnDate.Date + configuration.DailyFrequency?.Occurrence;
                }
                else
                {
                    return getNextTimeDaily(returnDate, configuration);
                }

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
            if (theDate.TimeOfDay >= configuration.DailyFrequency?.EndTime)
            {
                return GetNextExecutionTime(theDate.Date.AddDays(1), configuration);
            }
            else if (theDate.TimeOfDay < configuration.DailyFrequency?.StartTime)
            {
                return theDate.Date + configuration.DailyFrequency?.StartTime;
            }
            else
            {
                return theDate.Date + Calculator.GetDailyExecutionTimes(configuration.DailyFrequency).First(T => T > theDate.TimeOfDay);
            }
        }

        private static DateTime? getNextExecutionTimeWeekly(DateTime theDate, SchedulerConfiguration configuration)
        {
            if (configuration.Type == SchedulerType.Recurring && configuration.Frequency == FrequencyType.Weekly &&
                configuration.WeeklyFrequency == null)
            {
                throw new ConfigurationException(textManager.GetText("NO_WEEKLY_FREQUENCY"));
            }
            if (configuration.WeeklyFrequency.Occurrence <= 0)
            {
                throw new ConfigurationException(textManager.GetText("INCORRECT_WEEKLY_FREQUENCY"));
            }

            if (configuration.WeeklyFrequency.DaysOfWeek == null || configuration.WeeklyFrequency.DaysOfWeek.Length == 0)
            {
                throw new ConfigurationException(textManager.GetText("NO_VALID_WEEKDAY"));
            }

            if (configuration.WeeklyFrequency.DaysOfWeek.Any(T => T == theDate.DayOfWeek))
            {
                return getNextTimeDaily(theDate,configuration);
            }

            int weekOfYearStart = Calculator.GetWeekOfYear(configuration.StartDate);

            DateTime theDateAux = theDate;
            int counter = 0;
            while (counter < 365)
            {
                theDateAux = theDateAux.AddDays(1);
                if (configuration.WeeklyFrequency.DaysOfWeek.Any(T => T == theDateAux.DayOfWeek)
                    && (Calculator.GetWeekOfYear(theDateAux) - weekOfYearStart) % configuration.WeeklyFrequency.Occurrence == 0)
                {
                    return getNextTimeDaily(theDateAux,configuration);
                }
                counter++;
            }
            return getNextExecutionTimeWeekly(theDateAux, configuration);
        }

        private static DateTime? getNextExecutionTimeMonthly(DateTime theDate, SchedulerConfiguration configuration)
        {
            if (configuration.Type == SchedulerType.Recurring && configuration.Frequency == FrequencyType.Monthly &&
                configuration.MonthlyFrequency == null)
            {
                throw new ConfigurationException(textManager.GetText("NO_MONTHLY_FREQUENCY"));
            }
            if (configuration.MonthlyFrequency.IsDaily && configuration.MonthlyFrequency.DayNumber <= 0)
            {
                throw new ConfigurationException(textManager.GetText("INVALID_DAY"));
            }
            if (configuration.MonthlyFrequency.Interval <= 0)
            {
                throw new ConfigurationException(textManager.GetText("INVALID_INTERVAL"));
            }
            if (configuration.MonthlyFrequency.IsDaily == false && configuration.MonthlyFrequency.Frequency.HasValue == false)
            {
                throw new ConfigurationException(textManager.GetText("NO_FREQUENCY"));
            }
            if (configuration.MonthlyFrequency.IsDaily == false && configuration.MonthlyFrequency.DayType.HasValue == false)
            {
                throw new ConfigurationException(textManager.GetText("NO_DAY_TYPE"));
            }

            if (theDate < configuration.StartDate)
            {
                theDate = configuration.StartDate;
            }
            DateTime? nextDate = Calculator.GetMonthlyExecutionDates(configuration.StartDate, configuration.MonthlyFrequency)
                .FirstOrDefault(T => T >= theDate.Date);
            if(nextDate.HasValue == false)
            {
                return null;
            }
            if(nextDate.Value == theDate.Date)
            {
                return getNextTimeDaily(theDate, configuration);
            }
            else
            {
                return getNextTimeDaily(nextDate.Value, configuration);
            }
        }

        public static string GetDescription(DateTime currentDate, SchedulerConfiguration configuration)
        {
            DateTime? nextExecution = GetNextExecutionTime(currentDate, configuration);

            if (nextExecution.HasValue == false)
            {
                return string.Format(textManager.GetText("DATE_OVER_END"), configuration.EndDate.Value.ToShortDateString());
            }

            if (configuration.Type == SchedulerType.Once)
            {
                return string.Format(textManager.GetText("ONCE_OCCURRENCE"), nextExecution.Value.ToShortDateString(), nextExecution.Value.ToShortTimeString());
            }
            else
            {
                switch (configuration.Frequency)
                {
                    case FrequencyType.Daily:
                        return $"{textManager.GetText("OCCURS_EVERYDAY")} {Descriptor.GetDailyDescription(configuration.DailyFrequency, textManager)} {textManager.GetText("STARTING_ON")} {configuration.StartDate.ToString("dd/MM/yyyy")}";
                    case FrequencyType.Weekly:
                        return Descriptor.GetWeeklyDescription(configuration.WeeklyFrequency, textManager) + Descriptor.GetDailyDescription(configuration.DailyFrequency, textManager) +
                            $" {textManager.GetText("STARTING_ON")} {configuration.StartDate.ToString("dd/MM/yyyy")}";
                    case FrequencyType.Monthly:
                        return Descriptor.GetMonthlyDescription(configuration.MonthlyFrequency, textManager) + Descriptor.GetDailyDescription(configuration.DailyFrequency, textManager) +
                            $" {textManager.GetText("STARTING_ON")} {configuration.StartDate.ToString("dd/MM/yyyy")}";
                    default:
                        throw new ScheduleException(textManager.GetText("NO_VALID_FREQUENCY"));
                }
            }
        }
    }


}