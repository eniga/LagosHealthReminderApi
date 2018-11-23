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

namespace LagosHealthReminderApi.Repositories
{
    public class NextOfKinRepo
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string ConnectionString { get; set; }

        public NextOfKinRepo(IConfiguration Configuration)
        {
            this.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public List<NextOfKins> Read(int PatientId)
        {
            List<NextOfKins> list = new List<NextOfKins>();
            string sql = @"Select a.NextOfKinId, a.PatientId, CONCAT(b.FirstName, ' ', b.MiddleName, ' ', b.LastName) as PatientName,
                            a.FirstName, a.MiddleName, a.LastName, a.Phone, a.Email, a.InsertUserId, c.Username as InsertUser,
                            a.InsertDate, a.UpdateDate, a.UpdateUserId, d.Username as UpdateUser
                            from NextOfKins a left outer join Patients b on b.PatientId = a.PatientId
                            left outer join Users c on c.UserId = a.InsertUserId
                            left outer join Users d on d.UserId = a.UpdateUserId
                            where a.PatientId = @PatientId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    list = conn.Query<NextOfKins>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public Response Create(NextOfKinContext context)
        {
            Response response = new Response();
            string sql = "INSERT INTO NEXTOFKINS (PATIENTID, FIRSTNAME, MIDDLENAME, LASTNAME, PHONE, EMAIL, INSERTUSERID, USERDATE) VALUES (@PatientId, @FirstName, @MiddleName, @LastName, @Phone, @Email, @InsertUserId, GetDate())";
            try
            {
                using(IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, context);
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

        public Response Update(NextOfKinContext context)
        {
            Response response = new Response();
            string sql = "UPDATE NEXTOFKINS SET PATIENTID = @PatientId, FIRSTNAME = @FirstName, MIDDLENAME = @MiddleName, LASTNAME = @LastName, PHONE = @Phone, EMAIL = @Email, UPDATEUSERID = @UpdateUserId, UPDATEDATE = GETDATE() WHERE NEXTOFKINID = @NextOfKinId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, context);
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

        public Response Delete(int NextOfKinId)
        {
            Response response = new Response();
            string sql = "DELETE FROM NEXTOFKINS WHERE NEXTOFKINID = @NextOfKinId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, new { NextOfKinId });
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
