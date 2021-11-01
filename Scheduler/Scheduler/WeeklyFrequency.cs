using System;

namespace Scheduler
{
    public class WeeklyFrequency
    {
        private const string OCCURRENCE_STR = "Occurs every {0} weeks on {1} ";
        private string description;

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
                }
                return description;
            }
        }
        public int Occurrence { get; set; }
        public DayOfWeek[] DaysOfWeek { get; set; }
    }
}
