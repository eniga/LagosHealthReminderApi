using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.Models
{
    public class CampaignMessage
    {
        public int CampaignId { get; set; }
        public int LGAId { get; set; }
        public string LGA { get; set; }
        public int InsertUserId { get; set; }
        public string InsertUser { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime DateSent { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
    }
}
