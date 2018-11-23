using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.Models
{
    public class Settlements
    {
        public int SettlementId { get; set; }
        public string Settlement { get; set; }
        public int WardId { get; set; }
        public string Ward { get; set; }
        public int LGAId { get; set; }
        public string LGA { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
        public int InsertUserId { get; set; }
        public string InsertUser { get; set; }
        public DateTime InsertDate { get; set; }
        public int UpdateUserId { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
