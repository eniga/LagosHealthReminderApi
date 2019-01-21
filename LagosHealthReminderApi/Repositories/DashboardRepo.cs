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
    public class DashboardRepo
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string ConnectionString { get; set; }

        public DashboardRepo(IConfiguration Configuration)
        {
            this.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public Dashboard Get()
        {
            Dashboard response = new Dashboard();
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    response.Patients = conn.Query<int>("select count(1) from patients").FirstOrDefault();
                    response.Settlements = conn.Query<int>("select count(1) from Settlements").FirstOrDefault();
                    response.Appointments = conn.Query<int>("select count(1) from Appointments").FirstOrDefault();
                    response.Defaulters = conn.Query<int>("select count(1) from Appointments where AppointmentDate < GETDATE() and ConfirmationDate is null").FirstOrDefault();
                    response.TodayAppointments = conn.Query<int>("select count(1) from Appointments where CONVERT(DATE, AppointmentDate, 102) = CONVERT(date, GETDATE(), 102)").FirstOrDefault();
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
