using ClinicProject1.Data;
using ClinicProject1.Models.Entities;
using ClinicProject1.Models.Enums;
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
        [Route("viewPatientsData/{doctorId}")]
        public async Task<ActionResult> viewPatientsData(int doctorId)
        {
            var doctorExists = await _context.Doctors.AnyAsync(d => d.DoctorId == doctorId);
            if (!doctorExists)
            {
                return NotFound("Doctor not found");
            }

            var patients = await _context.Appointments
                .Where(p => p.DoctorId == doctorId && p.Status == Models.Enums.AppointmentStatus.Approved) 
                .Select(p => new { p.PatientId, p.Patient.User.Username, p.Patient.MedicalComplaint })
                .ToListAsync();

            return Ok(patients);
        }


        [HttpPatch("updatePrescription/{doctorId}/{patientId}")]
        public async Task<ActionResult> updatePrescription(int patientId, int doctorId ,[FromBody] string prescriptionData)
        {
            var doctorUsername = User.FindFirst(ClaimTypes.Name)?.Value;
            var doctor = await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.DoctorId == doctorId);

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
        //[Authorize(Roles = "Doctor")]
        public async Task<ActionResult> getDoctorsBySpeciality(Specialization speciality)
        {
            var doctors = await _context.Doctors
                .Where(d => d.Specialization == speciality)
                .Include(d => d.Availabilities)
                .Select(d => new { d.User.Username, d.DoctorId, d.Availabilities })
                .ToListAsync();
            if (doctors.Count == 0)
            {
                return NotFound("No doctors found with the specified speciality");
            }
            return Ok(doctors);
        }
    }
}
