using ClinicProject1.DTOs.AvailabilityDTOs;
using ClinicProject1.DTOs.Doctor_DTOs;
using ClinicProject1.DTOs.DoctorDTOs;
using ClinicProject1.Models.Entities;

namespace ClinicProject1.Services.Interfaces
{
    public interface IAdminService
    {
        // Doctor Management
        Task<IEnumerable<DoctorDashboardDto>> GetAllDoctors();
        Task<DoctorDashboardDto> GetDoctorById(int doctorId);
        Task<DoctorDashboardDto> CreateDoctor(CreateDoctorDto doctorDto);
        Task<bool> UpdateDoctor(int doctorId, UpdateDoctorDto doctorDto);
        Task<bool> DeleteDoctor(int doctorId);
    }
}
