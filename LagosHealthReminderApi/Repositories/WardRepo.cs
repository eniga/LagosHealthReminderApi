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
    public class WardRepo
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string ConnectionString { get; set; }

        public WardRepo(IConfiguration Configuration)
        {
            this.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public List<Wards> ReadAll()
        {
            List<Wards> list = new List<Wards>();
            string sql = @"Select a.WardId, a.Ward, a.LGAId,  b.LGA, a.InsertUserId,
                            b.StateId, e.State, c.username InsertUser, a.InsertDate,
                            a.UpdateUserId, d.username UpdateUser, a.UpdateDate
                            from Wards a inner join LGAs b on b.LGAId = a.LGAId
                            inner join States e on e.StateId = b.StateId
                            left outer join Users c on c.UserId = a.InsertUserId
                            left outer join Users d on d.UserId = a.UpdateUserId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    list = conn.Query<Wards>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public List<Wards> Read(int SettlementId)
        {
            List<Wards> list = new List<Wards>();
            string sql = @"Select a.WardId, a.Ward, a.LGAId,  b.LGA, a.InsertUserId
                            from Wards a inner join LGAs b on b.LGAId = a.LGAId 
                            left outer join Users c on c.UserId = a.InsertUserId
                            left outer join Users d on d.UserId = a.UpdateUserId
                            where a.SettlementId = @SettlementId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    list = conn.Query<Wards>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public Response Create(WardContext context)
        {
            Response response = new Response();
            string sql = "INSERT INTO WARDS (WARD, LGAID, INSERTUSERID, INSERTDATE) VALUES (@Ward, @LGAId, @InsertUserId, GetDate())";
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

        public Response Update(WardContext context)
        {
            Response response = new Response();
            string sql = "UPDATE WARDS SET WARD = @Ward, LGAID = @LGAId, UPDATEUSERID = @UpdateUserId, UPDATEDATE = GetDate() WHERE WARDID = @WardId";
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

        public Response Delete(int WardId)
        {
            Response response = new Response();
            string sql = "DELETE FROM WARDS WHERE WARDID = @WardId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, new { WardId });
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
