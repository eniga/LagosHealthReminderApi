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
    public class NextOfKinsController : ControllerBase
    {
        private NextOfKinRepo _repo;

        public NextOfKinsController(IConfiguration Configuration)
        {
            _repo = new NextOfKinRepo(Configuration);
        }

        [HttpGet("{PatientId}")]
        public ActionResult<List<NextOfKins>> Get(int PatientId)
        {
            return _repo.Read(PatientId);
        }

        [HttpPost]
        public ActionResult<Response> Post([FromBody] NextOfKinContext context)
        {
            return _repo.Create(context);
        }

        [HttpPut]
        public ActionResult<Response> Put([FromBody] NextOfKinContext context)
        {
            return _repo.Update(context);
        }

        [HttpDelete("{NextOfKinId}")]
        public ActionResult<Response> Delete(int NextOfKinId)
        {
            return _repo.Delete(NextOfKinId);
        }
    }
}