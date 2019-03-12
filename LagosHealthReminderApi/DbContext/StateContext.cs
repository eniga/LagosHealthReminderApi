using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.DbContext
{
    [Table("States")]
    public class StateContext
    {
        [Key]
        public int StateId { get; set; }
        public string State { get; set; }
        public int InsertUserId { get; set; }
        [Editable(false)]
        public string InsertUser { get; set; }
        [ReadOnly(true)]
        public DateTime InsertDate { get; set; }
        [IgnoreSelect]
        public int UpdateUserId { get; set; }
        [IgnoreSelect, Editable(false)]
        public string UpdateUser { get; set; }
        [IgnoreSelect, ReadOnly(true)]
        public DateTime UpdateDate { get; set; }
    }
}
