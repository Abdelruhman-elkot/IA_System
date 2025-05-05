using ClinicProject1.Models.Entities;

namespace ClinicProject1.Services.Interfaces
{
    public interface IDoctorAvailabilityRepository
    {
        Task<DoctorAvailability> GetAvailabilityByDoctorId(int doctorId);
        Task<bool> CreateAvailability(DoctorAvailability availability);
        Task<bool> DeleteAvailability(DoctorAvailability availability);
    }
}
