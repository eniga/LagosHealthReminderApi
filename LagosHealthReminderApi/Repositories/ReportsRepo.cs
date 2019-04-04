using Dapper;
using LagosHealthReminderApi.DbContext;
using LagosHealthReminderApi.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace LagosHealthReminderApi.Repositories
{
    public class ReportsRepo
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string ConnectionString { get; set; }

        public ReportsRepo(IConfiguration Configuration)
        {
            this.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public List<AppointmentReport> GetAppointments()
        {
            List<AppointmentReport> response = new List<AppointmentReport>();
            string sql = @"select e.FirstName, e.LastName, e.QrCode, Phone, 
                            c.ServiceKindName, d.ServiceTypeName, 
                            CAST(a.AppointmentDate AS DATE) AppointmentDate, CAST(a.ConfirmationDate as DATE) ConfirmationDate,
                            CAST(a.InsertDate AS DATE) DateCreated, f.Settlement, g.Ward, h.LGA, k.PHC,
                            (CASE WHEN a.ReminderSent = 3 THEN 'Sent' WHEN a.ReminderSent = 4 THEN 'Skipped' ELSE 'Awaiting' end) ReminderStatus,
                            i.datecreated DateSent, (CASE WHEN a.ReminderSent = 3 THEN 'Intervention' WHEN a.ReminderSent = 4 THEN 'Control' ELSE 'Awaiting' end) Intervention from Appointments a
                            inner join PatientAppointment b on a.PatientAppointmentId = b.PatientAppointmentId
                            inner join ServiceKinds c on a.ServiceKindId = c.ServiceKindId
                            inner join ServiceTypes d on b.ServiceTypeId = d.ServiceTypeId
                            inner join Patients e on b.PatientId = e.PatientId
                            inner join Settlements f on e.SettlementId = f.SettlementId
                            inner join Wards g on f.WardId = g.WardId
                            inner join LGAs h on g.LGAId = h.LGAId
                            left outer join reminder_messages i on a.AppointmentId = i.AppointmentId
                            left outer join Users j on a.InsertUserId = j.UserId
							left outer join PHCs k on j.PHCId = k.PHCId
                            order by a.AppointmentId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    response = conn.Query<AppointmentReport>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return response;
        }

        public List<AppointmentReport> GetAppointmentsRange(DateTime StartDate, DateTime EndDate)
        {
            List<AppointmentReport> response = new List<AppointmentReport>();
            string sql = @"select e.FirstName, e.LastName, e.QrCode, Phone, 
                            c.ServiceKindName, d.ServiceTypeName, 
                            CAST(a.AppointmentDate AS DATE) AppointmentDate, CAST(a.ConfirmationDate as DATE) ConfirmationDate,
                            CAST(a.InsertDate AS DATE) DateCreated, f.Settlement, g.Ward, h.LGA, k.PHC,
                            (CASE WHEN a.ReminderSent = 3 THEN 'Sent' WHEN a.ReminderSent = 4 THEN 'Skipped' ELSE 'Awaiting' end) ReminderStatus,
                            i.datecreated DateSent, (CASE WHEN a.ReminderSent = 3 THEN 'Intervention' WHEN a.ReminderSent = 4 THEN 'Control' ELSE 'Awaiting' end) Intervention from Appointments a
                            inner join PatientAppointment b on a.PatientAppointmentId = b.PatientAppointmentId
                            inner join ServiceKinds c on a.ServiceKindId = c.ServiceKindId
                            inner join ServiceTypes d on b.ServiceTypeId = d.ServiceTypeId
                            inner join Patients e on b.PatientId = e.PatientId
                            inner join Settlements f on e.SettlementId = f.SettlementId
                            inner join Wards g on f.WardId = g.WardId
                            inner join LGAs h on g.LGAId = h.LGAId
                            left outer join reminder_messages i on a.AppointmentId = i.AppointmentId
                            left outer join Users j on a.InsertUserId = j.UserId
							left outer join PHCs k on j.PHCId = k.PHCId
                            where CAST(a.AppointmentDate AS DATE) BETWEEN CAST(@StartDate AS DATE) and CAST(@EndDate AS DATE)
                            order by a.AppointmentId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    response = conn.Query<AppointmentReport>(sql, new { StartDate, EndDate }).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return response;
        }
    }
}
