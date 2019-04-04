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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.Repositories
{
    public class MessagesRepo
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public string ConnectionString;

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
                var details = GetSMSDetails();
                var service = new messagerService.messagerSoapClient(messagerService.messagerSoapClient.EndpointConfiguration.messagerSoap);
                var serviceResponse = service.SendSingleSMSAsync(details.Username, details.Password, request.Message, "myHealth", request.Destination).Result;
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
            Response response = new Response();
            try
            {
                var details = GetSMSDetails();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(details.Url);
                    HttpResponseMessage result = client.GetAsync($"?username={details.Username}&password={details.Password}&action=balance").Result;
                    
                    if(result.IsSuccessStatusCode)
                    {
                        var apiResponse = result.Content.ReadAsAsync<BalanceResult>().Result;
                        response.Status = true;
                        response.StatusMessage = apiResponse.balance.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
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
            string sql = @"select a.CampaignId, a.LGAId, (CASE WHEN a.LGAId = 999 THEN 'All' ELSE b.LGA END) LGA, a.InsertUserId, c.Username InsertUser,
                            a.InsertDate, a.DateSent, a.Message, a.Status
                            from CampaignMessages a
                            left outer join LGAs b on a.LGAId = b.LGAId
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

        public Response DeleteCampaignMessage(int CampaignId)
        {
            Response response = new Response();
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Delete<CampaignMessageContext>(CampaignId);
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
    }
}
