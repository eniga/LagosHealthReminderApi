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
    public class MessagesController : ControllerBase
    {
        private MessagesRepo repo;

        public MessagesController(IConfiguration Configuration)
        {
            repo = new MessagesRepo(Configuration);
        }

        [HttpPost]
        public MessageResponse SendMessage([FromBody]MessageRequest request)
        {
            return repo.SendMessage(request);
        }

        [HttpPost("Campaign")]
        public Response SendCampaign([FromBody]CampaignMessage request)
        {
            return repo.SendCampaign(request);
        }

        [HttpGet("Campaign")]
        public List<CampaignMessage> GetCampaignMessages()
        {
            return repo.GetCampaignMessages();
        }

        //[HttpGet("Test")]
        //public Response TestMessage()
        //{
        //    return repo.GetBalance();
        //}
    }
}