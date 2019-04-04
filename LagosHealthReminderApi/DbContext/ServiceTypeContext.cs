﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.DbContext
{
    [Table("ServiceTypes")]
    public class ServiceTypeContext
    {
        [Key]
        public int ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; }
        public int InsertUserId { get; set; }
        public DateTime InsertDate { get; set; }
        public int UpdateUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string SmsMessage { get; set; }
    }
}
