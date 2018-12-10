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
using System.Transactions;

namespace LagosHealthReminderApi.Repositories
{
    public class AppointmentsRepo
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string ConnectionString { get; set; }

        public AppointmentsRepo(IConfiguration Configuration)
        {
            this.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public List<AppointmentResponse> GetAll()
        {
            List<AppointmentResponse> list = new List<AppointmentResponse>();
            string sql = @"Select ServiceTypeId, ServiceTypeName from ServiceTypes";
            string sql1 = @"select a.PatientAppointmentId, a.PatientId, concat(b.FirstName, ' ', b.MiddleName, ' ', b.LastName) PatientName, b.Phone, b.AltPhone, b.Dob,
                            a.ServiceTypeId, d.ServiceTypeName, a.InsertUserId, c.Username InsertUser, a.InsertDate, a.UpdateUserId, e.Username UpdateUser, a.UpdateDate,
                            b.HouseNumber, b.SettlementId, f.Settlement, a.OptionType
                            from PatientAppointment a inner join patients b on a.patientid = b.patientid
                            inner join users c on a.InsertUserId = c.UserId
                            inner join ServiceTypes d on a.ServiceTypeId = d.ServiceTypeId inner join Settlements f on f.SettlementId = b.SettlementId
                            left outer join Users e on a.UpdateUserId = e.UserId where a.ServiceTypeId = @ServiceTypeId";
            string sql2 = @"select a.AppointmentId, a.PatientAppointmentId, a.ServiceKindId, b.ServiceKindName, 
                            a.AppointmentDate, a.StatusId, a.ConfirmationDate, a.InsertUserId,a.InsertDate, a.UpdateUserId, a.UpdateDate
                            from Appointments a inner join ServiceKinds b on a.ServiceKindId = b.ServiceKindId
                            where a.PatientAppointmentId = @PatientAppointmentId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    var services = conn.Query<ServiceTypes>(sql).ToList();
                    services.ForEach(service =>
                    {
                        AppointmentResponse aresponse = new AppointmentResponse()
                        {
                            ServiceTypeId = service.ServiceTypeId,
                            ServiceTypeName = service.ServiceTypeName
                        };
                        List<Appointments> appointmentList = new List<Appointments>();
                        var patientAppointments = conn.Query<PatientAppointment>(sql1, new { ServiceTypeId = service.ServiceTypeId }).ToList();
                        if (patientAppointments.Count > 0)
                        {
                            aresponse.OptionType = patientAppointments[0].OptionType;
                            patientAppointments.ForEach(item =>
                            {
                                var appointments = conn.Query<Appointments>(sql2, new { item.PatientAppointmentId }).ToList();
                                appointments.Select(c => { c.PatientId = item.PatientId; c.PatientName = item.PatientName; c.Phone = item.Phone; c.AltPhone = item.AltPhone; c.Dob = item.Dob; return c; }).ToList();
                                appointmentList.AddRange(appointments);
                            });
                        }
                        aresponse.appointments = appointmentList;
                        list.Add(aresponse);
                    });
                    
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public Appointments GetAppointment(int AppointmentId)
        {
            Appointments appointments = new Appointments();
            string sql = "SELECT * FROM APPOINTMENTS WHERE APPOINTMENTID = @AppointmentId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    appointments = conn.Query<Appointments>(sql, new { AppointmentId }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return appointments;
        }

        public List<PatientAppointmentResponse> GePatientAppointments(int PatientId)
        {
            List<PatientAppointmentResponse> list = new List<PatientAppointmentResponse>();
            string sql = @"Select ServiceTypeId, ServiceTypeName from ServiceTypes";
            string sql1 = @"select a.PatientAppointmentId, a.PatientId, concat(b.FirstName, ' ', b.MiddleName, ' ', b.LastName) PatientName, b.Phone, b.AltPhone, b.Dob,
                            a.ServiceTypeId, d.ServiceTypeName, a.InsertUserId, c.Username InsertUser, a.InsertDate, a.UpdateUserId, e.Username UpdateUser, a.UpdateDate,
                            b.HouseNumber, b.SettlementId, f.Settlement, a.OptionType
                            from PatientAppointment a inner join patients b on a.patientid = b.patientid
                            inner join users c on a.InsertUserId = c.UserId
                            inner join ServiceTypes d on a.ServiceTypeId = d.ServiceTypeId inner join Settlements f on f.SettlementId = b.SettlementId
                            left outer join Users e on a.UpdateUserId = e.UserId where a.PatientId = @PatientId and a.ServiceTypeId = @ServiceTypeId order by a.InsertDate";
            string sql2 = @"select a.AppointmentId, a.PatientAppointmentId, a.ServiceKindId, b.ServiceKindName, 
                            a.AppointmentDate, a.StatusId, a.ConfirmationDate, a.InsertUserId,a.InsertDate, a.UpdateUserId, a.UpdateDate
                            from Appointments a inner join ServiceKinds b on a.ServiceKindId = b.ServiceKindId
                            where a.PatientAppointmentId = @PatientAppointmentId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    var services = conn.Query<ServiceTypes>(sql).ToList();
                    foreach(var service in services)
                    {
                        var patientAppointments = conn.Query<PatientAppointment>(sql1, new { ServiceTypeId = service.ServiceTypeId, PatientId }).FirstOrDefault();
                        if (patientAppointments != null)
                        {
                            var appointments = conn.Query<Appointments>(sql2, new { patientAppointments.PatientAppointmentId }).FirstOrDefault();
                            PatientAppointmentResponse aresponse = new PatientAppointmentResponse()
                            {
                                ServiceTypeId = service.ServiceTypeId,
                                ServiceTypeName = service.ServiceTypeName,
                                OptionType = patientAppointments.OptionType,
                                PatientId = appointments.PatientId,
                                PatientName = appointments.PatientName,
                                Phone = appointments.Phone,
                                AltPhone = appointments.AltPhone,
                                Dob = appointments.Dob,
                                ConfirmationDate = appointments.ConfirmationDate,
                                AppointmentDate = appointments.AppointmentDate,
                                HouseNumber = appointments.HouseNumber,
                                ServiceKindId = appointments.ServiceKindId,
                                ServiceKindName = appointments.ServiceKindName,
                                Settlement = appointments.Settlement,
                                SettlementId = appointments.SettlementId,
                                StatusDescription = appointments.StatusId == 0 ? "Defaulter" : appointments.StatusId == 3 ? "Returned" : "Pending",
                                StatusId = appointments.StatusId
                            };
                            list.Add(aresponse);
                        }
                        
                    };
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public List<AppointmentResponse> GetPending()
        {
            List<AppointmentResponse> list = new List<AppointmentResponse>();
            string sql = @"Select ServiceTypeId, ServiceTypeName from ServiceTypes";
            string sql1 = @"select a.PatientAppointmentId, a.PatientId, concat(b.FirstName, ' ', b.MiddleName, ' ', b.LastName) PatientName, b.Phone, b.AltPhone, b.Dob,
                            a.ServiceTypeId, d.ServiceTypeName, a.InsertUserId, c.Username InsertUser, a.InsertDate, a.UpdateUserId, e.Username UpdateUser, a.UpdateDate,
                            b.HouseNumber, b.SettlementId, f.Settlement, a.OptionType
                            from PatientAppointment a inner join patients b on a.patientid = b.patientid
                            inner join users c on a.InsertUserId = c.UserId
                            inner join ServiceTypes d on a.ServiceTypeId = d.ServiceTypeId inner join Settlements f on f.SettlementId = b.SettlementId
                            left outer join Users e on a.UpdateUserId = e.UserId where a.ServiceTypeId = @ServiceTypeId";
            string sql2 = @"select a.AppointmentId, a.PatientAppointmentId, a.ServiceKindId, b.ServiceKindName, 
                            a.AppointmentDate, a.StatusId, a.ConfirmationDate, a.InsertUserId,a.InsertDate, a.UpdateUserId, a.UpdateDate
                            from Appointments a inner join ServiceKinds b on a.ServiceKindId = b.ServiceKindId
                            where a.PatientAppointmentId = @PatientAppointmentId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    var services = conn.Query<ServiceTypes>(sql).ToList();
                    services.ForEach(service =>
                    {
                        AppointmentResponse aresponse = new AppointmentResponse()
                        {
                            ServiceTypeId = service.ServiceTypeId,
                            ServiceTypeName = service.ServiceTypeName
                        };
                        List<Appointments> appointmentList = new List<Appointments>();
                        var patientAppointments = conn.Query<PatientAppointment>(sql1, new { ServiceTypeId = service.ServiceTypeId }).ToList();
                        if (patientAppointments.Count > 0)
                        {
                            aresponse.OptionType = patientAppointments[0].OptionType;
                            patientAppointments.ForEach(item =>
                            {
                                var appointments = conn.Query<Appointments>(sql2, new { item.PatientAppointmentId }).ToList();
                                appointments.Select(c => { c.PatientId = item.PatientId; c.PatientName = item.PatientName; c.Phone = item.Phone; c.AltPhone = item.AltPhone; c.Dob = item.Dob; return c; }).ToList();
                                appointmentList.AddRange(appointments);
                            });
                        }
                        aresponse.appointments = appointmentList;
                        list.Add(aresponse);
                    });
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public List<AppointmentResponse> GetPending(int PHCId)
        {
            List<AppointmentResponse> list = new List<AppointmentResponse>();
            string sql = @"Select ServiceTypeId, ServiceTypeName from ServiceTypes";
            string sql1 = @"select a.PatientAppointmentId, a.PatientId, concat(b.FirstName, ' ', b.MiddleName, ' ', b.LastName) PatientName, b.Phone, b.AltPhone, b.Dob,
                            a.ServiceTypeId, d.ServiceTypeName, a.InsertUserId, c.Username InsertUser, a.InsertDate, a.UpdateUserId, e.Username UpdateUser, a.UpdateDate,
                            b.HouseNumber, b.SettlementId, f.Settlement, a.OptionType
                            from PatientAppointment a inner join patients b on a.patientid = b.patientid
                            inner join users c on a.InsertUserId = c.UserId
                            inner join ServiceTypes d on a.ServiceTypeId = d.ServiceTypeId inner join Settlements f on f.SettlementId = b.SettlementId
                            left outer join Users e on a.UpdateUserId = e.UserId where b.PHCId = @PHCId and a.ServiceTypeId = @ServiceTypeId";
            string sql2 = @"select a.AppointmentId, a.PatientAppointmentId, a.ServiceKindId, b.ServiceKindName, 
                            a.AppointmentDate, a.StatusId, a.ConfirmationDate, a.InsertUserId,a.InsertDate, a.UpdateUserId, a.UpdateDate
                            from Appointments a inner join ServiceKinds b on a.ServiceKindId = b.ServiceKindId
                            where a.PatientAppointmentId = @PatientAppointmentId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    var services = conn.Query<ServiceTypes>(sql).ToList();
                    services.ForEach(service =>
                    {
                        AppointmentResponse aresponse = new AppointmentResponse()
                        {
                            ServiceTypeId = service.ServiceTypeId,
                            ServiceTypeName = service.ServiceTypeName
                        };
                        List<Appointments> appointmentList = new List<Appointments>();
                        var patientAppointments = conn.Query<PatientAppointment>(sql1, new { ServiceTypeId = service.ServiceTypeId, PHCId }).ToList();
                        if (patientAppointments.Count > 0)
                        {
                            aresponse.OptionType = patientAppointments[0].OptionType;
                            patientAppointments.ForEach(item =>
                            {
                                var appointments = conn.Query<Appointments>(sql2, new { item.PatientAppointmentId }).ToList();
                                appointments.Select(c => { c.PatientId = item.PatientId; c.PatientName = item.PatientName; c.Phone = item.Phone; c.AltPhone = item.AltPhone; c.Dob = item.Dob; return c; }).ToList();
                                appointmentList.AddRange(appointments);
                            });
                        }
                        aresponse.appointments = appointmentList;
                        list.Add(aresponse);
                    });
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public List<Appointments> GetConfirmed()
        {
            List<Appointments> list = new List<Appointments>();
            string sql = "SELECT * FROM APPOINTMENTS WHERE STATUS = 3";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    list = conn.Query<Appointments>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public List<AppointmentResponse> GetDefaulters()
        {
            List<AppointmentResponse> list = new List<AppointmentResponse>();
            string sql = @"Select ServiceTypeId, ServiceTypeName from ServiceTypes";
            string sql1 = @"select a.PatientAppointmentId, a.PatientId, concat(b.FirstName, ' ', b.MiddleName, ' ', b.LastName) PatientName, b.Phone, b.AltPhone, b.Dob,
                            a.ServiceTypeId, d.ServiceTypeName, a.InsertUserId, c.Username InsertUser, a.InsertDate, a.UpdateUserId, e.Username UpdateUser, a.UpdateDate,
                            b.HouseNumber, b.SettlementId, f.Settlement, a.OptionType
                            from PatientAppointment a inner join patients b on a.patientid = b.patientid
                            inner join users c on a.InsertUserId = c.UserId
                            inner join ServiceTypes d on a.ServiceTypeId = d.ServiceTypeId inner join Settlements f on f.SettlementId = b.SettlementId
                            left outer join Users e on a.UpdateUserId = e.UserId where a.ServiceTypeId = @ServiceTypeId";
            string sql2 = @"select a.AppointmentId, a.PatientAppointmentId, a.ServiceKindId, b.ServiceKindName, 
                            a.AppointmentDate, a.StatusId, a.ConfirmationDate, a.InsertUserId,a.InsertDate, a.UpdateUserId, a.UpdateDate
                            from Appointments a inner join ServiceKinds b on a.ServiceKindId = b.ServiceKindId
                            where a.PatientAppointmentId = @PatientAppointmentId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    var services = conn.Query<ServiceTypes>(sql).ToList();
                    services.ForEach(service =>
                    {
                        AppointmentResponse aresponse = new AppointmentResponse()
                        {
                            ServiceTypeId = service.ServiceTypeId,
                            ServiceTypeName = service.ServiceTypeName
                        };
                        List<Appointments> appointmentList = new List<Appointments>();
                        var patientAppointments = conn.Query<PatientAppointment>(sql1, new { ServiceTypeId = service.ServiceTypeId }).ToList();
                        if (patientAppointments.Count > 0)
                        {
                            aresponse.OptionType = patientAppointments[0].OptionType;
                            patientAppointments.ForEach(item =>
                            {
                                var appointments = conn.Query<Appointments>(sql2, new { item.PatientAppointmentId }).ToList();
                                appointments.Select(c => { c.PatientId = item.PatientId; c.PatientName = item.PatientName; c.Phone = item.Phone; c.AltPhone = item.AltPhone; c.Dob = item.Dob; return c; }).ToList();
                                appointmentList.AddRange(appointments);
                            });
                        }
                        aresponse.appointments = appointmentList;
                        list.Add(aresponse);
                    });
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public List<AppointmentResponse> GetDefaulters(int PHCId)
        {
            List<AppointmentResponse> list = new List<AppointmentResponse>();
            string sql = @"Select ServiceTypeId, ServiceTypeName from ServiceTypes";
            string sql1 = @"select a.PatientAppointmentId, a.PatientId, concat(b.FirstName, ' ', b.MiddleName, ' ', b.LastName) PatientName, b.Phone, b.AltPhone, b.Dob,
                            a.ServiceTypeId, d.ServiceTypeName, a.InsertUserId, c.Username InsertUser, a.InsertDate, a.UpdateUserId, e.Username UpdateUser, a.UpdateDate,
                            b.HouseNumber, b.SettlementId, f.Settlement, a.OptionType
                            from PatientAppointment a inner join patients b on a.patientid = b.patientid
                            inner join users c on a.InsertUserId = c.UserId
                            inner join ServiceTypes d on a.ServiceTypeId = d.ServiceTypeId inner join Settlements f on f.SettlementId = b.SettlementId
                            left outer join Users e on a.UpdateUserId = e.UserId where b.PHCId = @PHCId and a.ServiceTypeId = @ServiceTypeId";
            string sql2 = @"select a.AppointmentId, a.PatientAppointmentId, a.ServiceKindId, b.ServiceKindName, 
                            a.AppointmentDate, a.StatusId, a.ConfirmationDate, a.InsertUserId,a.InsertDate, a.UpdateUserId, a.UpdateDate
                            from Appointments a inner join ServiceKinds b on a.ServiceKindId = b.ServiceKindId
                            where a.PatientAppointmentId = @PatientAppointmentId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    var services = conn.Query<ServiceTypes>(sql).ToList();
                    services.ForEach(service =>
                    {
                        AppointmentResponse aresponse = new AppointmentResponse()
                        {
                            ServiceTypeId = service.ServiceTypeId,
                            ServiceTypeName = service.ServiceTypeName,
                        };
                        List<Appointments> appointmentList = new List<Appointments>();
                        var patientAppointments = conn.Query<PatientAppointment>(sql1, new { ServiceTypeId = service.ServiceTypeId, PHCId }).ToList();
                        if (patientAppointments.Count > 0)
                        {
                            aresponse.OptionType = patientAppointments[0].OptionType;
                            patientAppointments.ForEach(item =>
                            {
                                var appointments = conn.Query<Appointments>(sql2, new { item.PatientAppointmentId }).ToList();
                                appointments.Select(c => { c.PatientId = item.PatientId; c.PatientName = item.PatientName; c.Phone = item.Phone; c.AltPhone = item.AltPhone; c.Dob = item.Dob; return c; }).ToList();
                                appointmentList.AddRange(appointments);
                            });
                        }
                        aresponse.appointments = appointmentList;
                        list.Add(aresponse);
                    });
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public Response CreateAppointment(AppointmentRequest appointments)
        {
            Response response = new Response();
            string sql = @"INSERT INTO PatientAppointment (PATIENTID, SERVICETYPEID, OPTIONTYPE,
                            INSERTUSERID, INSERTDATE) VALUES (@PatientId, @ServiceTypeId, @OptionType,
                            @InsertUserId, GetDate()); SELECT CAST(SCOPE_IDENTITY() as int)";
            string sql2 = @"insert into Appointments (PatientAppointmentId, ServiceKindId, AppointmentDate, StatusId, InsertUserId, InsertDate)
                            values(@PatientAppointmentId, @ServiceKindId, @AppointmentDate, 1, @InsertUserId, GetDate())";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Open();
                    using (var tran = new TransactionScope())
                    {
                        int PatientAppointmentId = conn.Query<int>(sql, appointments).FirstOrDefault();
                        //var result = conn.Query<AppointmentsContext>(sql1, new { PatientAppointmentId }).FirstOrDefault();
                        var result = new AppointmentsContext()
                        {
                            PatientAppointmentId = PatientAppointmentId,
                            ServiceKindId = appointments.ServiceKindId,
                            AppointmentDate = appointments.AppointmentDate,
                            InsertUserId = appointments.InsertUserId,
                            StatusId = 1
                        };
                        conn.Execute(sql2, result);
                        tran.Complete();
                    }
                    if(conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
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

        public Response SetAppointment(NewAppointmentRequest appointments)
        {
            Response response = new Response();
            string sql = @"insert into Appointments (PatientAppointmentId, ServiceKindId, AppointmentDate, StatusId, InsertUserId, InsertDate)
                            select top 1 * from (select b.PatientAppointmentId, a.ServiceKindId, @AppointmentDate, 1 StatusId, @InsertUserId, GetDate() InsertDate
                             from ServiceKinds a inner join PatientAppointment b on a.ServiceTypeId = b.ServiceTypeId
                            left outer join Appointments c on c.PatientAppointmentId = b.PatientAppointmentId
                            where b.PatientAppointmentId = @PatientAppointmentId and c.ServiceKindId <> a.ServiceKindId) T1";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, appointments);
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

        public Response UpdateAppointment(AppointmentsContext appointments)
        {
            Response response = new Response();
            string sql = @"UPDATE APPOINTMENTS SET SERVICETYPEID = @ServiceTypeId, 
                            APPOINTMENTDATE = @AppointmentDate WHERE APPOINTMENTID = @AppointmentId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, appointments);
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

        public Response DeleteAppointment(int AppointmentId)
        {
            Response response = new Response();
            string sql = "DELETE FROM APPOINTMENTS WHERE APPOINTMENTID = @AppointmentId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, new { AppointmentId });
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
