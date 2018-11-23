using Dapper;
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
    public class CentresRepo
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public string ConnectionString { get; set; }

        public CentresRepo(IConfiguration Configuration)
        {
            this.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public List<Centres>  GetAllCentres()
        {
            List<Centres> list = new List<Centres>();
            string sql = "SELECT * FROM CENTRES";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    list = conn.Query<Centres>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public Centres GetCentre(int CentreId)
        {
            Centres centres = new Centres();
            string sql = "SELECT * FROM CENTRES WHERE CENTREID = ?CentreId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    centres = conn.Query<Centres>(sql, new { CentreId }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return centres;
        }

        public Response CreateCentre(Centres centres)
        {
            Response response = new Response();
            string sql = "";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, centres);
                    response.Status = true;
                    response.StatusMessage = "Approved and competed successfully";
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

        public Response UpdateCentre(Centres centres)
        {
            Response response = new Response();
            string sql = "";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, centres);
                    response.Status = true;
                    response.StatusMessage = "Approved and competed successfully";
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

        public Response DeleteCentre(int CentreId)
        {
            Response response = new Response();
            string sql = "DELETE FROM CENTRES WHERE CENTREID = ?CentreId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, new { CentreId });
                    response.Status = true;
                    response.StatusMessage = "Approved and competed successfully";
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
