using Dapper;
using LagosHealthReminderApi.DbContext;
using LagosHealthReminderApi.Models;
using Microsoft.Extensions.Configuration;
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
    public class QrCodeRepo
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string ConnectionString { get; set; }

        public QrCodeRepo(IConfiguration Configuration)
        {
            this.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public List<QrCodes> Read()
        {
            List<QrCodes> list = new List<QrCodes>();
            string sql = @"Select a.QrCodeId, a.QrCode, a.InsertUserId,
                            b.Username InsertUser, a.InsertDate, a.UpdateUserId, 
                            c.Username UpdateUser, a.UpdateDate, a.printstatus
                            from QrCodes a inner join Users b on b.UserId = a.InsertUserId
                            left outer join Users c on c.UserId = a.UpdateUserId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    list = conn.Query<QrCodes>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public QrCodeStats GetStats()
        {
            QrCodeStats obj = new QrCodeStats();
            string sql = @"select count(a.QrCode) Generated, SUM(CASE a.PrintStatus WHEN 1 THEN 1 ELSE 0 END) Unprinted,
                SUM(CASE a.PrintStatus WHEN 2 THEN 1 ELSE 0 END) Printed, count(b.PatientId) Mapped,
                (select count(*) from qrcodes where cast(qrcode as varchar) not in (select qrcode from patients)) Unmapped
                from QrCodes a left outer join patients b on cast(a.QrCode as VARCHAR) = b.QrCode ";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    obj = conn.Query<QrCodeStats>(sql).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return obj;
        }

        public List<QrCodes> Unprinted()
        {
            List<QrCodes> list = new List<QrCodes>();
            string sql = @"Select a.QrCodeId, a.QrCode, a.InsertUserId,
                            b.Username InsertUser, a.InsertDate, a.UpdateUserId, 
                            c.Username UpdateUser, a.UpdateDate, a.printstatus
                            from QrCodes a inner join Users b on b.UserId = a.InsertUserId
                            left outer join Users c on c.UserId = a.UpdateUserId where a.printstatus = 1";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    list = conn.Query<QrCodes>(sql).ToList();
                    if(list.Count > 0)
                    {
                        list.Select(item => { item.QrCodeImage = GenerateQrCodeImage(item.QrCode); return item; }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public Response Printed()
        {
            Response response = new Response();
            string sql = "UPDATE QRCODES SET PRINTSTATUS = 2 WHERE PRINTSTATUS = 1";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql);
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

        public Response Create(QrCodesRequest request)
        {
            Response response = new Response();
            string sql = "dbo.Generate";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, request, commandType: CommandType.StoredProcedure);
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

        public Response Update(QrCodeContext context)
        {
            Response response = new Response();
            string sql = "UPDATE QRCODES SET QRCODE = @QrCode, UPDATEUSERID = @UpdateUserId, UPDATEDATE = GetDate() WHERE QrCodeId = @QrCodeId";
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

        public Response Delete(int QrCodeId)
        {
            Response response = new Response();
            string sql = "DELETE FROM QRCODE WHERE SERVICETYPEID = @QrCodeId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, new { QrCodeId });
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

        public List<string> GenerateCodes(int n)
        {
            List<string> codes = new List<string>();
            int i = 0;
            while(i < n)
            {
                string code = GenerateSingle();
                codes.Add(code);
                i++;
            }
            return codes;
        }

        public string GenerateSingle()
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return unixTimestamp.GetHashCode().ToString() + GetRandom();
        }

        public string GetRandom()
        {
            Random random = new Random();
            string result = random.Next(0, 99999).ToString("D6");
            return result;
        }

        public byte[] GenerateQrCodeImage(string QrCode)
        {
            string result = string.Empty;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(QrCode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            //qrCodeImage.Save("testimage", ImageFormat.Jpeg);
            using (var m = new MemoryStream())
            {
                qrCodeImage.Save(m, ImageFormat.Jpeg);
                return m.ToArray();
            }
        }

        public List<QrCodes> MyTest(int number)
        {
            List<QrCodes> list = new List<QrCodes>();
            try
            {
                var codes = GenerateCodes(number);
                codes.ForEach(item =>
                {
                    QrCodes code = new QrCodes();
                    code.QrCode = item;
                    code.QrCodeImage = GenerateQrCodeImage(item);
                    list.Add(code);
                });
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }
    }
}
