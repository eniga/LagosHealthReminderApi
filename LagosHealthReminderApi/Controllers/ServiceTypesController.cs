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
    public class ServiceTypesController : ControllerBase
    {
        public readonly ServiceTypesRepo _repo;

        public ServiceTypesController(IConfiguration Configuration)
        {
            _repo = new ServiceTypesRepo(Configuration);
        }

        [HttpGet]
        public ActionResult<List<ServiceTypes>> Get()
        {
            return _repo.Read();
        }

        [HttpPost]
        public ActionResult<Response> Post([FromBody] ServiceTypeContext serviceTypes)
        {
            return _repo.Create(serviceTypes);
        }

        [HttpPut]
        public ActionResult<Response> Put([FromBody] ServiceTypeContext serviceTypes)
        {
            return _repo.Update(serviceTypes);
        }

        [HttpDelete("{ServiceTypeId}")]
        public ActionResult<Response> Delete(int ServiceTypeId)
        {
            return _repo.Delete(ServiceTypeId);
        }

        [HttpPut("smsmessage")]
        public ActionResult<Response> PutMessage([FromBody] ServiceTypeContext serviceTypes)
        {
            return _repo.UpdateMessage(serviceTypes);
        }
    }
}