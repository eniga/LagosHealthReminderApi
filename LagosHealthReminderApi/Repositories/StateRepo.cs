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
    public class StateRepo
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string ConnectionString { get; set; }

        public StateRepo(IConfiguration Configuration)
        {
            this.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public List<StateContext> ReadAll()
        {
            List<StateContext> list = new List<StateContext>();
            string sql = "SELECT a.*, b.Username InsertUser FROM STATES a inner join users b on a.InsertUserId = b.UserId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    list = conn.Query<StateContext>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public StateContext Read(int StateId)
        {
            StateContext result = new StateContext();
            string sql = "SELECT a.*, b.Username InsertUser FROM STATES a inner join users b on a.InsertUserId = b.UserId where a.StateId = @StateId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    result = conn.Query<StateContext>(sql, new { StateId }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return result;
        }

        public Response Create(StateContext context)
        {
            Response response = new Response();
            string sql = "INSERT INTO STATES (STATE, INSERTUSERID, INSERTDATE) VALUES (@State, @InsertUserId, GetDate())";
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

        public Response Update(StateContext context)
        {
            Response response = new Response();
            string sql = "UPDATE STATES SET STATE = @State, UPDATEUSERID = @UpdateUserId, UPDATEDATE = GetDate() WHERE STATEID = @StateId";
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

        public Response Delete(int StateId)
        {
            Response response = new Response();
            string sql = "DELETE FROM STATES WHERE STATEID = @StateId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Delete<StateContext>(StateId);
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
