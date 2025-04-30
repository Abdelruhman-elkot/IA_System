using ClinicProject1.Data.Enums;
using ClinicProject1.DTOs.AppointmentDtos;
using ClinicProject1.Models.Entities;

namespace ClinicProject1.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Appointment> GetAppointmentById(int appointmentId);
        Task<IEnumerable<Appointment>> GetAllAppointments();
        Task<bool> AddAppointment(Appointment appointment);
        Task<bool> UpdateAppointment(Appointment appointment);
        Task<bool> DeleteAppointment(int appointmentId);
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientId(int patientId);
        Task<IEnumerable<Appointment>> GetAppointmentsByDoctorId(int doctorId);
        Task<bool> IsTimeSlotAvailable(int doctorId, WorkDays day, AppointmentTimes time);
    }
}
