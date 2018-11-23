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
    public class SettlementRepo
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string ConnectionString { get; set; }

        public SettlementRepo(IConfiguration Configuration)
        {
            this.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public List<Settlements> Read()
        {
            List<Settlements> list = new List<Settlements>();
            string sql = @"select a.SettlementId, a.Settlement, a.WardId, b.Ward, a.InsertUserId,
                            c.Username as InsertUser, a.InsertDate, a.UpdateUserId, d.Username as UpdateUser,
                            a.UpdateDate, b.LGAId, e.LGA, e.StateId, f.State 
                            from Settlements a inner join Wards b on b.WardId = a.WardId
                            inner join LGAs e on e.LGAId = b.LGAId inner join States f on f.StateId = e.StateId
                            left outer join Users c on c.UserId = a.InsertUserId left outer join Users d on d.UserId = a.UpdateUserId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    list = conn.Query<Settlements>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public Response Create(SettlementContext context)
        {
            Response response = new Response();
            string sql = "INSERT INTO SETTLEMENTS (SETTLEMENT, WARDID, INSERTUSERID, INSERTDATE) VALUES (@Settlement, @WardId, @InsertUserId, GetDate())";
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

        public Response Update(SettlementContext context)
        {
            Response response = new Response();
            string sql = "UPDATE SETTLEMENTS SET SETTLEMENT = @Settlement, WARDID = @WardId, UPDATEUSERID = @UpdateUserId, UPDATEDATE = GetDate() WHERE SETTLEMENTID = @SettlementId";
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

        public Response Delete(int SettlementId)
        {
            Response response = new Response();
            string sql = "DELETE FROM SETTLEMENTS WHERE SETTLEMENTID = @SettlementId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, new { SettlementId });
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
