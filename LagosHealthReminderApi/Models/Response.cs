using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.Models
{
    public class Response
    {
        public bool Status { get; set; }
        public string StatusMessage { get; set; }
    }

    public class ConfirmAppointmentResponse : Response
    {
        public bool LastAppointment { get; set; }
    }
}
