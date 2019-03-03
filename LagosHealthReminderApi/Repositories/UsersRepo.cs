using Dapper;
using LagosHealthReminderApi.DbContext;
using LagosHealthReminderApi.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Transactions;

namespace LagosHealthReminderApi.Repositories
{
    public class UsersRepo
    {
        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public string ConnectionString { get; set; }

        public UsersRepo(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public List<Users> Read()
        {
            List<Users> list = new List<Users>();
            string sql = @"Select a.UserId, a.Username, a.DisplayName, a.Email, a.InsertUserId, b.Username as InsertUser, a.PHCId, f.PHC,
                            f.WardId, g.Ward, g.LGAId, h.LGA, h.StateId, i.State,
                            a.InsertDate, a.UpdateUserId, c.Username as UpdateUser, a.UpdateDate, a.IsActive, d.UserRoleId, d.RoleId, e.RoleName
                            from Users a left outer join Users b on b.UserId = a.InsertUserId
                            left outer join Users c on c.UserId = a.UpdateUserId
							left outer join UserRoles d on d.UserId = a.UserId
							left outer join Roles e on d.RoleId = e.RoleId
                            left outer join PHCs f on f.PHCId = a.PHCId
							left outer join Wards g on g.WardId = f.WardId
							left outer join LGAs h on h.LGAId = g.LGAId
							left outer join States i on i.StateId = h.StateId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    list = conn.Query<Users>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return list;
        }

        public Users GetUser(int UserId)
        {
            Users user = new Users();
            string sql = @"Select a.UserId, a.Username, a.DisplayName, a.Email, a.InsertUserId, b.Username as InsertUser, a.PHCId, f.PHC,
                            f.WardId, g.Ward, g.LGAId, h.LGA, h.StateId, i.State,
                            a.InsertDate, a.UpdateUserId, c.Username as UpdateUser, a.UpdateDate, a.IsActive, d.UserRoleId, d.RoleId, e.RoleName
                            from Users a left outer join Users b on b.UserId = a.InsertUserId
                            left outer join Users c on c.UserId = a.UpdateUserId
							left outer join UserRoles d on d.UserId = a.UserId
							left outer join Roles e on d.RoleId = e.RoleId
                            left outer join PHCs f on f.PHCId = a.PHCId
							left outer join Wards g on g.WardId = f.WardId
							left outer join LGAs h on h.LGAId = g.LGAId
							left outer join States i on i.StateId = h.StateId Where a.UserId = @UserId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    user = conn.Query<Users>(sql, new { UserId }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return user;
        }

        public Users GetUserByUsername(string Username)
        {
            Users user = new Users();
            string sql = @"Select a.UserId, a.Username, a.DisplayName, a.Email, a.InsertUserId, b.Username as InsertUser, a.PHCId, f.PHC,
                            f.WardId, g.Ward, g.LGAId, h.LGA, h.StateId, i.State,
                            a.InsertDate, a.UpdateUserId, c.Username as UpdateUser, a.UpdateDate, a.IsActive, d.UserRoleId, d.RoleId, e.RoleName
                            from Users a left outer join Users b on b.UserId = a.InsertUserId
                            left outer join Users c on c.UserId = a.UpdateUserId
							left outer join UserRoles d on d.UserId = a.UserId
							left outer join Roles e on d.RoleId = e.RoleId 
                            left outer join PHCs f on f.PHCId = a.PHCId
							left outer join Wards g on g.WardId = f.WardId
							left outer join LGAs h on h.LGAId = g.LGAId
							left outer join States i on i.StateId = h.StateId Where a.Username = @Username";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    user = conn.Query<Users>(sql, new { Username }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return user;
        }

        public Users GetUserByUsernameLogin(string Username)
        {
            Users user = new Users();
            string sql = @"Select a.UserId, a.Username, a.DisplayName, a.Email, a.UserId InsertUserId, a.Username as InsertUser, a.PHCId, f.PHC,
                            f.WardId, g.Ward, g.LGAId, h.LGA, h.StateId, i.State,
                            a.InsertDate, a.UserId UpdateUserId, a.Username as UpdateUser, a.UpdateDate, a.IsActive, d.UserRoleId, d.RoleId, e.RoleName
                            from Users a left outer join UserRoles d on d.UserId = a.UserId
							left outer join Roles e on d.RoleId = e.RoleId 
                            left outer join PHCs f on f.PHCId = a.PHCId
							left outer join Wards g on g.WardId = f.WardId
							left outer join LGAs h on h.LGAId = g.LGAId
							left outer join States i on i.StateId = h.StateId Where a.Username = @Username";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    user = conn.Query<Users>(sql, new { Username }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return user;
        }

        public Response Create(Users users)
        {
            Response response = new Response();
            string sql = "INSERT INTO USERS (USERNAME, DISPLAYNAME, EMAIL, SOURCE, PASSWORDHASH, PASSWORDSALT, INSERTDATE, INSERTUSERID, ISACTIVE, PHCId) ";
            sql += "VALUES(@Username, @DisplayName, @Email, 'site', @PasswordHash, @PasswordSalt, GetDate(), @InsertUserId, 1, @PHCId)";
            try
            {
                var details = GetUserByUsername(users.Username);
                if(details != null)
                {
                    response.Status = false;
                    response.StatusMessage = "Username already exists";
                    return response;
                }
                string salt = string.Empty;
                UserContext context = new UserContext()
                {
                    DisplayName = users.DisplayName,
                    Email = users.Email,
                    InsertUserId = users.InsertUserId,
                    PasswordHash = CreatePasswordHash(users.Password, ref salt),
                    PasswordSalt = salt,
                    Username = users.Username,
                    PHCId = users.PHCId
                };
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

        public Response Delete(int UserId)
        {
            Response response = new Response();
            string sql1 = "DELETE FROM USERROLES WHERE USERID = @UserId";
            string sql = "DELETE FROM USERS WHERE USERID = @UserId";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    conn.Open();
                    using (var tran = new TransactionScope())
                    {
                        conn.Execute(sql1, new { UserId });
                        conn.Execute(sql, new { UserId });
                        response.Status = true;
                        response.StatusMessage = "Approved and completed successfully";
                        tran.Complete();
                    }
                    conn.Close();
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


        public Response Update(Users context)
        {
            Response response = new Response();
            string sql = @"UPDATE USERS SET USERNAME = @Username, DisplayName = @DisplayName, Email = @Email, PHCId = @PHCId,
                            UpdateUserId = @UpdateUserId, UpdateDate = GetDate(), IsActive = @IsActive WHERE USERID = @UserId";
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

        public Response ChangePassword(int UserId, string NewPassword)
        {
            Response response = new Response();
            string sql = "UPDATE USERS SET PasswordHash = @PasswordHash, PasswordSalt = @PasswordSalt, UpdateUserId = UserId, UpdateDate = GetDate() WHERE USERID = @UserId";
            try
            {
                string PasswordSalt = string.Empty;
                string PasswordHash = CreatePasswordHash(NewPassword, ref PasswordSalt);
                using (IDbConnection conn = GetConnection())
                {
                    conn.Execute(sql, new { UserId, PasswordHash, PasswordSalt });
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

        public LoginResponse Login(string Username, string Password)
        {
            LoginResponse response = new LoginResponse();
            string sql = "SELECT PASSWORDHASH, PASSWORDSALT FROM USERS WHERE LOWER(USERNAME) = LOWER(@Username)";
            try
            {
                using (IDbConnection conn = GetConnection())
                {
                    var result = conn.Query<UserContext>(sql, new { Username }).FirstOrDefault();
                    if(result != null)
                    {
                        bool ok = ComparePassword(Password, result.PasswordHash, result.PasswordSalt);
                        if(ok)
                        {
                            response.Status = true;
                            response.StatusMessage = "Approved and completed successfully";
                            response.details = GetUserByUsernameLogin(Username);
                            return response;
                        }
                    }
                    response.Status = false;
                    response.StatusMessage = "Invalid Username / Password";
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

        private static byte[] CreateSalt()
        {
            //Generate a cryptographic random number.
            int size = 5;
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number.
            return buff;
        }

        private static string CreatePasswordHash(string pwd, ref string mysalt)
        {
            byte[] salt = CreateSalt();
            mysalt = Convert.ToBase64String(salt);
            string hashedPwd = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: pwd,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
                ));
            return hashedPwd;
        }

        private static bool ComparePassword(string plainPassword, string hashedPassword, string mysalt)
        {
            byte[] salt = Convert.FromBase64String(mysalt);
            string hashPwd = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: plainPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
                ));
            if(hashedPassword == hashPwd)
            {
                return true;
            }
            return false;
        }
    }
}
