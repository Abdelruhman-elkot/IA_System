using ClinicProject1.Models.DTOs.MedicalRecordDTOs;

namespace ClinicProject1.Services.Interfaces
{
    public interface IMedicalRecordService
    {
        Task<PatientMedicalHistoryDto> GetPatientMedicalHistory(int patientId);
    }
}
