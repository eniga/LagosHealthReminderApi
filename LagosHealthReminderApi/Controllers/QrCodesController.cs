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
    public class QrCodesController : ControllerBase
    {
        private QrCodeRepo _repo;

        public QrCodesController(IConfiguration Configuration)
        {
            _repo = new QrCodeRepo(Configuration);
        }

        [HttpGet]
        public ActionResult<List<QrCodes>> Get()
        {
            return _repo.Read();
        }

        [HttpPost]
        public ActionResult<Response> GenerateCodes(QrCodesRequest request)
        {
            return _repo.Create(request);
        }

        [HttpGet("Unprinted")]
        public ActionResult<List<QrCodes>> Unprinted()
        {
            return _repo.Unprinted();
        }

        [HttpPut("Printed")]
        public ActionResult<Response> Printed()
        {
            return _repo.Printed();
        }

    }
}