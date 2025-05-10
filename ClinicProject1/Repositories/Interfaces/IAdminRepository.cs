using ClinicProject1.Models.Entities;

namespace ClinicProject1.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        // Doctor Management CRUD Operations
        Task<IEnumerable<Doctor>> GetAllDoctors();
        Task<Doctor> GetDoctorById(int doctorId);
        Task<Doctor> CreateDoctor(Doctor doctor);
        Task<bool> UpdateDoctor(Doctor doctor);
        Task<bool> DeleteDoctor(int doctorId);

        Task<IEnumerable<Doctor>> GetDoctorSchedulesReport();
        Task<IEnumerable<Patient>> GetPatientVisitsReport();
    }
}
