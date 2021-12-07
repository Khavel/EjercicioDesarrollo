using System;
using System.Collections.Generic;

namespace Scheduler
{
    public class DailyFrequency
    {
        private const string OCCURRENCE_STR_RECURRING = "every {0} {1} between {2} and {3}";
        private const string OCCURRENCE_STR_ONCE = "once at {0}";

        public DailyFrequency()
        {

        }

        public TimeSpan Occurrence { get; set; }
        public bool IsRecurring { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        public TimeSpan[] GetDailyExecutionTimes()
        {
            List<TimeSpan> executionTimesAux = new List<TimeSpan>();
            for (var ts = StartTime; ts <= EndTime; ts += Occurrence)
            {
                executionTimesAux.Add(ts.Value);
            }
            return executionTimesAux.ToArray();
        }

        public string GetDescription()
        {
            string description;
            if (IsRecurring == false)
            {
                description = string.Format(OCCURRENCE_STR_ONCE, Occurrence.ToString(@"hh\:mm\:ss"));
            }
            else
            {
                string timeStr = "seconds";
                int timePart = Occurrence.Seconds;
                if (Occurrence.Hours != 0)
                {
                    timeStr = "hours";
                    timePart = Occurrence.Hours;
                }
                else if (Occurrence.Minutes != 0)
                {
                    timeStr = "minutes";
                    timePart = Occurrence.Minutes;
                }
                if (timePart == 1)
                {
                    timeStr = timeStr.Substring(0, timeStr.Length - 1);
                }
                description = string.Format(OCCURRENCE_STR_RECURRING, timePart, timeStr,
                    StartTime.Value.ToString(@"hh\:mm\:ss"), EndTime.Value.ToString(@"hh\:mm\:ss"));
            }
            return description;
        }

    }
}
