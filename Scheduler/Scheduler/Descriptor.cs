using System;

namespace Scheduler
{
    public static class Descriptor
    {
        public static string GetMonthlyDescription(MonthlyFrequency freq, TextManager textManager)
        {
            if (freq.IsDaily)
            {
                return string.Format(textManager.GetText("DAILY_OCCURRENCE"), freq.DayNumber, freq.Interval, (freq.Interval > 1 ? textManager.GetText("MONTHS") : textManager.GetText("MONTH")));
            }
            else
            {
                return string.Format(textManager.GetText("RECURRING_OCCURENCE"), freq.Frequency, freq.DayType, freq.Interval, (freq.Interval > 1 ? textManager.GetText("MONTHS") : textManager.GetText("MONTH")));
            }
        }

        public static string GetWeeklyDescription(WeeklyFrequency freq, TextManager textManager)
        {
            string daysOfWeekStr = "";
            string description;
            for (int i = 0; i < freq.DaysOfWeek.Length; i++)
            {
                if (i == freq.DaysOfWeek.Length - 1 && freq.DaysOfWeek.Length > 1)
                {
                    daysOfWeekStr = daysOfWeekStr.Remove(daysOfWeekStr.Length - 2, 1);
                    daysOfWeekStr += "and " + Enum.GetName(typeof(DayOfWeek), freq.DaysOfWeek[i]);
                }
                else
                {
                    daysOfWeekStr += Enum.GetName(typeof(DayOfWeek), freq.DaysOfWeek[i]) + ", ";
                }
            }
            if (freq.DaysOfWeek.Length == 1)
            {
                daysOfWeekStr = daysOfWeekStr.Remove(daysOfWeekStr.Length - 2, 2);
            }
            description = string.Format(textManager.GetText("OCCURRENCE_STR"), freq.Occurrence, daysOfWeekStr.ToLower());
            if (freq.Occurrence == 1)
            {
                description = description.Replace(textManager.GetText("WEEKS"), textManager.GetText("WEEK"));
            }
            return description;
        }

        public static string GetDailyDescription(DailyFrequency freq, TextManager textManager)
        {

            string description;
            if (freq.IsRecurring == false)
            {
                description = string.Format(textManager.GetText("OCCURRENCE_STR_ONCE"), freq.Occurrence.ToString(@"hh\:mm\:ss"));
            }
            else
            {
                string timeStr = textManager.GetText("SECONDS");
                int timePart = freq.Occurrence.Seconds;
                if (freq.Occurrence.Hours != 0)
                {
                    timeStr = textManager.GetText("HOURS");
                    timePart = freq.Occurrence.Hours;
                }
                else if (freq.Occurrence.Minutes != 0)
                {
                    timeStr = textManager.GetText("MINUTES");
                    timePart = freq.Occurrence.Minutes;
                }
                if (timePart == 1)
                {
                    timeStr = timeStr.Substring(0, timeStr.Length - 1);
                }
                description = string.Format(textManager.GetText("OCCURRENCE_STR_RECURRING"), timePart, timeStr,
                    freq.StartTime.Value.ToString(@"hh\:mm\:ss"), freq.EndTime.Value.ToString(@"hh\:mm\:ss"));
            }
            return description;
        }


    }
}
