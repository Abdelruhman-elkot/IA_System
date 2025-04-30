using ClinicProject1.Models.Entities;

namespace ClinicProject1.Repositories.Interfaces
{
    public interface IDoctorAvailabilityRepository
    {
        Task<DoctorAvailability> GetDoctorAvailability(int doctorId);
    }
}
