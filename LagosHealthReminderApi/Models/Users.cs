using LagosHealthReminderApi.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.Models
{
    public class Users
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public int InsertUserId { get; set; }
        public string InsertUser { get; set; }
        public DateTime InsertDate { get; set; }
        public int UpdateUserId { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UserRoleId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int IsActive { get; set; }
        public int PHCId { get; set; }
        public string PHC { get; set; }
        public int WardId { get; set; }
        public string Ward { get; set; }
        public int LGAId { get; set; }
        public string LGA { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
    }
}
