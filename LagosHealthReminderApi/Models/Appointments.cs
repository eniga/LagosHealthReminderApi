using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.Models
{
    public class Appointments
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int ServiceTypeId { get; set; }
        public string ServiceType { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int InsertUserId { get; set; }
        public string InsertUser { get; set; }
        public DateTime InsertDate { get; set; }
        public int UpdateUserId { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
