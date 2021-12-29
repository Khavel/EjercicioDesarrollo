using System;
using System.Collections.Generic;
using System.Linq;

namespace Scheduler
{
    public class MonthlyFrequency
    {
        #region Properties
        public bool IsDaily { get; set; }
        public int DayNumber { get; set; }
        public int Interval { get; set; }
        public MonthlyFrequencyType? Frequency { get; set; }
        public MonthlyDayType? DayType { get; set; }
        #endregion
    }
}
