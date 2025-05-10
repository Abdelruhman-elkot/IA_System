using ClinicProject1.Data;
using ClinicProject1.Models.Entities;
using ClinicProject1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicProject1.Repositories.Implementations
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private readonly ClinicDbContext _context;

        public MedicalRecordRepository(ClinicDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByPatientId(int patientId)
        {
            return await _context.MedicalRecords
                .Include(mr => mr.Doctor)
                .ThenInclude(d => d.User)
                .Where(mr => mr.PatientId == patientId)
                .OrderByDescending(mr => mr.RecordId)
                .ToListAsync();
        }

        public async Task<Patient> GetPatientWithDetails(int patientId)
        {
            return await _context.Patients
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.PatientId == patientId);
        }
    }
}
