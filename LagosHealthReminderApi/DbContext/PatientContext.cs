using LagosHealthReminderApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.DbContext
{
    public class PatientContext
    {
        public int PatientId { get; set; }
        public string QrCode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string AltPhone { get; set; }
        public string Email { get; set; }
        public DateTime Dob { get; set; }
        public int SettlementId { get; set; }
        public string HouseNumber { get; set; }
        public int InsertUserId { get; set; }
        public DateTime InsertDate { get; set; }
        public int UpdateUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public int PHCId { get; set; }
    }

    public class CreatePatientResponse : Response
    {
        public int PatientId { get; set; }
    }
}
