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
        public ActionResult<List<AppointmentResponse>> Get()
        {
            return _repo.GetAll();
        }

        [HttpGet("{AppointmentId}")]
        public ActionResult<Appointments> Get(int AppointmentId)
        {
            return _repo.GetAppointment(AppointmentId);
        }

        [HttpGet("Patient/{PatientId}")]
        public ActionResult<List<PatientAppointmentResponse>> GetBYPatienId(int PatientId)
        {
            return _repo.GePatientAppointments(PatientId);
        }

        [HttpGet("Pending")]
        public ActionResult<List<AppointmentResponse>> GetPending()
        {
            return _repo.GetPending();
        }

        [HttpGet("Pending/{PHCId}")]
        public ActionResult<List<AppointmentResponse>> GetPending(int PHCId)
        {
            return _repo.GetPending(PHCId);
        }

        [HttpGet("Confirmed")]
        public ActionResult<List<Appointments>> GetConfirmed()
        {
            return _repo.GetConfirmed();
        }

        [HttpGet("Defaulters")]
        public ActionResult<List<AppointmentResponse>> GetDefaulters()
        {
            return _repo.GetDefaulters();
        }

        [HttpGet("Defaulters/{PHCId}")]
        public ActionResult<List<AppointmentResponse>> GetDefaulters(int PHCId)
        {
            return _repo.GetDefaulters(PHCId);
        }

        [HttpPost]
        public ActionResult<Response> Post([FromBody]AppointmentRequest appointments)
        {
            return _repo.CreateAppointment(appointments);
        }

        [HttpPost("Confirm")]
        public ActionResult<ConfirmAppointmentResponse> ConfirmAppointment([FromBody] ConfirmAppointmentRequest context)
        {
            return _repo.ConfirmAppointment(context);
        }

        [HttpPost("Defaulters/Confirm")]
        public ActionResult<Response> ConfirmDefaulter([FromBody] ConfirmAppointmentRequest context)
        {
            return _repo.ConfirmDefaulterCall(context);
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

        [HttpGet("Randomization")]
        public ActionResult SetRandomization()
        {
            try
            {
                _repo.Randomization();
                return StatusCode(200);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            
        }
    }
}