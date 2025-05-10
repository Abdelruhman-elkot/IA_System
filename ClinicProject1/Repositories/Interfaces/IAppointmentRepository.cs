using ClinicProject1.Models.Entities;
using ClinicProject1.Models.Enums;

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
