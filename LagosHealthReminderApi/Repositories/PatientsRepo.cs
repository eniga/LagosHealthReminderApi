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
    public class PatientsRepo
    {
        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public string ConnectionString { get; set; }

        public PatientsRepo(IConfiguration configuration)
        {
            this.ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public List<Patients> ReadAll()
        {
            List<Patients> list = new List<Patients>();
            string sql = @"SELECT a.PatientId, a.QrCode, a.PHCId, i.PHC,
                            a.FirstName, a.MiddleName, a.LastName, a.Phone, a.AltPhone,
                            a.Email, a.Dob, a.SettlementId, c.Settlement, c.WardId, d.Ward,
                            d.LGAId, e.LGA, e.StateId, f.State, a.InsertUserId, g.username InsertUser,
                            a.InsertDate, a.UpdateDate, a.UpdateUserId, h.Username UpdateUser, a.HouseNumber
                            from Patients a 
                            left outer join Settlements c on c.SettlementId = a.SettlementId
                            left outer join Wards d on d.WardId = c.WardId
                            left outer join LGAs e on e.LGAId = d.LGAId
                            left outer join States f on f.StateId = e.StateId
                            left outer join users g on g.UserId = a.InsertUserId
                            left outer join PHCs i on i.PHCId = a.PHCId
                            left outer join Users h on h.UserId = a.UpdateUserId order by a.PatientId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    list = conn.Query<Patients>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public Patients Read(int PatientId)
        {
            Patients patient = new Patients();
            string sql = @"SELECT a.PatientId, a.QrCode, i.PHCId, i.PHC,
                            a.FirstName, a.MiddleName, a.LastName, a.Phone, a.AltPhone,
                            a.Email, a.Dob, a.SettlementId, c.Settlement, c.WardId, d.Ward,
                            d.LGAId, e.LGA, e.StateId, f.State, a.InsertUserId, g.username InsertUser,
                            a.InsertDate, a.UpdateDate, a.UpdateUserId, h.Username UpdateUser, a.HouseNumber
                            from Patients a 
                            left outer join Settlements c on c.SettlementId = a.SettlementId
                            left outer join Wards d on d.WardId = c.WardId
                            left outer join LGAs e on e.LGAId = d.LGAId
                            left outer join States f on f.StateId = e.StateId
                            left outer join users g on g.UserId = a.InsertUserId
                            left outer join PHCs i on i.WardId = c.WardId
                            left outer join Users h on h.UserId = a.UpdateUserId WHERE a.PatientId = @PatientId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    patient = conn.Query<Patients>(sql, new { PatientId }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return patient;
        }

        public Patients ReadQrCode(string QrCode)
        {
            Patients patient = new Patients();
            string sql = @"SELECT a.PatientId, a.QrCode, i.PHCId, i.PHC,
                            a.FirstName, a.MiddleName, a.LastName, a.Phone, a.AltPhone,
                            a.Email, a.Dob, a.SettlementId, c.Settlement, c.WardId, d.Ward,
                            d.LGAId, e.LGA, e.StateId, f.State, a.InsertUserId, g.username InsertUser,
                            a.InsertDate, a.UpdateDate, a.UpdateUserId, h.Username UpdateUser, a.HouseNumber
                            from Patients a 
                            left outer join Settlements c on c.SettlementId = a.SettlementId
                            left outer join Wards d on d.WardId = c.WardId
                            left outer join LGAs e on e.LGAId = d.LGAId
                            left outer join States f on f.StateId = e.StateId
                            left outer join users g on g.UserId = a.InsertUserId
                            left outer join PHCs i on i.WardId = c.WardId
                            left outer join Users h on h.UserId = a.UpdateUserId WHERE a.QrCode = @QrCode";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    patient = conn.Query<Patients>(sql, new { QrCode }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return patient;
        }

        public List<Patients> ReadByPhone(string PhoneNumber)
        {
            List<Patients> list = new List<Patients>();
            string sql = @"SELECT a.PatientId, a.QrCode, i.PHCId, i.PHC,
                            a.FirstName, a.MiddleName, a.LastName, a.Phone, a.AltPhone,
                            a.Email, a.Dob, a.SettlementId, c.Settlement, c.WardId, d.Ward,
                            d.LGAId, e.LGA, e.StateId, f.State, a.InsertUserId, g.username InsertUser,
                            a.InsertDate, a.UpdateDate, a.UpdateUserId, h.Username UpdateUser, a.HouseNumber
                            from Patients a 
                            left outer join Settlements c on c.SettlementId = a.SettlementId
                            left outer join Wards d on d.WardId = c.WardId
                            left outer join LGAs e on e.LGAId = d.LGAId
                            left outer join States f on f.StateId = e.StateId
                            left outer join users g on g.UserId = a.InsertUserId
                            left outer join PHCs i on i.WardId = c.WardId
                            left outer join Users h on h.UserId = a.UpdateUserId where SUBSTRING(a.Phone, LEN(a.Phone) - 9, 10) = @PhoneNumber";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    PhoneNumber = PhoneNumber.Substring(PhoneNumber.Length - 10, 10);
                    list = conn.Query<Patients>(sql, new { PhoneNumber }).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public Response UpdateQrCode(int PatientId, string QrCode)
        {
            Response response = new Response();
            string sql = "update patients set qrcode = @QrCode where patientid = @PatientId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    var result = ReadQrCode(QrCode);
                    if (result == null)
                    {
                        conn.Execute(sql, new { PatientId, QrCode });
                        response.Status = true;
                        response.StatusMessage = "Approved and completed successfully";
                    }
                    else
                    {
                        response.Status = false;
                        response.StatusMessage = "QR Code already assigned to a patient";
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

        public CreatePatientResponse Create(PatientContext context)
        {
            CreatePatientResponse response = new CreatePatientResponse();
            string sql = @"INSERT INTO PATIENTS (FIRSTNAME, MIDDLENAME, LASTNAME, PHONE, ALTPHONE, EMAIL, DOB, SETTLEMENTID, INSERTUSERID, INSERTDATE, QRCODE, PHCID, HOUSENUMBER) VALUES
                            (@FirstName, @MiddleName, @LastName, @Phone, @AltPhone, @Email, @Dob, @SettlementId, @InsertUserId, GetDate(), @QrCode, @PHCId, @HouseNumber); SELECT CAST(SCOPE_IDENTITY() as int)";
            try
            {
                if(context.Dob > DateTime.Now)
                {
                    response.Status = false;
                    response.StatusMessage = "Invalid Date as date cannot be in the future.";
                    return response;
                }
                if(context.Phone.Length != 11)
                {
                    response.Status = false;
                    response.StatusMessage = "Invalid Phone Number";
                    return response;
                }
                if(string.IsNullOrEmpty(context.QrCode))
                {
                    response.Status = false;
                    response.StatusMessage = "Kindly assign a QR Code to this patient";
                    return response;
                }
                if(!string.IsNullOrEmpty(context.QrCode))
                {
                    var result = ReadQrCode(context.QrCode);
                    if (result != null)
                    {
                        response.Status = false;
                        response.StatusMessage = "QR Code already assigned to a patient";
                        return response;
                    }
                }
                using (IDbConnection conn = GetConnection())
                {
                    UsersRepo usersRepo = new UsersRepo(ConnectionString);
                    context.PHCId = usersRepo.GetUser(context.InsertUserId).PHCId;
                    int id = conn.Query<int>(sql, context).FirstOrDefault();
                    response.Status = true;
                    response.StatusMessage = "Approved and completed successfully";
                    response.PatientId = id;
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.StatusMessage = ex.Message; // "System Malfunction";
                logger.Error(ex);
            }
            return response;
        }

        public Response Update(PatientContext context)
        {
            Response response = new Response();
            string sql = @"UPDATE PATIENTS SET FIRSTNAME = @FirstName, MiddleName = @MiddleName, LastName = @LastName, Phone = @Phone, HouseNumber = @HouseNumber,
                            AltPhone = @AltPhone, Email = @Email, Dob = @Dob, SettlementId = @SettlementId, UpdateUserId = @UpdateUserId, QrCode = @QrCode,
                            UpdateDate = GetDate() where PatientId = @PatientId";
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

        public Response Delete(int PatientId)
        {
            Response response = new Response();
            string sql = @"DELETE FROM PATIENTS WHERE PATIENTID = @PatientId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, new { PatientId });
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

        public List<AppointmentResponse> GetAppointments(int PatientId)
        {
            List<AppointmentResponse> list = new List<AppointmentResponse>();
            string sql = @"Select ServiceTypeId, ServiceTypeName from ServiceTypes";
            string sql1 = @"select a.PatientAppointmentId, a.PatientId, concat(b.FirstName, ' ', b.MiddleName, ' ', b.LastName) PatientName, b.Phone, b.AltPhone, b.Dob,
                            a.ServiceTypeId, d.ServiceTypeName, a.InsertUserId, c.Username InsertUser, a.InsertDate, a.UpdateUserId, e.Username UpdateUser, a.UpdateDate 
                            from PatientAppointment a inner join patients b on a.patientid = b.patientid
                            left outer join users c on a.InsertUserId = c.UserId
                            inner join ServiceTypes d on a.ServiceTypeId = d.ServiceTypeId
                            left outer join Users e on a.UpdateUserId = e.UserId where a.PatientId = @PatientId and a.ServiceTypeId = @ServiceTypeId";
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
                        var patientAppointments = conn.Query<PatientAppointment>(sql1, new { ServiceTypeId = service.ServiceTypeId, PatientId }).ToList();
                        if (patientAppointments.Count > 0)
                        {
                            aresponse.OptionType = patientAppointments[0].OptionType;
                            patientAppointments.ForEach(item =>
                            {
                                var appointments = conn.Query<Appointments>(sql2, new { item.PatientAppointmentId }).FirstOrDefault();
                                appointments.AltPhone = item.AltPhone;
                                appointments.PatientId = item.PatientId;
                                appointments.PatientName = item.PatientName;
                                appointments.Phone = item.Phone;
                                appointments.Dob = item.Dob;
                                appointmentList.Add(appointments);
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

    }
}
