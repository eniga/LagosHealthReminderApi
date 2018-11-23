using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LagosHealthReminderApi.Models;
using LagosHealthReminderApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LagosHealthReminderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CentresController : ControllerBase
    {
        private readonly CentresRepo _repo;

        public CentresController(IConfiguration Configuration)
        {
            _repo = new CentresRepo(Configuration);
        }

        [HttpGet]
        public ActionResult<List<Centres>> Get()
        {
            return _repo.GetAllCentres();
        }

        [HttpGet("{CentreId}")]
        public ActionResult<Centres> Get(int CentreId)
        {
            return _repo.GetCentre(CentreId);
        }

        [HttpPost]
        public ActionResult<Response> Post([FromBody]Centres centres)
        {
            return _repo.CreateCentre(centres);
        }

        [HttpPut]
        public ActionResult<Response> Put([FromBody]Centres centres)
        {
            return _repo.UpdateCentre(centres);
        }

        [HttpDelete("{CentreId}")]
        public ActionResult<Response> Delete(int CentreId)
        {
            return _repo.DeleteCentre(CentreId);
        }
    }
}