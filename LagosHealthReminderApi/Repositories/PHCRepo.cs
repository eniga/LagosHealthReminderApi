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
    public class PHCRepo
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string ConnectionString { get; set; }

        public PHCRepo(IConfiguration Configuration)
        {
            this.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public List<PHCs> Read()
        {
            List<PHCs> list = new List<PHCs>();
            string sql = @"select a.PHCId, a.PHC, a.WardId, b.Ward, a.InsertUserId,
                            c.Username as InsertUser, a.InsertDate, a.UpdateUserId, d.Username as UpdateUser,
                            a.UpdateDate, b.LGAId, e.LGA, e.StateId, f.State 
                            from PHCs a inner join Wards b on b.WardId = a.WardId
                            inner join LGAs e on e.LGAId = b.LGAId inner join States f on f.StateId = e.StateId
                            left outer join Users c on c.UserId = a.InsertUserId left outer join Users d on d.UserId = a.UpdateUserId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    list = conn.Query<PHCs>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public Response Create(PHCContext context)
        {
            Response response = new Response();
            string sql = "INSERT INTO PHCs (PHC, WARDID, INSERTUSERID, INSERTDATE) VALUES (@PHC, @WardId, @InsertUserId, GetDate())";
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

        public Response Update(PHCContext context)
        {
            Response response = new Response();
            string sql = "UPDATE PHCs SET PHC = @PHC, WARDID = @WardId, UPDATEUSERID = @UpdateUserId, UPDATEDATE = GetDate() WHERE PHCID = @PHCId";
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

        public Response Delete(int PHCId)
        {
            Response response = new Response();
            string sql = "DELETE FROM PHCs WHERE PHCID = @PHCId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, new { PHCId });
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
