using ClinicProject1.DTOs.AvailabilityDTOs;
using ClinicProject1.Services.Interfaces;
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

        [HttpPost]
        public async Task<IActionResult> AssignAvailability(
            int doctorId, [FromBody] AssignAvailabilityDto dto)
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
