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
    public class SMSController : ControllerBase
    {
        public readonly MessengerRepo _repo;

        public SMSController(IConfiguration Configuration)
        {
            _repo = new MessengerRepo(Configuration);
        }

        [HttpGet("balance")]
        public GetBalanceResult GetBalance()
        {
            return _repo.GetSMSBalance();
        }

        [HttpGet("reminder")]
        public Response SendReminder()
        {
            return _repo.SendReminder();
        }
    }
}