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
    public class LGARepo
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string ConnectionString { get; set; }

        public LGARepo(IConfiguration Configuration)
        {
            this.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public Response Create(LGAContext context)
        {
            Response response = new Response();
            string sql = "INSERT INTO LGAS (LGA, INSERTUSERID, INSERTDATE) VALUES (@LGA, @InsertUserId, GetDate())";
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
                response.StatusMessage = ex.Message;
                logger.Error(ex);
            }
            return response;
        }

        public List<LGAS> Read()
        {
            List<LGAS> list = new List<LGAS>();
            string sql = @"Select a.LGAId, a.LGA, a.InsertUserId, a.InsertDate, a.UpdateDate, a.UpdateUserId,
                            b.Username as InsertUser, c.Username as UpdateUser, a.StateId, d.State
                            from LGAs a left outer join Users b on b.UserId = a.InsertUserId
                            left outer join Users c on c.UserId = a.UpdateUserId
                            left outer join States d on d.StateId = a.StateId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    list = conn.Query<LGAS>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public Response Update(LGAContext context)
        {
            Response response = new Response();
            string sql = "UPDATE LGAS SET LGA = @LGA, UPDATEUSERID = @UpdateUserId, UPDATEDATE = GETDATE() WHERE LGAID = @LGAId";
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

        public Response Delete(int LGAId)
        {
            Response response = new Response();
            string sql = "DELETE FROM LGAS WHERE LGAID = @LGAId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, new { LGAId });
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
