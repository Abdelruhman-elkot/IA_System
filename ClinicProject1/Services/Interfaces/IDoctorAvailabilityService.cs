using ClinicProject1.DTOs.AvailabilityDTOs;

namespace ClinicProject1.Services.Interfaces
{
    public interface IDoctorAvailabilityService
    {
        Task<DoctorAvailabilityDto> GetAvailability(int doctorId);
        Task<bool> AssignAvailability(int doctorId, AssignAvailabilityDto dto);
    }
}
