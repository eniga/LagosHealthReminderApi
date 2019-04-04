using Dapper;
using LagosHealthReminderApi.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.Models
{
    [Table("ServiceTypes")]
    public class ServiceTypes
    {
        [Key]
        public int ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; }
        public int InsertUserId { get; set; }
        [Editable(false)]
        public string InsertUser { get; set; }
        public DateTime InsertDate { get; set; }
        public int UpdateUserId { get; set; }
        [Editable(false)]
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public string SmsMessage { get; set; }
    }
}
