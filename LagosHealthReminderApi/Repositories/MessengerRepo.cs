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
using System.Net.Http;
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
            
            try
            {
                var details = GetSMSDetails();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(details.Url);
                    HttpResponseMessage result = client.GetAsync($"?username={details.Username}&password={details.Password}&balance=true").Result;

                    if (result.IsSuccessStatusCode)
                    {
                        var Apiresponse = result.Content.ReadAsStringAsync().Result;
                        //balance = JsonConvert.DeserializeObject<GetBalanceResult>(Apiresponse);
                        balance.balance = Apiresponse;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return balance;
        }

        public Response SendMessage(SMSRequest request)
        {
            Response response = new Response();
            try
            {
                var details = GetSMSDetails();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(details.Url);
                    HttpResponseMessage result = client.GetAsync($"?username={details.Username}&password={details.Password}&sender={details.AppName}&recipient={request.Phone}&message={request.Message}").Result;

                    if (result.IsSuccessStatusCode)
                    {
                        var Apiresponse = result.Content.ReadAsStringAsync().Result;
                        //var json = JsonConvert.DeserializeObject<SMSResponse>(Apiresponse);
                        if(Apiresponse.Contains("OK"))
                        {
                            response.Status = true;
                            response.StatusMessage = "Approved and completed successfully";
                            UpdateSMSSent();
                        }
                        else
                        {
                            response.Status = false;
                            response.StatusMessage = "Failed to send SMS";
                        }
                    }
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

        public Response SendReminder()
        {
            Response response = new Response();
            string sql = @"select a.AppointmentId, a.AppointmentDate, b.ServiceKindName, c.ServiceTypeName, c.SMSMessage, e.FirstName, e.LastName, e.Phone, e.PatientId 
                            from Appointments a inner join ServiceKinds b on a.ServiceKindId = b.ServiceKindId
                            inner join ServiceTypes c on b.ServiceTypeId = c.ServiceTypeId
                            inner join PatientAppointment d on a.PatientAppointmentId = d.PatientAppointmentId
                            inner join Patients e on d.PatientId = e.PatientId
                            WHERE CAST(a.AppointmentDate AS DATE) = CAST(GETDATE() + 1 AS DATE) AND (a.ReminderSent = 2 OR a.ReminderSent IS NULL)";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    var result = conn.Query<SMSNotificationRequest>(sql).ToList();
                    result.ForEach(item =>
                    {
                        var agentTask = new List<Task>();
                        agentTask.Add(Task.Factory.StartNew(() =>
                        {
                            item.SMSMessage = item.SMSMessage.Replace("[firstname]", item.FirstName);
                            item.Phone = "234" + item.Phone.Substring(item.Phone.Length - 10, 10);
                            SMSRequest request = new SMSRequest()
                            {
                                Message = item.SMSMessage,
                                Phone = item.Phone
                            };
                            response = SendMessage(request);
                            var s = new ReminderMessages()
                            {
                                AppointmentId = item.AppointmentId,
                                Message = item.SMSMessage,
                                PatientId = item.PatientId,
                                PhoneNumber = item.Phone,
                                Sent = response.Status
                            };
                            conn.Insert<ReminderMessages>(s);
                            conn.Execute("update Appointments set ReminderSent = 3 where AppointmentId = @AppointmentId", new { item.AppointmentId });
                        }));
                        Task.WaitAny(agentTask.ToArray());
                        
                    });
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return response;
        }

        public void UpdateSMSSent()
        {
            string sql = "update SMSDetails set sent = sent + 1, balance = balance - 1, lastsent = GetDate()";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
    }
}
