using Microsoft.AspNetCore.Mvc;
using ClinicProject1.DTOs.Doctor_DTOs;
using ClinicProject1.Services.Interfaces;
using ClinicProject1.DTOs.DoctorDTOs;
using ClinicProject1.Models.Entities;

namespace ClinicProject1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        #region Doctor Management
        [HttpGet("doctors")]
        public async Task<ActionResult<IEnumerable<DoctorDashboardDto>>> GetAllDoctors()
        {
            var doctors = await _adminService.GetAllDoctors();
            return Ok(doctors);
        }

        [HttpGet("doctors/{doctorId:int}")]
        public async Task<ActionResult<DoctorDashboardDto>> GetDoctorById(int doctorId)
        {
            var doctor = await _adminService.GetDoctorById(doctorId);
            if (doctor == null) return NotFound();
            return Ok(doctor);
        }

        [HttpPost("doctors")]
        public async Task<ActionResult<DoctorDashboardDto>> CreateDoctor([FromBody] CreateDoctorDto doctorDto)
        {
            var doctor = await _adminService.CreateDoctor(doctorDto);
            return CreatedAtAction(nameof(GetDoctorById), new { doctorId = doctor.DoctorId }, doctor);
        }

        [HttpPut("doctors/{doctorId:int}")]
        public async Task<IActionResult> UpdateDoctor(int doctorId, [FromBody] UpdateDoctorDto doctorDto)
        {
            var result = await _adminService.UpdateDoctor(doctorId, doctorDto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("doctors/{doctorId:int}")]
        public async Task<IActionResult> DeleteDoctor(int doctorId)
        {
            var result = await _adminService.DeleteDoctor(doctorId);
            if (!result) return NotFound();
            return NoContent();
        }
        #endregion
    }
}
