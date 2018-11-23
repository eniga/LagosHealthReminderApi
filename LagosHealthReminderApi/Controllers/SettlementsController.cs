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
    public class SettlementsController : ControllerBase
    {
        private SettlementRepo _repo;

        public SettlementsController(IConfiguration configuration)
        {
            _repo = new SettlementRepo(configuration);
        }

        [HttpGet]
        public ActionResult<List<Settlements>> Get()
        {
            return _repo.Read();
        }

        [HttpPost]
        public ActionResult<Response> Post([FromBody]SettlementContext context)
        {
            return _repo.Create(context);
        }

        [HttpPut]
        public ActionResult<Response> Put([FromBody]SettlementContext context)
        {
            return _repo.Update(context);
        }

        [HttpDelete("{SettlementId}")]
        public ActionResult<Response> Delete(int SettlementId)
        {
            return _repo.Delete(SettlementId);
        }
    }
}