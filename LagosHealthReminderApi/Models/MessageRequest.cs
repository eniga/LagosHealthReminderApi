using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.Models
{
    public class MessageRequest
    {
        public string Destination { get; set; }
        public string Message { get; set; }
    }

    public class MessageResponse : Response
    {
        public string Sid { get; set; }
    }

    public class MessageServiceResponse
    {
        public string status { get; set; }
        public int count { get; set; }
        public decimal price { get; set; }
    }

    public class BalanceResult
    {
        public decimal balance { get; set; }
    }
}
