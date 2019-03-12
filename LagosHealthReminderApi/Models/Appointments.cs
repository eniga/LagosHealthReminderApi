using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.Models
{
    public class Appointments
    {
        public int AppointmentId { get; set; }
        public int PatientAppointmentId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string Phone { get; set; }
        public string AltPhone { get; set; }
        public DateTime Dob { get; set; }
        public string HouseNumber { get; set; }
        public int SettlementId { get; set; }
        public string Settlement { get; set; }
        public int ServiceKindId { get; set; }
        public string ServiceKindName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int StatusId { get; set; }
        public string StatusDescription { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public int InsertUserId { get; set; }
        public string InsertUser { get; set; }
        public DateTime InsertDate { get; set; }
        public int UpdateUserId { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public int Defaulter { get; set; }
        public DateTime ContactedOn { get; set; }
        public int ContactedBy { get; set; }
        public string ContactByUsername { get; set; }
        public int ReminderSent { get; set; }
    }

    public class PatientAppointment
    {
        public int PatientAppointmentId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string Phone { get; set; }
        public string AltPhone { get; set; }
        public DateTime Dob { get; set; }
        public string HouseNumber { get; set; }
        public int SettlementId { get; set; }
        public string Settlement { get; set; }
        public int ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; }
        public string OptionType { get; set; }
        public int InsertUserId { get; set; }
        public string InsertUser { get; set; }
        public DateTime InsertDate { get; set; }
        public int UpdateUserId { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
    }

    public class PatientAppointmentResponse
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string Phone { get; set; }
        public string AltPhone { get; set; }
        public DateTime Dob { get; set; }
        public string HouseNumber { get; set; }
        public int SettlementId { get; set; }
        public string Settlement { get; set; }
        public int ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; }
        public string OptionType { get; set; }
        public int ServiceKindId { get; set; }
        public string ServiceKindName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int StatusId { get; set; }
        public string StatusDescription { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public int AppointmentId { get; set; }
        public int Defaulter { get; set; }
        public DateTime ContactedOn { get; set; }
        public int ContactedBy { get; set; }
    }

    public class AppointmentResponse
    {
        public int ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; }
        public string OptionType { get; set; }
        public List<Appointments> appointments { get; set; }
    }

    public class AppointmentRequest
    {
        public int PatientAppointmentId { get; set; }
        public int PatientId { get; set; }
        public int ServiceTypeId { get; set; }
        public int ServiceKindId { get; set; }
        public string OptionType { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int InsertUserId { get; set; }
        public DateTime InsertDate { get; set; }
        public int UpdateUserId { get; set; }
        public DateTime UpdateDate { get; set; }
    }

    public class NewAppointmentRequest
    {
        public int PatientAppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int InsertUserId { get; set; }
        public DateTime InsertDate { get; set; }
        public int UpdateUserId { get; set; }
        public DateTime UpdateDate { get; set; }
    }

    public class ConfirmAppointmentRequest
    {
        public int AppointmentId { get; set; }
        public int InsertUserId { get; set; }
    }

    public class LastAppointment
    {
        public int ServiceTypeId { get; set; }
        public int TotalAppointments { get; set; }
        public int TotalServiceKinds { get; set; }
    }
}
