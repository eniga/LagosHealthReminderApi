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
    public class ReportsController : ControllerBase
    {
        ReportsRepo repo;

        public ReportsController(IConfiguration configuration)
        {
            repo = new ReportsRepo(configuration);
        }

        [HttpGet("appointments")]
        public List<AppointmentReport> GetAppointments()
        {
            return repo.GetAppointments();
        }

        [HttpGet("appointments/{StartDate}/{EndDate}")]
        public List<AppointmentReport> GetAppointmentsRange(DateTime StartDate, DateTime EndDate)
        {
            return repo.GetAppointmentsRange(StartDate, EndDate);
        }
    }
}