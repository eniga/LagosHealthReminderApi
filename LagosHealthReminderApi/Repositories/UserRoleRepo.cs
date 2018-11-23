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
    public class UserRoleRepo
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string ConnectionString { get; set; }

        public UserRoleRepo(IConfiguration Configuration)
        {
            this.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public List<UserRoles> Read()
        {
            List<UserRoles> list = new List<UserRoles>();
            string sql = @"SELECT a.UserRoleId, a.UserId, b.Username, a.RoleId, c.RoleName FROM UserRoles a 
                            inner join Users b on b.UserId = a.UserId
                            inner join Roles c on c.RoleId = a.RoleId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    list = conn.Query<UserRoles>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public Response Create(UserRoleContext context)
        {
            Response response = new Response();
            string sql = "INSERT INTO USERROLES (USERID, ROLEID) VALUES (@UserId, @RoleId)";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute("update users set phcid = @PHCId, updateuserid = @UpdateUserId, updatedate = getdate() where userid = @UserId", context);
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

        public Response Update(UserRoleContext context)
        {
            Response response = new Response();
            string sql = "UPDATE USERROLES SET USERID = @UserId, ROLEID = @RoleId WHERE USERROLEID = @UserRoleId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute("update users set phcid = @PHCId, updateuserid = @UpdateUserId, updatedate = getdate() where userid = @UserId", context);
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

        public Response Delete(int UserRoleId)
        {
            Response response = new Response();
            string sql = "DELETE FROM USERROLES WHERE USERROLEID = @UserRoleId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, new { UserRoleId });
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
