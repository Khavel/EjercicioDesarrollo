using System;
using System.Collections.Generic;

namespace Scheduler
{
    public class DailyFrequency
    {
        public TimeSpan Occurrence { get; set; }
        public bool IsRecurring { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}
