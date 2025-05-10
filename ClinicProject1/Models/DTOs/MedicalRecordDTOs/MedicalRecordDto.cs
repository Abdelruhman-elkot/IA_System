namespace ClinicProject1.Models.DTOs.MedicalRecordDTOs
{
    public class MedicalRecordDto
    {
        public int RecordId { get; set; }
        public string DoctorName { get; set; }
        public string Photo { get; set; }
        public string Specialization { get; set; }
        public string Diagnosis { get; set; }
        public string Prescription { get; set; }
    }
}
