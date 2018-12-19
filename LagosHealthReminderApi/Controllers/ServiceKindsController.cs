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
    public class ServiceKindsController : ControllerBase
    {
        public readonly ServiceKindsRepo _repo;

        public ServiceKindsController(IConfiguration Configuration)
        {
            _repo = new ServiceKindsRepo(Configuration);
        }


        [HttpGet("{ServiceTypeId}")]
        public ActionResult<List<ServiceKinds>> Get(int ServiceTypeId)
        {
            return _repo.Read(ServiceTypeId);
        }

        [HttpPost]
        public ActionResult<Response> Post([FromBody] ServiceKindContext kinds)
        {
            return _repo.Create(kinds);
        }

        [HttpPut]
        public ActionResult<Response> Put(int ServiceKindId, [FromBody] ServiceKindContext kinds)
        {
            return _repo.Update(kinds);
        }

        [HttpDelete("{ServiceKindId}")]
        public ActionResult<Response> Delete(int ServiceKindId)
        {
            return _repo.Delete(ServiceKindId);
        }

        [HttpGet("types")]
        public ActionResult<List<Types>> ReadTypes()
        {
            return _repo.ReadTypes();
        }
    }
}