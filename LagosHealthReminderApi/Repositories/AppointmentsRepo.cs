using Dapper;
using LagosHealthReminderApi.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.Repositories
{
    public class AppointmentsRepo
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string ConnectionString { get; set; }

        public AppointmentsRepo(IConfiguration Configuration)
        {
            this.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public List<Appointments> GetAll()
        {
            List<Appointments> list = new List<Appointments>();
            string sql = "SELECT * FROM APPOINTMENTS";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    list = conn.Query<Appointments>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public Appointments GetAppointment(int AppointmentId)
        {
            Appointments appointments = new Appointments();
            string sql = "SELECT * FROM APPOINTMENTS WHERE APPOINTMENTID = ?AppointmentId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    appointments = conn.Query<Appointments>(sql, new { AppointmentId }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return appointments;
        }

        public List<Appointments> GetPending()
        {
            List<Appointments> list = new List<Appointments>();
            string sql = "SELECT * FROM APPOINTMENTS WHERE STATUS = 1";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    list = conn.Query<Appointments>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public List<Appointments> GetConfirmed()
        {
            List<Appointments> list = new List<Appointments>();
            string sql = "SELECT * FROM APPOINTMENTS WHERE STATUS = 3";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    list = conn.Query<Appointments>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public List<Appointments> GetDefaulters()
        {
            List<Appointments> list = new List<Appointments>();
            string sql = "SELECT * FROM APPOINTMENTS WHERE STATUS = 0";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    list = conn.Query<Appointments>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public Response CreateAppointment(Appointments appointments)
        {
            Response response = new Response();
            string sql = "";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, appointments);
                    response.Status = true;
                    response.StatusMessage = "Approved and completed successfully";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.StatusMessage = "System Malfunction";
                logger.Error(ex);
            }
            return response;
        }

        public Response UpdateAppointment(Appointments appointments)
        {
            Response response = new Response();
            string sql = "";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, appointments);
                    response.Status = true;
                    response.StatusMessage = "Approved and completed successfully";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.StatusMessage = "System Malfunction";
                logger.Error(ex);
            }
            return response;
        }

        public Response DeleteAppointment(int AppointmentId)
        {
            Response response = new Response();
            string sql = "DELETE FROM APPOINTMENTS WHERE APPOINTMENTID = ?AppointmentId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, new { AppointmentId });
                    response.Status = true;
                    response.StatusMessage = "Approved and completed successfully";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.StatusMessage = "System Malfunction";
                logger.Error(ex);
            }
            return response;
        }
    }
}
