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
    public class MessengerRepo
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string ConnectionString { get; set; }

        public MessengerRepo(IConfiguration Configuration)
        {
            this.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public SMSDetails GetSMSDetails()
        {
            SMSDetails details = new SMSDetails();
            string sql = "select * from smsdetails";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    details = conn.Query<SMSDetails>(sql).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return details;
        }

        public GetBalanceResult GetSMSBalance()
        {
            GetBalanceResult balance = new GetBalanceResult();
            var details = GetSMSDetails();
            try
            {
                var service = new messagerService.messagerSoapClient(messagerService.messagerSoapClient.EndpointConfiguration.messagerSoap);
                var result = service.GetBalanceAsync(details.Username, details.Password).Result;
                var json = JsonConvert.DeserializeObject<string>(result.Body.GetBalanceResult);
                balance = JsonConvert.DeserializeObject<GetBalanceResult>(json);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return balance;
        }
    }
}
