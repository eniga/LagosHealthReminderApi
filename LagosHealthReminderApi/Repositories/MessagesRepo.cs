using Dapper;
using LagosHealthReminderApi.DbContext;
using LagosHealthReminderApi.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.Repositories
{
    public class MessagesRepo
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public string ConnectionString;
        private string username = "bolajiworld@gmail.com";
        private string password = "password";

        public MessagesRepo(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public MessageResponse SendMessage(MessageRequest request)
        {
            MessageResponse response = new MessageResponse();
            try
            {
                var service = new messagerService.messagerSoapClient(messagerService.messagerSoapClient.EndpointConfiguration.messagerSoap);
                var serviceResponse = service.SendSingleSMSAsync(username, password, request.Message, "myHealth", request.Destination).Result;
                var result = serviceResponse.Body.SendSingleSMSResult.ToString();
                string jsonString = JsonConvert.DeserializeObject<string>(result);
                MessageServiceResponse json = JsonConvert.DeserializeObject<MessageServiceResponse>(jsonString);
                response.Status = true;
                response.StatusMessage = "Approved and completed successfully";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.StatusMessage = "System malfunction";
                logger.Error(ex);
            }
            return response;
        }

        public Response GetBalance()
        {
            var service = new messagerService.messagerSoapClient(messagerService.messagerSoapClient.EndpointConfiguration.messagerSoap);
            var serviceResponse = service.GetBalanceAsync(username, password).Result;
            var result = serviceResponse.Body.GetBalanceResult.ToString();
            string jsonString = JsonConvert.DeserializeObject<string>(result);
            BalanceResult json = JsonConvert.DeserializeObject<BalanceResult>(jsonString);
            Response response = new Response()
            {
                Status = true,
                StatusMessage = json.balance.ToString()
            };
            return response;
        }

        public Response SendCampaign(CampaignMessage campaign)
        {
            Response response = new Response();
            string sql = "INSERT INTO CAMPAIGNMESSAGES (LGAID, INSERTUSERID, INSERTDATE, MESSAGE) VALUES (@LGAId, @InsertUserId, GetDate(), @Message)";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, campaign);
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

        public List<CampaignMessage> GetCampaignMessages()
        {
            List<CampaignMessage> list = new List<CampaignMessage>();
            string sql = @"select a.CampaignId, a.LGAId, b.LGA, a.InsertUserId, c.Username InsertUser,
                            a.InsertDate, a.DateSent, a.Message, a.Status
                            from CampaignMessages a
                            inner join LGAs b on a.LGAId = b.LGAId
                            inner join Users c on a.InsertUserId = c.UserId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    list = conn.Query<CampaignMessage>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }
    }
}
