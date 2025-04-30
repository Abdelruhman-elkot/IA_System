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
        Task<Doctor> CreateDoctor(CreateDoctorDto doctorDto);
        Task<bool> UpdateDoctor(int doctorId, UpdateDoctorDto doctorDto);
        Task<bool> DeleteDoctor(int doctorId);

        // Doctor Availability Management
        //Task<bool> AddDoctorAvailability(int doctorId, CreateAvailabilityDto availabilityDto);
        //Task<bool> UpdateDoctorAvailability(int availabilityId, UpdateAvailabilityDto availabilityDto);
        //Task<bool> RemoveDoctorAvailability(int availabilityId);

        // Appointment Management
        //Task<IEnumerable<AppointmentDashboardDto>> GetAllAppointments();
        //Task<bool> ApproveAppointment(int appointmentId);
        //Task<bool> RescheduleAppointment(int appoinmtmentId, RescheduleAppointmentDto dto);
        //Task<bool> CancelAppointment(int appoinmentId);

        //// Report Generation
        //Task<ReportDto> GenerateDoctorScheduleReport();
        //Task<ReportDto> GeneratePatientVisitsReport(DateTime? fromDate, DateTime? toDate);
        //Task<IEnumerable<ReportDto>> GetAllReports();
        //Task<ReportDto> GetReportById(int id);
    }
}
