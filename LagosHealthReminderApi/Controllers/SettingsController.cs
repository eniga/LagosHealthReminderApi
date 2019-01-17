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
    public class SettingsController : ControllerBase
    {
        public readonly SettingsRepo _repo;

        public SettingsController(IConfiguration Configuration)
        {
            _repo = new SettingsRepo(Configuration);
        }

        // GET: api/Settings
        [HttpGet]
        public List<Settings> Get()
        {
            return _repo.List();
        }

        // GET: api/Settings/5
        [HttpGet("{SettingsId}")]
        public Settings Get(int SettingsId)
        {
            return _repo.Get(SettingsId);
        }

        // POST: api/Settings
        [HttpPost]
        public Response Post([FromBody] Settings context)
        {
            return _repo.Add(context);
        }

        // PUT: api/Settings/5
        [HttpPut]
        public Response Put([FromBody] Settings context)
        {
            return _repo.Update(context);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{SettingsId}")]
        public Response Delete(int SettingsId)
        {
            return _repo.Delete(SettingsId);
        }
    }
}
