using Dapper;
using LagosHealthReminderApi.DbContext;
using LagosHealthReminderApi.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.Repositories
{
    public class SettingsRepo
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string ConnectionString { get; set; }

        public SettingsRepo(IConfiguration Configuration)
        {
            this.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public List<Settings> List()
        {
            List<Settings> list = new List<Settings>();
            string sql = "select * from settings";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    list = conn.Query<Settings>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public Settings Get(int SettingsId)
        {
            Settings result = new Settings();
            string sql = "select * from settings where settingsid = @SettingsId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    result = conn.Query<Settings>(sql, new { SettingsId }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return result;
        }

        public Response Add(Settings settings)
        {
            Response response = new Response();
            string sql = "insert into settings (name, value) values(@Name, @Value)";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, settings);
                    response.Status = true;
                    response.StatusMessage = "Approved and completed successfully";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.StatusMessage = "System malfunction";
                logger.Error(ex);
            }
            return response;
        }

        public Response Update(Settings settings)
        {
            Response response = new Response();
            string sql = "update settings set name = @Name, value = @Value where settingsid = @SettingsId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, settings);
                    response.Status = true;
                    response.StatusMessage = "Approved and completed successfully";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.StatusMessage = "System malfunction";
                logger.Error(ex);
            }
            return response;
        }

        public Response Delete(int SettingsId)
        {
            Response response = new Response();
            string sql = "delete from settings where settingsid = @SettingsId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, new { SettingsId });
                    response.Status = true;
                    response.StatusMessage = "Approved and completed successfully";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.StatusMessage = "System malfunction";
                logger.Error(ex);
            }
            return response;
        }
    }
}
