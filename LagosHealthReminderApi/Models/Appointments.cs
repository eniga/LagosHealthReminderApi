using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.Models
{
    public class Appointments
    {
        public int AppointmentId { get; set; }
        public int ServiceKindId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int PatientId { get; set; }
        public int Confirmed { get; set; }
        public DateTime? DateConfirmed { get; set; }
    }
}
