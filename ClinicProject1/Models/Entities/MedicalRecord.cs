using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicProject1.Models.Entities
{
    public class MedicalRecord
    {
        [Key]
        public int RecordId { get; set; }

        [Required]
        [ForeignKey("PatientId")]
        public int PatientId { get; set; }

        [Required]
        [ForeignKey("DoctorId")]
        public int DoctorId { get; set; }

        [StringLength(1000)]
        public string Diagnosis { get; set; }

        [StringLength(1000)]
        public string Prescription { get; set; }

        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
    }
}
