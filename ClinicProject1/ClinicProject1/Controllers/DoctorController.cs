using ClinicProject1.Data;
using ClinicProject1.Data.Enums;
using ClinicProject1.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ClinicProject1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly ClinicDbContext _context;
        public DoctorController(ClinicDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Doctor")]
        [Route("viewPatientsData")]
        public async Task<ActionResult> viewPatientsData()
        {
            var patients = await _context.Patients
                .Select(p => new { p.User.Username, p.MedicalComplaint })
                .ToListAsync();
            return Ok(patients);
        }

        [HttpPatch("updatePrescription/{patientId}")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult> updatePrescription(int patientId, [FromBody] string prescriptionData)
        {
            var doctorUsername = User.FindFirst(ClaimTypes.Name)?.Value;
            var doctor = await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.User.Username == doctorUsername);

            if (doctor == null)
            {
                return Unauthorized("Doctor not found");
            }

            var patient = await _context.Patients
                .Include(p => p.MedicalRecords)
                .FirstOrDefaultAsync(p => p.PatientId == patientId);

            if (patient == null)
            {
                return NotFound("Patient not found");
            }

            var medicalRecord = patient.MedicalRecords
                .OrderByDescending(mr => mr.RecordId)
                .FirstOrDefault();

            if (medicalRecord == null)
            {
                medicalRecord = new MedicalRecord
                {
                    PatientId = patientId,
                    DoctorId = doctor.DoctorId,
                    Diagnosis = "",
                    Prescription = prescriptionData
                };
                _context.MedicalRecords.Add(medicalRecord);
            }
            else
            {
                medicalRecord.Prescription = prescriptionData;
            }

            await _context.SaveChangesAsync();
            return Ok(medicalRecord.Prescription);
        }

        [HttpGet("getDoctorsBySpeciality/{speciality}")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult> getDoctorsBySpeciality(Specialization speciality)
        {
            var doctors = await _context.Doctors
                .Where(d => d.Specialization == speciality)
                .Include(d => d.Availabilities)
                // patient.User is null unless lazy loading is enabled or explicitly " included "
                .Select(d => new { d.User.Username, d.DoctorId , d.Availabilities})
                .ToListAsync();
            if (doctors.Count == 0)
            {
                return NotFound("No doctors found with the specified speciality");
            }
            return Ok(doctors);
        }
    }
}
