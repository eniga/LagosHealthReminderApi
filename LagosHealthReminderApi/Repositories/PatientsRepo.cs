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
            string sql = @"SELECT a.PatientId, a.QrCode, b.QrCodeImage, i.PHCId, i.PHC,
                            a.FirstName, a.MiddleName, a.LastName, a.Phone, a.AltPhone,
                            a.Email, a.Dob, a.SettlementId, c.Settlement, c.WardId, d.Ward,
                            d.LGAId, e.LGA, e.StateId, f.State, a.InsertUserId, g.username InsertUser,
                            a.InsertDate, a.UpdateDate, a.UpdateUserId, h.Username UpdateUser
                            from Patients a 
                            left outer join QrCodes b on b.QrCode = a.QrCode
                            inner join Settlements c on c.SettlementId = a.SettlementId
                            inner join Wards d on d.WardId = c.WardId
                            inner join LGAs e on e.LGAId = d.LGAId
                            inner join States f on f.StateId = e.StateId
                            inner join users g on g.UserId = a.InsertUserId
                            left outer join PHCs i on i.WardId = c.WardId
                            left outer join Users h on h.UserId = a.UpdateUserId";
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
            string sql = @"SELECT a.PatientId, a.QrCode, b.QrCodeImage, i.PHCId, i.PHC,
                            a.FirstName, a.MiddleName, a.LastName, a.Phone, a.AltPhone,
                            a.Email, a.Dob, a.SettlementId, c.Settlement, c.WardId, d.Ward,
                            d.LGAId, e.LGA, e.StateId, f.State, a.InsertUserId, g.username InsertUser,
                            a.InsertDate, a.UpdateDate, a.UpdateUserId, h.Username UpdateUser
                            from Patients a 
                            left outer join QrCodes b on b.QrCode = a.QrCode
                            inner join Settlements c on c.SettlementId = a.SettlementId
                            inner join Wards d on d.WardId = c.WardId
                            inner join LGAs e on e.LGAId = d.LGAId
                            inner join States f on f.StateId = e.StateId
                            inner join users g on g.UserId = a.InsertUserId
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
            string sql = @"SELECT a.PatientId, a.QrCode, b.QrCodeImage, i.PHCId, i.PHC,
                            a.FirstName, a.MiddleName, a.LastName, a.Phone, a.AltPhone,
                            a.Email, a.Dob, a.SettlementId, c.Settlement, c.WardId, d.Ward,
                            d.LGAId, e.LGA, e.StateId, f.State, a.InsertUserId, g.username InsertUser,
                            a.InsertDate, a.UpdateDate, a.UpdateUserId, h.Username UpdateUser
                            from Patients a 
                            left outer join QrCodes b on b.QrCode = a.QrCode
                            inner join Settlements c on c.SettlementId = a.SettlementId
                            inner join Wards d on d.WardId = c.WardId
                            inner join LGAs e on e.LGAId = d.LGAId
                            inner join States f on f.StateId = e.StateId
                            inner join users g on g.UserId = a.InsertUserId
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

        public CreatePatientResponse Create(PatientContext context)
        {
            CreatePatientResponse response = new CreatePatientResponse();
            string sql = @"INSERT INTO PATIENTS (FIRSTNAME, MIDDLENAME, LASTNAME, PHONE, ALTPHONE, EMAIL, DOB, SETTLEMENTID, INSERTUSERID, INSERTDATE, QRCODE, PHCID) VALUES
                            (@FirstName, @MiddleName, @LastName, @Phone, @AltPhone, @Email, @Dob, @SettlementId, @InsertUserId, GetDate(), @QrCode, @PHCId); SELECT CAST(SCOPE_IDENTITY() as int)";
            try
            {
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
                response.StatusMessage = "System Malfunction";
                logger.Error(ex);
            }
            return response;
        }

        public Response Update(PatientContext context)
        {
            Response response = new Response();
            string sql = @"UPDATE PATIENTS SET FIRSTNAME = @FirstName, MiddleName = @MiddleName, LastName = @LastName, Phone = @Phone,
                            AltPhone = @AltPhone, Email = @Email, Dob = @Dob, SettlementId = @SettlementId, UpdateUserId = @UpdateUserId, 
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

    }
}
