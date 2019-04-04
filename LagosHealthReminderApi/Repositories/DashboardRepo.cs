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
    public class DashboardRepo
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string ConnectionString { get; set; }

        public DashboardRepo(IConfiguration Configuration)
        {
            this.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public int GetTotalAppointments()
        {
            int count = 0;
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    count = conn.RecordCount<AppointmentsContext>();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return count;
        }

        public int GetTotalDefaulters()
        {
            int count = 0;
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    count = conn.RecordCount<AppointmentsContext>("where AppointmentDate < GETDATE() and ConfirmationDate is null");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return count;
        }

        public int GetTotalTodayAppointments()
        {
            int count = 0;
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    count = conn.RecordCount<AppointmentsContext>("where CONVERT(DATE, AppointmentDate, 102) = CONVERT(date, GETDATE(), 102)");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return count;
        }

        public int GetTotalPatients()
        {
            int count = 0;
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    count = conn.RecordCount<PatientContext>();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return count;
        }

        public int GetTotalSettlements()
        {
            int count = 0;
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    count = conn.RecordCount<SettlementContext>();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return count;
        }

        public int GetTotalWards()
        {
            int count = 0;
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    count = conn.RecordCount<WardContext>();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return count;
        }

        public int GetTotalLGAs()
        {
            int count = 0;
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    count = conn.RecordCount<LGAContext>();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return count;
        }

        public int GetTotalPHCs()
        {
            int count = 0;
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    count = conn.RecordCount<PHCContext>();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return count;
        }

        public int GetTotalUsers()
        {
            int count = 0;
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    count = conn.RecordCount<UserContext>();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return count;
        }

        public int GetTotalServices()
        {
            int count = 0;
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    count = conn.RecordCount<ServiceTypeContext>();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return count;
        }

        public Dashboard GetClientDashboard()
        {
            Dashboard result = new Dashboard();
            string sql = @"SELECT
                        (SELECT COUNT(*) FROM Patients WHERE MONTH(InsertDate) = MONTH(GETDATE()) AND YEAR(InsertDate) = YEAR(GETDATE())) CurrentMonth,
                        (SELECT COUNT(*) FROM Patients WHERE MONTH(InsertDate) = MONTH(DATEADD(MONTH, -1, GETDATE())) AND YEAR(InsertDate) = YEAR(DATEADD(MONTH, -1, GETDATE()))) LastMonth";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    result = conn.Query<Dashboard>(sql).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return result;
        }

        public Dashboard GetAppointmentsDashboard()
        {
            Dashboard result = new Dashboard();
            string sql = @"SELECT
                        (SELECT COUNT(*) FROM Appointments WHERE MONTH(InsertDate) = MONTH(GETDATE()) AND YEAR(InsertDate) = YEAR(GETDATE())) CurrentMonth,
                        (SELECT COUNT(*) FROM Appointments WHERE MONTH(InsertDate) = MONTH(DATEADD(MONTH, -1, GETDATE())) AND YEAR(InsertDate) = YEAR(DATEADD(MONTH, -1, GETDATE()))) LastMonth";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    result = conn.Query<Dashboard>(sql).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return result;
        }

        public Dashboard GetClientsOnScheduledDashboard()
        {
            Dashboard result = new Dashboard();
            string sql = @"SELECT
                        (SELECT COUNT(*) FROM Appointments WHERE CAST(AppointmentDate as DATE) = CAST(ConfirmationDate as date) AND MONTH(InsertDate) = MONTH(GETDATE()) 
                            AND YEAR(InsertDate) = YEAR(GETDATE())) CurrentMonth,
                        (SELECT COUNT(*) FROM Appointments WHERE MONTH(InsertDate) = MONTH(DATEADD(MONTH, -1, GETDATE())) AND YEAR(InsertDate) = YEAR(DATEADD(MONTH, -1, GETDATE())) 
                            AND CAST(AppointmentDate as DATE) = CAST(ConfirmationDate as date)) LastMonth";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    result = conn.Query<Dashboard>(sql).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return result;
        }

        public Dashboard GetDefaultersDashboard()
        {
            Dashboard result = new Dashboard();
            string sql = @"SELECT
                        (SELECT COUNT(*) FROM Appointments WHERE (AppointmentDate < GETDATE() and ConfirmationDate is null) and MONTH(InsertDate) = MONTH(GETDATE()) 
                            AND YEAR(InsertDate) = YEAR(GETDATE())) CurrentMonth,
                        (SELECT COUNT(*) FROM Appointments WHERE (AppointmentDate < DATEADD(MONTH, -1, GETDATE()) and ConfirmationDate is null) and MONTH(InsertDate) = MONTH(DATEADD(MONTH, -1, GETDATE())) 
                            AND YEAR(InsertDate) = YEAR(DATEADD(MONTH, -1, GETDATE()))) LastMonth";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    result = conn.Query<Dashboard>(sql).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return result;
        }

        public Dashboard GetDefaultersReturnedDashboard()
        {
            Dashboard result = new Dashboard();
            string sql = @"SELECT
                        (SELECT COUNT(*) FROM Appointments WHERE (CAST(AppointmentDate as DATE) <> CAST(ConfirmationDate as date) AND ConfirmationDate IS NOT NULL) 
                            AND MONTH(InsertDate) = MONTH(GETDATE()) AND YEAR(InsertDate) = YEAR(GETDATE())) CurrentMonth,
                        (SELECT COUNT(*) FROM Appointments WHERE (CAST(AppointmentDate as DATE) <> CAST(ConfirmationDate as date) AND ConfirmationDate IS NOT NULL) 
                            AND MONTH(InsertDate) = MONTH(DATEADD(MONTH, -1, GETDATE())) AND YEAR(InsertDate) = YEAR(DATEADD(MONTH, -1, GETDATE()))) LastMonth";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    result = conn.Query<Dashboard>(sql).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return result;
        }

        public Dashboard GetActivePHCsDashboard()
        {
            Dashboard result = new Dashboard();
            string sql = @"SELECT
                            (SELECT COUNT(DISTINCT B.PHCId) FROM Appointments A
                            LEFT OUTER JOIN USERS B ON A.InsertUserId = B.USERID 
                            WHERE MONTH(A.InsertDate) = MONTH(GETDATE()) AND YEAR(A.InsertDate) = YEAR(GETDATE())
                            ) CurrentMonth,
                            (
                            SELECT COUNT(DISTINCT B.PHCId) FROM Appointments A
                            LEFT OUTER JOIN USERS B ON A.InsertUserId = B.USERID 
                            WHERE MONTH(A.InsertDate) = MONTH(DATEADD(MONTH, -1, GETDATE())) AND YEAR(A.InsertDate) = YEAR(DATEADD(MONTH, -1, GETDATE()))
                            ) LastMonth";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    result = conn.Query<Dashboard>(sql).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return result;
        }

        public List<string> GetActiveServices()
        {
            List<string> list = new List<string>();
            string sql = @"SELECT DISTINCT C.ServiceTypeName FROM Appointments A 
                            INNER JOIN ServiceKinds B ON A.ServiceKindId = B.ServiceKindId
                            INNER JOIN ServiceTypes C ON B.ServiceTypeId = C.ServiceTypeId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    list = conn.Query<string>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        //public List<MonthlyBreakdown> GetPatientBreakdowns(int YearId)
        //{
        //    List<MonthlyBreakdown> list = new List<MonthlyBreakdown>();
        //    string sql = @"SELECT MONTH(InsertDate) MonthId, DATENAME(month, InsertDate) as MonthName, COUNT(R.PatientId) AS TotalCount 
        //                    FROM Patients R
        //                    WHERE YEAR(InsertDate) = @YearId
        //                    GROUP BY MONTH(InsertDate), DATENAME(month, InsertDate)
        //                    ORDER BY MONTH(InsertDate)";
        //    try
        //    {
        //        using (IDbConnection conn = GetConnection())
        //        {
        //            YearId = YearId > 0 ? YearId : DateTime.Now.Year;
        //            list = conn.Query<MonthlyBreakdown>(sql, new { YearId }).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex);
        //    }
        //    return list;
        //}

        //public List<MonthlyBreakdown> GetAppointmentBreakdown(int YearId)
        //{
        //    List<MonthlyBreakdown> list = new List<MonthlyBreakdown>();
        //    string sql = @"SELECT MONTH(InsertDate) MonthId, DATENAME(month, InsertDate) as MonthName, COUNT(R.AppointmentId) AS TotalCount 
        //                    FROM Appointments R
        //                    WHERE YEAR(InsertDate) = @YearId
        //                    GROUP BY MONTH(InsertDate), DATENAME(month, InsertDate)
        //                    ORDER BY MONTH(InsertDate)";
        //    try
        //    {
        //        using (IDbConnection conn = GetConnection())
        //        {
        //            YearId = YearId > 0 ? YearId : DateTime.Now.Year;
        //            list = conn.Query<MonthlyBreakdown>(sql, new { YearId }).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex);
        //    }
        //    return list;
        //}

        //public List<MonthlyBreakdown> GetDefaulterBreakdown(int YearId)
        //{
        //    List<MonthlyBreakdown> list = new List<MonthlyBreakdown>();
        //    string sql = @"SELECT MONTH(AppointmentDate) MonthId, DATENAME(month, AppointmentDate) as MonthName, COUNT(R.AppointmentId) AS TotalCount 
        //                    FROM Appointments R
        //                    WHERE YEAR(AppointmentDate) = @YearId
        //                    AND AppointmentDate < GETDATE() and ConfirmationDate is null
        //                    GROUP BY MONTH(AppointmentDate), DATENAME(month, AppointmentDate)
        //                    ORDER BY MONTH(AppointmentDate)";
        //    try
        //    {
        //        using (IDbConnection conn = GetConnection())
        //        {
        //            YearId = YearId > 0 ? YearId : DateTime.Now.Year;
        //            list = conn.Query<MonthlyBreakdown>(sql, new { YearId }).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex);
        //    }
        //    return list;
        //}

        //public List<MonthlyBreakdown> GetAppointmentsOnScheduleBreakdown()
        //{
        //    List<MonthlyBreakdown> list = new List<MonthlyBreakdown>();
        //    string sql = @"SELECT TOP 2 YEAR(AppointmentDate) YearId, MONTH(AppointmentDate) MonthId, DATENAME(month, AppointmentDate) as MonthName, COUNT(R.AppointmentId) AS TotalCount 
        //                    FROM Appointments R
        //                    WHERE CAST(AppointmentDate as DATE) = CAST(ConfirmationDate as date)
        //                    GROUP BY YEAR(AppointmentDate), MONTH(AppointmentDate), DATENAME(month, AppointmentDate)
        //                    ORDER BY YEAR(AppointmentDate), MONTH(AppointmentDate) DESC";
        //    try
        //    {
        //        using (IDbConnection conn = GetConnection())
        //        {
        //            list = conn.Query<MonthlyBreakdown>(sql).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex);
        //    }
        //    return list;
        //}

        //public List<MonthlyBreakdown> GetReturnedPatientsBreakdown()
        //{
        //    List<MonthlyBreakdown> list = new List<MonthlyBreakdown>();
        //    string sql = @"SELECT TOP 2 YEAR(InsertDate) YearId, MONTH(InsertDate) MonthId, DATENAME(month, InsertDate) as MonthName, COUNT(R.AppointmentId) AS TotalCount 
        //                    FROM Appointments R
        //                    WHERE ConfirmationDate IS NOT NULL
        //                    GROUP BY YEAR(InsertDate), MONTH(InsertDate), DATENAME(month, InsertDate)
        //                    ORDER BY YEAR(InsertDate), MONTH(InsertDate) DESC";
        //    try
        //    {
        //        using (IDbConnection conn = GetConnection())
        //        {
        //            list = conn.Query<MonthlyBreakdown>(sql).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex);
        //    }
        //    return list;
        //}

        public int GetRandomization()
        {
            int count = 0;
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    var item = conn.Get<Settings>(1);
                    if(item != null)
                    {
                        count = int.Parse(item.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return count;
        }
    }
}
