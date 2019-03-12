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
    public class StatesController : ControllerBase
    {
        StateRepo repo;

        public StatesController(IConfiguration configuration)
        {
            repo = new StateRepo(configuration);
        }

        [HttpGet]
        public List<StateContext> GetAll()
        {
            return repo.ReadAll();
        }

        [HttpGet("{StateId}")]
        public StateContext Get(int StateId)
        {
            return repo.Read(StateId);
        }

        [HttpPost]
        public Response Post([FromBody] StateContext context)
        {
            return repo.Create(context);
        }

        [HttpPut]
        public Response Put([FromBody] StateContext context)
        {
            return repo.Update(context);
        }

        [HttpDelete("{StateId}")]
        public Response Delete(int StateId)
        {
            return repo.Delete(StateId);
        }
    }
}