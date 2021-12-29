using System;
using System.Globalization;
using System.Linq;

namespace Scheduler
{
    public class WeeklyFrequency
    {
        private DayOfWeek[] daysOfWeek;

        public int Occurrence { get; set; }
        public DayOfWeek[] DaysOfWeek 
        {
            get
            {
                return Calculator.GetDaysOfWeekOrdered(daysOfWeek);
            }
            set
            {
                daysOfWeek = value;
            }
        }
    }
}
