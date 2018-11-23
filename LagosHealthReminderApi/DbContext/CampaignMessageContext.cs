using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.DbContext
{
    public class CampaignMessageContext
    {
        public int CampaignId { get; set; }
        public int LGAId { get; set; }
        public int InsertUserId { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime DateSent { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
    }
}
