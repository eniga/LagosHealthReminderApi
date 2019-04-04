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
        public readonly MessengerRepo messengerRepo;

        public DashboardController(IConfiguration Configuration)
        {
            _repo = new DashboardRepo(Configuration);
            messengerRepo = new MessengerRepo(Configuration);
        }

        [HttpGet("sms")]
        public ActionResult<SMSDetails> GetSMSDetails()
        {
            return messengerRepo.GetSMSDetails();
        }

        [HttpGet("patients/total")]
        public ActionResult<int> GetTotalPatients()
        {
            return _repo.GetTotalPatients();
        }


        [HttpGet("appointments/total")]
        public ActionResult<int> GetTotalAppointments()
        {
            return _repo.GetTotalAppointments();
        }


        [HttpGet("defaulters/total")]
        public ActionResult<int> GetTotalDefaulters()
        {
            return _repo.GetTotalDefaulters();
        }

        [HttpGet("appointments/today/total")]
        public ActionResult<int> GetTotalTodayAppointments()
        {
            return _repo.GetTotalTodayAppointments();
        }


        [HttpGet("settlements/total")]
        public ActionResult<int> GetTotalSettlements()
        {
            return _repo.GetTotalSettlements();
        }

        [HttpGet("wards/total")]
        public ActionResult<int> GetTotalWards()
        {
            return _repo.GetTotalWards();
        }

        [HttpGet("lgas/total")]
        public ActionResult<int> GetTotalLGAs()
        {
            return _repo.GetTotalLGAs();
        }

        [HttpGet("phcs/total")]
        public ActionResult<int> GetTotalPHCs()
        {
            return _repo.GetTotalPHCs();
        }

        [HttpGet("users/total")]
        public ActionResult<int> GetTotalUsers()
        {
            return _repo.GetTotalUsers();
        }

        [HttpGet("services/total")]
        public ActionResult<int> GetTotalServices()
        {
            return _repo.GetTotalServices();
        }

        [HttpGet("randomization")]
        public ActionResult<int> GetRandomization()
        {
            return _repo.GetRandomization();
        }

        [HttpGet("patients")]
        public Dashboard GetClientDashboard()
        {
            return _repo.GetClientDashboard();
        }

        [HttpGet("appointments")]
        public Dashboard GetAppointmentsDashboard()
        {
            return _repo.GetAppointmentsDashboard();
        }

        [HttpGet("patients/onschedule")]
        public Dashboard GetClientsOnScheduledDashboard()
        {
            return _repo.GetClientsOnScheduledDashboard();
        }

        [HttpGet("defaulters")]
        public Dashboard GetDefaultersDashboard()
        {
            return _repo.GetDefaultersDashboard();
        }

        [HttpGet("defaulters/returned")]
        public Dashboard GetDefaultersReturnedDashboard()
        {
            return _repo.GetDefaultersReturnedDashboard();
        }

        [HttpGet("phcs/active")]
        public Dashboard GetActivePHCsDashboard()
        {
            return _repo.GetActivePHCsDashboard();
        }

        [HttpGet("services/active/list")]
        public List<string> GetActiveServices()
        {
            return _repo.GetActiveServices();
        }
    }
}