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
    public class AppointmentsController : ControllerBase
    {
        public readonly AppointmentsRepo _repo;

        public AppointmentsController(IConfiguration Configuration)
        {
            _repo = new AppointmentsRepo(Configuration);
        }

        [HttpGet]
        public ActionResult<List<Appointments>> Get()
        {
            return _repo.GetAll();
        }

        [HttpGet("{AppointmentId}")]
        public ActionResult<Appointments> Get(int AppointmentId)
        {
            return _repo.GetAppointment(AppointmentId);
        }

        [HttpGet("Patient/{PatientId}")]
        public ActionResult<List<Appointments>> GetBYPatienId(int PatientId)
        {
            return _repo.GePatientAppointments(PatientId);
        }

        [HttpGet("Pending")]
        public ActionResult<List<Appointments>> GetPending()
        {
            return _repo.GetPending();
        }

        [HttpGet("Confirmed")]
        public ActionResult<List<Appointments>> GetConfirmed()
        {
            return _repo.GetConfirmed();
        }

        [HttpGet("Defaulters")]
        public ActionResult<List<Appointments>> GetDefaulters()
        {
            return _repo.GetDefaulters();
        }

        [HttpPost]
        public ActionResult<Response> Post([FromBody]AppointmentsContext appointments)
        {
            return _repo.CreateAppointment(appointments);
        }

        [HttpPut]
        public ActionResult<Response> Put([FromBody]AppointmentsContext appointments)
        {
            return _repo.UpdateAppointment(appointments);
        }

        [HttpDelete("{AppointmentId}")]
        public ActionResult<Response> Delete(int AppointmentId)
        {
            return _repo.DeleteAppointment(AppointmentId);
        }
    }
}