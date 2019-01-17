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
    }

    public class GetBalanceResult
    {
        public string balance { get; set; }
    }
}
