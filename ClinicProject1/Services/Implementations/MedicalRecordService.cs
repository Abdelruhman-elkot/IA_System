using AutoMapper;
using ClinicProject1.Models.DTOs.MedicalRecordDTOs;
using ClinicProject1.Repositories.Interfaces;
using ClinicProject1.Services.Interfaces;

namespace ClinicProject1.Services.Implementations
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IMapper _mapper;

        public MedicalRecordService(IMedicalRecordRepository medicalRecordRepository, IMapper mapper)
        {
            _medicalRecordRepository = medicalRecordRepository;
            _mapper = mapper;
        }

        public async Task<PatientMedicalHistoryDto> GetPatientMedicalHistory(int patientId)
        {
            var patient = await _medicalRecordRepository.GetPatientWithDetails(patientId);
            var medicalRecords = await _medicalRecordRepository.GetMedicalRecordsByPatientId(patientId);

            var historyDto = new PatientMedicalHistoryDto
            {
                PatientName = $"{patient.User.FirstName} {patient.User.LastName}",
                Age = patient.Age,
                Gender = patient.Gender.ToString(),
                ChronicDiseases = patient.ChronicDiseases
            };

            foreach (var record in medicalRecords)
            {
                var recordDto = _mapper.Map<MedicalRecordDto>(record);
                recordDto.DoctorName = $"{record.Doctor.User.FirstName} {record.Doctor.User.LastName}";
                recordDto.Specialization = record.Doctor.Specialization.ToString();
                recordDto.Photo = record.Doctor.User.Photo;
                historyDto.MedicalRecords.Add(recordDto);
            }

            return historyDto;
        }
    }
}
