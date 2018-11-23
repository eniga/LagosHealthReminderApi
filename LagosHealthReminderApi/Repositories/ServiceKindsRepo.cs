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
    public class ServiceKindsRepo
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string ConnectionString { get; set; }

        public ServiceKindsRepo(IConfiguration Configuration)
        {
            this.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public List<ServiceKinds> Read(int ServiceTypeId)
        {
            List<ServiceKinds> list = new List<ServiceKinds>();
            string sql = @"SELECT a.ServiceKindId, a.ServiceKindName, a.ServiceTypeId, d.ServiceTypeName as ServiceType,
                            a.InsertUserId, b.Username as InsertUser, a.InsertDate, a.UpdateUserId, c.Username, a.UpdateDate
                            FROM SERVICEKINDS a inner join ServiceTypes d on d.ServiceTypeId = a.ServiceTypeId
                            left outer join Users b on b.UserId = a.InsertUserId left outer join Users c on c.UserId = a.UpdateUserId
                            WHERE a.ServiceTypeId = @ServiceTypeId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    list = conn.Query<ServiceKinds>(sql, new { ServiceTypeId }).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public Response Create(ServiceKindContext kind)
        {
            Response response = new Response();
            string sql = "INSERT INTO SERVICEKINDS (SERVICEKINDNAME, SERVICETYPEID, INSERTUSERID, INSERTDATE) VALUES (@ServiceKindName, @ServiceTypeId, @InsertUserId, GetDate())";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, kind);
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

        public Response Update(ServiceKindContext kind)
        {
            Response response = new Response();
            string sql = "UPDATE SERVICEKINDS SET SERVICEKINDNAME = @ServiceKindName, SERVICETYPEID = @ServiceTypeId, UPDATEUSERID = @UpdateUserId, UPDATEDATE = GETDATE() WHERE SERVICEKINDID = @ServiceKindId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, kind);
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

        public Response Delete(int ServiceKindId)
        {
            Response response = new Response();
            string sql = "DELETE FROM SERVICEKINDS WHERE SERVICEKINDID = @ServiceKindId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, new { ServiceKindId });
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
