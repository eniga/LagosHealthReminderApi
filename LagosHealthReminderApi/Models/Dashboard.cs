using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.Models
{
    public class Dashboard
    {
        public int LastMonth { get; set; }
        public int CurrentMonth { get; set; }
    }

    public class MonthlyBreakdown
    {
        public int MonthId { get; set; }
        public string MonthName { get; set; }
        public int TotalCount { get; set; }
    }
}
