using ClinicProject1.Models.DTOs.MedicalRecordDTOs;
using ClinicProject1.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicProject1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalRecordsController : ControllerBase
    {
        private readonly IMedicalRecordService _medicalRecordService;

        public MedicalRecordsController(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<PatientMedicalHistoryDto>> GetPatientMedicalHistory(int patientId)
        {
            var history = await _medicalRecordService.GetPatientMedicalHistory(patientId);
            if (history == null)
            {
                return NotFound();
            }
            return Ok(history);
        }
    }
}
