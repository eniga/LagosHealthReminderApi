using LagosHealthReminderApi.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.Models
{
    public class Patients
    {
        public int PatientId { get; set; }
        public string QrCode { get; set; }
        public string QrCodeImage { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string AltPhone { get; set; }
        public string Email { get; set; }
        public DateTime Dob { get; set; }
        public string HouseNumber { get; set; }
        public int SettlementId { get; set; }
        public string Settlement { get; set; }
        public int WardId { get; set; }
        public string Ward { get; set; }
        public int PHCId { get; set; }
        public string PHC { get; set; }
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
