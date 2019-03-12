using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.DbContext
{
    public class AppointmentsContext
    {
        public int AppointmentId { get; set; }
        public int PatientAppointmentId { get; set; }
        public int ServiceKindId { get; set; }
        public string OptionType { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int StatusId { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public int InsertUserId { get; set; }
        public DateTime InsertDate { get; set; }
        public int UpdateUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public int Defaulter { get; set; }
        public DateTime ContactedOn { get; set; }
        public int ContactedBy { get; set; }
        public int ReminderSent { get; set; }
    }

    public class PatientAppointmentContext
    {
        public int PatientAppointmentId { get; set; }
        public int PatientId { get; set; }
        public int ServiceTypeId { get; set; }
        public int InsertUserId { get; set; }
        public DateTime InsertDate { get; set; }
        public int UpdateUserId { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
