using Microsoft.AspNetCore.Mvc;
using ClinicProject1.Services.Interfaces;
using ClinicProject1.Models.DTOs.DoctorDTOs;
using ClinicProject1.Models.DTOs.ReportsDTOs;
using Microsoft.AspNetCore.Authorization;

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
            var updatedDoctor = await _adminService.GetDoctorById(doctorId);
            return Ok(updatedDoctor);
        }

        [HttpDelete("doctors/{doctorId:int}")]
        public async Task<IActionResult> DeleteDoctor(int doctorId)
        {
            var result = await _adminService.DeleteDoctor(doctorId);
            if (!result) return NotFound();
            var doctors = await _adminService.GetAllDoctors();
            return Ok(doctors);
        }
        #endregion


        #region Reports
        [HttpGet("reports/doctor-schedules")]
        public async Task<ActionResult<IEnumerable<DoctorScheduleReportDto>>> GetDoctorSchedulesReport()
        {
            var report = await _adminService.GenerateDoctorSchedulesReport();
            return Ok(report);
        }

        [HttpGet("reports/patient-visits")]
        public async Task<ActionResult<IEnumerable<PatientVisitReportDto>>> GetPatientVisitsReport()
        {
            var report = await _adminService.GeneratePatientVisitsReport();
            return Ok(report);
        }
        #endregion
    }
}
