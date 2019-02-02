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
    public class WardsController : ControllerBase
    {
        private WardRepo _repo;

        public WardsController(IConfiguration configuration)
        {
            _repo = new WardRepo(configuration);
        }

        [HttpGet]
        public ActionResult<List<Wards>> GetAll()
        {
            return _repo.ReadAll();
        }

        [HttpGet("{LGAId}")]
        public ActionResult<List<Wards>> Get(int LGAId)
        {
            return _repo.Read(LGAId);
        }

        [HttpPost]
        public ActionResult<Response> Post([FromBody]WardContext context)
        {
            return _repo.Create(context);
        }

        [HttpPut]
        public ActionResult<Response> Put([FromBody]WardContext context)
        {
            return _repo.Update(context);
        }

        [HttpDelete("{WardId}")]
        public ActionResult<Response> Delete(int WardId)
        {
            return _repo.Delete(WardId);
        }
    }
}