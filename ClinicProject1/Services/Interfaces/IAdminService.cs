using ClinicProject1.Models.DTOs.DoctorDTOs;
using ClinicProject1.Models.DTOs.ReportsDTOs;

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

        Task<IEnumerable<DoctorScheduleReportDto>> GenerateDoctorSchedulesReport();
        Task<IEnumerable<PatientVisitReportDto>> GeneratePatientVisitsReport();
    }
}
