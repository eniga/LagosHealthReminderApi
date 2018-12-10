using LagosHealthReminderApi.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.Models
{
    public class ServiceKinds
    {
        public int ServiceKindId { get; set; }
        public string ServiceKindName { get; set; }
        public int ServiceTypeId { get; set; }
        public string ServiceType { get; set; }
        public int InsertUserId { get; set; }
        public string InsertUser { get; set; }
        public DateTime InsertDate { get; set; }
        public int UpdateUserId { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public int TypeId { get; set; }
        public string Type { get; set; }
        public int Duration { get; set; }
    }
}
