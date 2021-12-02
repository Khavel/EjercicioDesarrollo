using System;
using System.Globalization;
using System.Linq;

namespace Scheduler
{
    public class WeeklyFrequency
    {
        private const string OCCURRENCE_STR = "Occurs every {0} weeks on {1} ";
        private string description;
        private DayOfWeek[] daysOfWeek;

        public WeeklyFrequency()
        {

        }

        public string Description
        {
            get
            {
                if (description == null)
                {
                    string daysOfWeekStr = "";
                    for (int i = 0; i < DaysOfWeek.Length; i++)
                    {
                        if (i == DaysOfWeek.Length - 1 && DaysOfWeek.Length > 1)
                        {
                            daysOfWeekStr = daysOfWeekStr.Remove(daysOfWeekStr.Length-2, 1);
                            daysOfWeekStr += "and " + Enum.GetName(typeof(DayOfWeek), DaysOfWeek[i]);
                        }
                        else
                        {
                            daysOfWeekStr += Enum.GetName(typeof(DayOfWeek), DaysOfWeek[i]) + ", ";
                        }
                    }
                    if(DaysOfWeek.Length == 1)
                    {
                        daysOfWeekStr = daysOfWeekStr.Remove(daysOfWeekStr.Length - 2, 2);
                    }
                    description = string.Format(OCCURRENCE_STR, Occurrence, daysOfWeekStr.ToLower());
                    if(Occurrence == 1)
                    {
                        description = description.Replace("weeks", "week");
                    }
                }
                return description;
            }
        }
        public int Occurrence { get; set; }
        public DayOfWeek[] DaysOfWeek 
        {
            get
            {
                return daysOfWeek;
            }
            set
            {
                daysOfWeek = value.OrderBy(D => ((int)D + 6) % 7).ToArray();
            }
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

    }
}
