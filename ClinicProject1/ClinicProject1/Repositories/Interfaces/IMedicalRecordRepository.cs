using ClinicProject1.Models.Entities;

namespace ClinicProject1.Repositories.Interfaces
{
    public interface IMedicalRecordRepository
    {
        Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByPatientId(int patientId);
        Task<Patient> GetPatientWithDetails(int patientId);
    }
}
