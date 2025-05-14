using ClinicProject1.Data;
using ClinicProject1.Models.Entities;
using ClinicProject1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicProject1.Repositories.Implementations
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ClinicDbContext _context;

        public AdminRepository(ClinicDbContext context)
        {
            _context = context;
        }

        #region Doctor CRUD Operations
        public async Task<IEnumerable<Doctor>> GetAllDoctors()
        {
            return await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.Availabilities)
                .ToListAsync();
        }


        public async Task<Doctor> GetDoctorById(int doctorId)
        {
            return await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.Availabilities)
                .FirstOrDefaultAsync(d => d.DoctorId == doctorId);
        }


        public async Task<Doctor> CreateDoctor(Doctor doctor)
        {
            if (doctor.User != null)
            {
                _context.Users.Add(doctor.User);
                await _context.SaveChangesAsync();
                doctor.DoctorId = doctor.User.UserId;
            }

            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();
            return doctor;
        }


        public async Task<bool> UpdateDoctor(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<bool> DeleteDoctor(int doctorId)
        {
            var doctor = await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.DoctorId == doctorId);

            if (doctor == null) return false;

            if (await _context.Appointments.AnyAsync(a => a.DoctorId == doctorId))
                return false;

            if (await _context.MedicalRecords.AnyAsync(m => m.DoctorId == doctorId))
                return false;

            _context.Doctors.Remove(doctor);
            if (doctor.User != null)
            {
                _context.Users.Remove(doctor.User);
            }

            return await _context.SaveChangesAsync() > 0;
        }
        #endregion

        #region Reports
        public async Task<IEnumerable<Doctor>> GetDoctorSchedulesReport()
        {
            return await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.Availabilities)
                .Include(d => d.Appointments)
                .ToListAsync();
        }

        public async Task<IEnumerable<Patient>> GetPatientVisitsReport()
        {
            return await _context.Patients
                .Include(p => p.User)
                .Include(p => p.Appointments.Where(a => a.Status == Models.Enums.AppointmentStatus.Approved))
                .Include(p => p.MedicalRecords)
                .Where(p => p.Appointments.Any(a => a.Status == Models.Enums.AppointmentStatus.Approved))
                .ToListAsync();
        }
        #endregion
    }
}
