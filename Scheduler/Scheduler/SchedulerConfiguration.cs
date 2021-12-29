﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
    public class SchedulerConfiguration
    {
        public SchedulerConfiguration() { }
        #region Properties
        public SchedulerType Type { get; set; }
        public DateTime? DateTimeOnce { get; set; }
        public FrequencyType Frequency { get; set; }
        public DailyFrequency DailyFrequency { get; set; }
        public WeeklyFrequency WeeklyFrequency { get; set; }
        public MonthlyFrequency MonthlyFrequency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Culture { get; set; }
        #endregion
    }

}
