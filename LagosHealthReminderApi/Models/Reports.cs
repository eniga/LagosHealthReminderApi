﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.Models
{
    public class Reports
    {
    }

    public class AppointmentReport
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get => string.Format("{0} {1}", FirstName, LastName); }
        public string QrCode { get; set; }
        public string Phone { get; set; }
        public string ServiceKindName { get; set; }
        public string ServiceTypeName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public DateTime DateCreated { get; set; }
        public string Settlement { get; set; }
        public string Ward { get; set; }
        public string LGA { get; set; }
        public string PHC { get; set; }
        public string ReminderStatus { get; set; }
        public DateTime DateSent { get; set; }
        public string Intervention { get; set; }
    }
}
