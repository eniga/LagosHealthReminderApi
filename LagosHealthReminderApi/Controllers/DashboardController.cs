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
    public class DashboardController : ControllerBase
    {
        public readonly DashboardRepo _repo;

        public DashboardController(IConfiguration Configuration)
        {
            _repo = new DashboardRepo(Configuration);
        }

        [HttpGet]
        public ActionResult<Dashboard> Get()
        {
            return _repo.Get();
        }
    }
}