using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Scheduler
{
    public static class Calculator
    {
        public static TimeSpan[] GetDailyExecutionTimes(DailyFrequency freq)
        {
            List<TimeSpan> executionTimesAux = new List<TimeSpan>();
            for (var ts = freq.StartTime; ts <= freq.EndTime; ts += freq.Occurrence)
            {
                executionTimesAux.Add(ts.Value);
            }
            return executionTimesAux.ToArray();
        }
        public static DayOfWeek[] GetDaysOfWeekOrdered(DayOfWeek[] daysOfWeek)
        {
            return daysOfWeek.OrderBy(D => ((int)D + 6) % 7).ToArray();
        }

        public static int GetWeekOfYear(DateTime date)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                date = date.AddDays(3);
            }
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static int GetWeeksInYear(int year)
        {
            DateTimeFormatInfo theFormatInfo = DateTimeFormatInfo.CurrentInfo;
            DateTime lastDayOfYear = new DateTime(year, 12, 31);
            Calendar cal = theFormatInfo.Calendar;
            return cal.GetWeekOfYear(lastDayOfYear, theFormatInfo.CalendarWeekRule,
                                                theFormatInfo.FirstDayOfWeek);
        }

        public static DateTime[] GetMonthlyExecutionDates(DateTime startDate, MonthlyFrequency freq)
        {
            List<DateTime> theDates = new List<DateTime>();
            if (freq.IsDaily)
            {
                int month = startDate.Month;
                int cont = 0;
                int yearsToAdd = 0;
                while (cont < 24)
                {
                    if ((cont % freq.Interval == 0) && (DateTime.DaysInMonth(startDate.Year + yearsToAdd, month) >= freq.DayNumber))
                    {
                        theDates.Add(new DateTime(startDate.Year + yearsToAdd, month, freq.DayNumber));
                    }
                    month++;
                    if (month == 13)
                    {
                        month = 1;
                        yearsToAdd++;
                    }
                    cont++;
                }
                return theDates.ToArray();
            }
            else
            {
                int month = startDate.Month;
                int cont = 0;
                int yearsToAdd = 0;
                while (cont < 24)
                {
                    if ((cont % freq.Interval == 0) && (GetDayInMonth(month, startDate.Year + yearsToAdd,freq).HasValue))
                    {
                        theDates.Add(GetDayInMonth(month, startDate.Year + yearsToAdd,freq).Value);
                    }
                    month++;
                    if (month == 13)
                    {
                        month = 1;
                        yearsToAdd++;
                    }
                    cont++;
                }
                return theDates.ToArray();
            }
        }

        private static DateTime? GetDayInMonth(int month, int year,MonthlyFrequency freq)
        {
            List<DateTime> validDays = new List<DateTime>();
            for (DateTime date = new DateTime(year, month, 01); date < new DateTime(year, month, 01).AddMonths(1); date = date.AddDays(1))
            {
                if (IsValidDay(date, freq.DayType))
                {
                    validDays.Add(date);
                }
            }
            switch (freq.Frequency)
            {
                case MonthlyFrequencyType.First:
                    return validDays[0];

                case MonthlyFrequencyType.Second:
                    return validDays[1];

                case MonthlyFrequencyType.Third:
                    return validDays[2];

                case MonthlyFrequencyType.Fourth:
                    return validDays[3];

                case MonthlyFrequencyType.Last:
                    return validDays.Last();

                default:
                    return null;
            }
        }

        private static bool IsValidDay(DateTime day, MonthlyDayType? dayType)
        {
            switch (dayType)
            {
                case MonthlyDayType.Day:
                    return true;

                case MonthlyDayType.Monday:
                    return day.DayOfWeek == DayOfWeek.Monday;

                case MonthlyDayType.Tuesday:
                    return day.DayOfWeek == DayOfWeek.Tuesday;

                case MonthlyDayType.Wednesday:
                    return day.DayOfWeek == DayOfWeek.Wednesday;

                case MonthlyDayType.Thursday:
                    return day.DayOfWeek == DayOfWeek.Thursday;

                case MonthlyDayType.Friday:
                    return day.DayOfWeek == DayOfWeek.Friday;

                case MonthlyDayType.Saturday:
                    return day.DayOfWeek == DayOfWeek.Saturday;

                case MonthlyDayType.Sunday:
                    return day.DayOfWeek == DayOfWeek.Sunday;

                case MonthlyDayType.Weekday:
                    return new DayOfWeek[] { DayOfWeek.Saturday, DayOfWeek.Sunday }.Contains(day.DayOfWeek) == false;

                case MonthlyDayType.WeekendDay:
                    return new DayOfWeek[] { DayOfWeek.Saturday, DayOfWeek.Sunday }.Contains(day.DayOfWeek) == true;

                default:
                    return false;

            }
        }
    }
}
