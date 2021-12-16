using System;
using System.Collections.Generic;
using System.Linq;

namespace Scheduler
{
    public class MonthlyFrequency
    {
        #region Constructor
        public MonthlyFrequency()
        {

        }
        #endregion
        #region Properties
        public bool IsDaily { get; set; }
        public int DayNumber { get; set; }
        public int Interval { get; set; }
        public MonthlyFrequencyType? Frequency { get; set; }
        public MonthlyDayType? DayType { get; set; }
        #endregion
        #region Methods
        public DateTime[] GetExecutionDates(DateTime startDate)
        {
            List<DateTime> theDates = new List<DateTime>();
            if (IsDaily)
            {
                int month = startDate.Month;
                int cont = 0;
                while(month < 13)
                {
                    if((cont % Interval == 0) && (DateTime.DaysInMonth(startDate.Year, month) >= DayNumber))
                    {
                        theDates.Add(new DateTime(startDate.Year, month, DayNumber));
                    }
                    month++;
                    cont++;
                }
                return theDates.ToArray();
            }
            else
            {
                int month = startDate.Month;
                int cont = 0;
                while (month < 13)
                {
                    if ((cont % Interval == 0) && (GetDayInMonth(month, startDate.Year).HasValue))
                    {
                        theDates.Add(GetDayInMonth(month, startDate.Year).Value);
                    }
                    month++;
                    cont++;
                }
                return theDates.ToArray();
            }
        }

        private DateTime? GetDayInMonth(int month, int year)
        {
            List<DateTime> validDays = new List<DateTime>();
            for (DateTime date = new DateTime(year, month, 01); date < new DateTime(year, month, 01).AddMonths(1); date = date.AddDays(1))
            {
                if (IsValidDay(date, month, year))
                {
                    validDays.Add(date);
                }
            }
            switch (Frequency)
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

        private bool IsValidDay(DateTime day, int month, int year)
        {
            switch (DayType)
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

        public string GetDescription()
        {
            if (IsDaily)
            {
                return $"Occurs the {DayNumber} of every {Interval} {(Interval > 1 ? "months" : "month")} ";
            }
            else
            {
                return $"Occurs the {Frequency} {DayType} of every {Interval} {(Interval > 1 ? "months" : "month")} ";
            }
        }
        #endregion
    }
}
