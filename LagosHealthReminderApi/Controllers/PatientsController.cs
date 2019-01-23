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
    public class PatientsController : ControllerBase
    {
        private PatientsRepo _repo;
        public PatientsController(IConfiguration configuration)
        {
            _repo = new PatientsRepo(configuration);
        }

        [HttpGet]
        public ActionResult<List<Patients>> Get()
        {
            return _repo.ReadAll();
        }

        [HttpGet("{PatientId}")]
        public ActionResult<Patients> Get(int PatientId)
        {
            return _repo.Read(PatientId);
        }

        [HttpGet("Name/{Name}")]
        public ActionResult<List<Patients>> GetByName(string Name)
        {
            return new List<Patients>();
        }

        [HttpGet("Phone/{PhoneNumber}")]
        public ActionResult<List<Patients>> GetByPhone(string PhoneNumber)
        {
            return _repo.ReadByPhone(PhoneNumber);
        }

        [HttpGet("QrCode/{QrCode}")]
        public ActionResult<Patients> GetByQrCode(string QrCode)
        {
            return _repo.ReadQrCode(QrCode);
        }

        [HttpPost]
        public ActionResult<CreatePatientResponse> Post([FromBody]PatientContext patients)
        {
            return _repo.Create(patients);
        }

        [HttpPut]
        public ActionResult<Response> Put([FromBody]PatientContext patients)
        {
            return _repo.Update(patients);
        }

        [HttpDelete("{PatientId}")]
        public ActionResult<Response> Delete(int PatientId)
        {
            return _repo.Delete(PatientId);
        }

        [HttpGet("Appointments/{PatientId}")]
        public ActionResult<List<AppointmentResponse>> GetAppointments(int PatientId)
        {
            return _repo.GetAppointments(PatientId);
        }

        [HttpGet("Appointments/Pending")]
        public ActionResult<List<Appointments>> GetPending()
        {
            return new List<Appointments>();
        }

        [HttpPost("QRCode")]
        public ActionResult<Response> PostQR([FromBody]Patients patients)
        {
            return new Response();
        }

        [HttpPut("QRCode/{QRCode}/{PatientId}")]
        public ActionResult<Response> UpdateQR(string QRCode, int PatientId)
        {
            return _repo.UpdateQrCode(PatientId, QRCode);
        }
    }
}