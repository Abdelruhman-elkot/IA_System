using ClinicProject1.Data;
using ClinicProject1.Models.Entities;
using ClinicProject1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ClinicProject1.Models.Enums;

namespace ClinicProject1.Repositories.Implementations
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ClinicDbContext _context;

        public AppointmentRepository(ClinicDbContext context)
        {
            _context = context;
        }

        public async Task<Appointment> GetAppointmentById(int appointmentId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .ThenInclude(p => p.User)
                .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointments()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .ThenInclude(p => p.User)
                .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientId(int patientId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .ThenInclude(p => p.User)
                .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorId(int doctorId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                    .ThenInclude(p => p.User)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.User)
                .Where(a => a.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<bool> AddAppointment(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAppointment(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAppointment(int appointmentId)
        {
            var appointment = await GetAppointmentById(appointmentId);
            if (appointment == null) return false;

            _context.Appointments.Remove(appointment);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsTimeSlotAvailable(int doctorId, WorkDays day, AppointmentTimes time)
        {
            var existingAppointments = await _context.Appointments
                .Where(a => a.DoctorId == doctorId &&
                       a.AppointmentDay == day &&
                       a.Time == time &&
                       a.Status != AppointmentStatus.Canceled)
                .ToListAsync();

            return !existingAppointments.Any();
        }
    }
}
