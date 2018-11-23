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
    public class PHCController : ControllerBase
    {
        private PHCRepo _repo;

        public PHCController(IConfiguration configuration)
        {
            _repo = new PHCRepo(configuration);
        }

        [HttpGet]
        public ActionResult<List<PHCs>> Get()
        {
            return _repo.Read();
        }

        [HttpPost]
        public ActionResult<Response> Post([FromBody]PHCContext context)
        {
            return _repo.Create(context);
        }

        [HttpPut]
        public ActionResult<Response> Put([FromBody]PHCContext context)
        {
            return _repo.Update(context);
        }

        [HttpDelete("{SettlementId}")]
        public ActionResult<Response> Delete(int PHCId)
        {
            return _repo.Delete(PHCId);
        }
    }
}