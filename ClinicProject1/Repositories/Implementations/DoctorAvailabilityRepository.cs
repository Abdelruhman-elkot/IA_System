using ClinicProject1.Data;
using ClinicProject1.Models.Entities;
using ClinicProject1.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicProject1.Repositories.Implementations
{
    public class DoctorAvailabilityRepository : IDoctorAvailabilityRepository
    {
        private readonly ClinicDbContext _context;

        public DoctorAvailabilityRepository(ClinicDbContext context)
        {
            _context = context;
        }

        public async Task<DoctorAvailability> GetAvailabilityByDoctorId(int doctorId)
        {
            return await _context.DoctorAvailability
                .FirstOrDefaultAsync(da => da.DoctorId == doctorId);
        }

        public async Task<bool> CreateAvailability(DoctorAvailability availability)
        {
            await _context.DoctorAvailability.AddAsync(availability);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAvailability(DoctorAvailability availability)
        {
            _context.DoctorAvailability.Remove(availability);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
