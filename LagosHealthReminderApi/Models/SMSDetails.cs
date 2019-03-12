using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.Models
{
    public class SMSDetails
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int Sent { get; set; }
        public decimal Balance { get; set; }
        public DateTime LastSent { get; set; }
        public DateTime LastUpdated { get; set; }
        public decimal LastAmount { get; set; }
        public string Url { get; set; }
        public string AppName { get; set; }
    }

    public class GetBalanceResult
    {
        public string balance { get; set; }
    }

    public class SMSRequest
    {
        public string Phone { get; set; }
        public string Message { get; set; }
    }

    public class SMSResponse
    {
        public string status { get; set; }
        public int count { get; set; }
        public int price { get; set; }
    }

    public class SMSNotificationRequest
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string ServiceKindName { get; set; }
        public string ServiceTypeName { get; set; }
        public string SMSMessage { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public int PatientId { get; set; }
    }
}
