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
    public class RolesController : ControllerBase
    {
        private RoleRepo _repo;

        public RolesController(IConfiguration Configuration)
        {
            _repo = new RoleRepo(Configuration);
        }

        [HttpGet]
        public ActionResult<List<RoleContext>> Get()
        {
            return _repo.Read();
        }

        [HttpPost]
        public ActionResult<Response> Post([FromBody]RoleContext context)
        {
            return _repo.Create(context);
        }

        [HttpPut]
        public ActionResult<Response> Put([FromBody]RoleContext context)
        {
            return _repo.Update(context);
        }

        [HttpDelete("{RoleId}")]
        public ActionResult<Response> Delete(int RoleId)
        {
            return _repo.Delete(RoleId);
        }
    }
}