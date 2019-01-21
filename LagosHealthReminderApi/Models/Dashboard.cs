using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.Models
{
    public class Dashboard
    {
        public int Patients { get; set; }
        public int Settlements { get; set; }
        public int Appointments { get; set; }
        public int Defaulters { get; set; }
        public int TodayAppointments { get; set; }
    }
}
