using ClinicProject1.Data;
using ClinicProject1.Models.Entities;
using ClinicProject1.Repositories.Interfaces;
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

        public async Task<DoctorAvailability> GetDoctorAvailability(int doctorId)
        {
            return await _context.DoctorAvailability
                .FirstOrDefaultAsync(da => da.DoctorId == doctorId);
        }
    }
}
