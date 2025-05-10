using ClinicProject1.Models.DTOs.AppointmentDtos;
using ClinicProject1.Models.Enums;

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

        Task<List<AppointmentTimes>> GetAvailableTimeSlots(int doctorId, WorkDays day);
    }
}
