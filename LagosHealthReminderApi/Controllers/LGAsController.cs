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
    public class LGAsController : ControllerBase
    {
        private LGARepo _repo;

        public LGAsController(IConfiguration Configuration)
        {
            _repo = new LGARepo(Configuration);
        }

        [HttpGet]
        public List<LGAS> Get()
        {
            return _repo.Read();
        }

        [HttpPost]
        public Response Post([FromBody]LGAContext context)
        {
            return _repo.Create(context);
        }

        [HttpPut]
        public Response Put([FromBody]LGAContext context)
        {
            return _repo.Update(context);
        }

        [HttpDelete("{LGAId}")]
        public Response Delete(int LGAId)
        {
            return _repo.Delete(LGAId);
        }
    }
}