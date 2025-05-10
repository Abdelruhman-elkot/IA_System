using ClinicProject1.Models.DTOs.AppointmentDtos;
using ClinicProject1.Models.Enums;
using ClinicProject1.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicProject1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<ActionResult<AppointmentDashboardDto>> GetAllAppointments()
        {
            var appointments = await _appointmentService.GetAllAppointments();
            return Ok(appointments);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<AppointmentDashboardDto>>> GetAppointmentsByPatientId(int patientId)
        {
            var appointments = await _appointmentService.GetAppointmentsByPatientId(patientId);
            return Ok(appointments);
        }

        [HttpGet("doctor/{doctorId}")]
        public async Task<ActionResult<IEnumerable<AppointmentDashboardDto>>> GetAppointmentsByDoctorId(int doctorId)
        {
            var appointments = await _appointmentService.GetAppointmentsByDoctorId(doctorId);
            return Ok(appointments);
        }


        [HttpPut("{appointmentId}/approve")]
        public async Task<IActionResult> ApproveAppointment(int appointmentId)
        {
            try
            {
                await _appointmentService.ApproveAppointment(appointmentId);
                var appointment = await _appointmentService.GetAppointmentById(appointmentId);
                return Ok(appointment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{appointmentId}/cancel")]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            try
            {
                await _appointmentService.CancelAppointment(appointmentId);
                var appointment = await _appointmentService.GetAppointmentById(appointmentId);
                return Ok(appointment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{appointmentId}")]
        public async Task<ActionResult<AppointmentDashboardDto>> GetAppointmentById(int appointmentId)
        {
            try
            {
                var appointment = await _appointmentService.GetAppointmentById(appointmentId);
                return Ok(appointment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("book")]
        public async Task<ActionResult<AppointmentDashboardDto>> BookAppointment([FromBody] CreateAppointmentDto appointmentDto)
        {
            try
            {
                var appointment = await _appointmentService.BookAppointment(appointmentDto);
                return CreatedAtAction(nameof(GetAppointmentById), new { appointmentId = appointment.AppointmentId }, appointment);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{appointmentId}/reschedule")]
        public async Task<ActionResult<AppointmentDashboardDto>> RescheduleAppointment(
    int appointmentId, [FromBody] RescheduleAppointmentDto rescheduleDto)
        {
            try
            {
                var appointment = await _appointmentService.RescheduleAppointment(appointmentId, rescheduleDto);
                return Ok(appointment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpGet("available-slots/{doctorId}/{day}")]
        //public async Task<ActionResult<List<string>>> GetAvailableTimeSlots(int doctorId, WorkDays day)
        //{
        //    try
        //    {
        //        var timeSlots = await _appointmentService.GetAvailableTimeSlots(doctorId, day);
        //        return Ok(timeSlots.Select(t => t.ToString()).ToList());
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //}
    }
}
