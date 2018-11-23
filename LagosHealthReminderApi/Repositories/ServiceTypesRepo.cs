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
    public class ServiceTypesRepo
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string ConnectionString { get; set; }

        public ServiceTypesRepo(IConfiguration Configuration)
        {
            this.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public List<ServiceTypes> Read()
        {
            List<ServiceTypes> list = new List<ServiceTypes>();
            string sql = @"Select a.ServiceTypeId, a.ServiceTypeName, a.InsertUserId, b.Username InsertUser, a.InsertDate, a.UpdateUserId, c.Username UpdateUser, a.UpdateDate, a.SMSMessage
                            from ServiceTypes a left outer join Users b on b.UserId = a.InsertUserId
                            left outer join Users c on c.UserId = a.UpdateUserId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    list = conn.Query<ServiceTypes>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public Response Create(ServiceTypeContext context)
        {
            Response response = new Response();
            string sql = "INSERT INTO SERVICETYPES (SERVICETYPENAME, INSERTUSERID, INSERTDATE) VALUES (@ServiceTypeName, @InsertUserId, GetDate())";
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

        public Response Update(ServiceTypeContext context)
        {
            Response response = new Response();
            string sql = "UPDATE SERVICETYPES SET SERVICETYPENAME = @ServiceTypeName, UPDATEUSERID = @UpdateUserId, UPDATEDATE = GetDate() WHERE SERVICETYPEID = @ServiceTypeId";
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

        public Response Delete(int ServiceTypeId)
        {
            Response response = new Response();
            string sql = "DELETE FROM SERVICETYPES WHERE SERVICETYPEID = @ServiceTypeId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, new { ServiceTypeId });
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

        public Response UpdateMessage(ServiceTypeContext context)
        {
            Response response = new Response();
            string sql = "UPDATE SERVICETYPES SET SMSMESSAGE = @SMSMessage, UPDATEUSERID = @UpdateUserId, UPDATEDATE = GetDate() WHERE SERVICETYPEID = @ServiceTypeId";
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
    }
}
