using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LagosHealthReminderApi.DbContext;
using LagosHealthReminderApi.Models;
using LagosHealthReminderApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LagosHealthReminderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private UsersRepo _repo;
        private UserRoleRepo _userRoleRepo;

        public UsersController(IConfiguration configuration)
        {
            _repo = new UsersRepo(configuration.GetConnectionString("DefaultConnection"));
            _userRoleRepo = new UserRoleRepo(configuration);
        }


        [HttpGet]
        public ActionResult<List<Users>> Get()
        {
            return _repo.Read();
        }

        [HttpGet("{UserId}")]
        public ActionResult<Users> Get(int UserId)
        {
            return _repo.GetUser(UserId);
        }

        [HttpGet("Username/{Username}")]
        public ActionResult<Users> GetUserByUsername(string Username)
        {
            return _repo.GetUserByUsername(Username);
        }

        [HttpPost]
        public ActionResult<Response> Post([FromBody]Users users)
        {
            return _repo.Create(users);
        }

        [HttpPut]
        public ActionResult<Response> Put([FromBody]Users users)
        {
            return _repo.Update(users);
        }

        [HttpDelete("{UserId}")]
        public ActionResult<Response> Delete(int UserId)
        {
            return _repo.Delete(UserId);
        }

        [HttpPost("Login")]
        public ActionResult<Response> Login([FromBody]UserLogin user)
        {
            return _repo.Login(user.Username, user.Password);
        }

        [HttpPost("ForgotPassword")]
        public ActionResult<Response> ForgotPassword([FromBody] string Username)
        {
            return new Response();
        }

        [HttpPut("UpdatePassword")]
        public ActionResult<Response> UpdatePassword([FromBody] Users users)
        {
            return _repo.ChangePassword(users.UserId, users.Password);
        }

        [HttpPost("Role")]
        public ActionResult<Response> AssignRole([FromBody] UserRoleContext context)
        {
            return _userRoleRepo.Create(context);
        }

        [HttpPut("Role")]
        public ActionResult<Response> UpdateRole([FromBody] UserRoleContext context)
        {
            return _userRoleRepo.Update(context);
        }

    }
}