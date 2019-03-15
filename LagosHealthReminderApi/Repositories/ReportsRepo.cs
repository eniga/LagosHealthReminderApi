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
            string sql = @"select concat(e.firstname, ' ', e.LastName) PatientName, e.QrCode, Phone, 
                            c.ServiceKindName, d.ServiceTypeName, 
                            CAST(a.AppointmentDate AS DATE) AppointmentDate, a.ConfirmationDate,
                            CAST(a.InsertDate AS DATE) DateCreated, f.Settlement, g.Ward, h.LGA, 
                            (CASE WHEN a.ReminderSent = 3 THEN 'Sent' WHEN a.ReminderSent = 4 THEN 'Skipped' ELSE 'Not Sent' end) ReminderStatus,
                            i.datecreated DateSent from Appointments a
                            inner join PatientAppointment b on a.PatientAppointmentId = b.PatientAppointmentId
                            inner join ServiceKinds c on a.ServiceKindId = c.ServiceKindId
                            inner join ServiceTypes d on b.ServiceTypeId = d.ServiceTypeId
                            inner join Patients e on b.PatientId = e.PatientId
                            inner join Settlements f on e.SettlementId = f.SettlementId
                            inner join Wards g on f.WardId = g.WardId
                            inner join LGAs h on g.LGAId = h.LGAId
                            left outer join reminder_messages i on a.AppointmentId = i.AppointmentId
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
    }
}
