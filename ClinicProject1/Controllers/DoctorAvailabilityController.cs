using ClinicProject1.Models.DTOs.AvailabilityDTOs;
using ClinicProject1.Models.Enums;
using ClinicProject1.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicProject1.Controllers
{
    [Route("api/Admin/doctors/{doctorId}/availability")]
    [ApiController]
    public class DoctorAvailabilityController : ControllerBase
    {
        private readonly IDoctorAvailabilityService _availabilityService;

        public DoctorAvailabilityController(IDoctorAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }

        [HttpGet]
        public async Task<ActionResult<DoctorAvailabilityDto>> GetAvailability(int doctorId)
        {
            try
            {
                var availability = await _availabilityService.GetAvailability(doctorId);
                return availability == null ? NotFound() : Ok(availability);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("timeslots/{day}")]
        public async Task<ActionResult<List<string>>> GetAvailableTimeSlots(int doctorId, WorkDays day)
        {
            try
            {
                var availability = await _availabilityService.GetAvailability(doctorId);
                if (availability == null)
                    return NotFound("Doctor availability not found");

                if (!availability.AvailableDays.Contains(day.ToString()))
                    return BadRequest("The selected day is not available for this doctor");

                return Ok(availability.AvailableTimeSlots[day.ToString()]);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AssignAvailability(int doctorId, [FromBody] AssignAvailabilityDto dto)
        {
            try
            {
                var result = await _availabilityService.AssignAvailability(doctorId, dto);
                if (!result) return BadRequest("Failed to assign availability");

                var availability = await _availabilityService.GetAvailability(doctorId);
                return Ok(availability);
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
    }
}
