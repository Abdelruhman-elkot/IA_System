namespace ClinicProject1.Models.DTOs.MedicalRecordDTOs
{
    public class PatientMedicalHistoryDto
    {
        public string PatientName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string ChronicDiseases { get; set; }
        public List<MedicalRecordDto> MedicalRecords { get; set; } = new List<MedicalRecordDto>();
    }
}
