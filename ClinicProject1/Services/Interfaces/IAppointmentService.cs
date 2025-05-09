using ClinicProject1.Data.Enums;
using ClinicProject1.DTOs.AppointmentDtos;

namespace ClinicProject1.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentDashboardDto>> GetAllAppointments();
        Task<AppointmentDashboardDto> GetAppointmentById(int appointmentId);
        Task<IEnumerable<AppointmentDashboardDto>> GetAppointmentsByPatientId(int patientId);
        Task<IEnumerable<AppointmentDashboardDto>> GetAppointmentsByDoctorId(int doctorId);

        Task<AppointmentDashboardDto> BookAppointment(CreateAppointmentDto appointmentDto);
        Task<AppointmentDashboardDto> CancelAppointment(int appointmentId);
        Task<AppointmentDashboardDto> RescheduleAppointment(int appointmentId, RescheduleAppointmentDto rescheduleDto);

        Task<AppointmentDashboardDto> ApproveAppointment(int appointmentId);
    }
}
