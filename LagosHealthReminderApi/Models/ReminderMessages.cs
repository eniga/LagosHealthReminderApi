using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.Models
{
    [Table("reminder_messages")]
    public class ReminderMessages
    {
        [Key]
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int AppointmentId { get; set; }
        public string Message { get; set; }
        [ReadOnly(true)]
        public DateTime DateCreated { get; set; }
        public bool Sent { get; set; }
        public string PhoneNumber { get; set; }
    }
}
