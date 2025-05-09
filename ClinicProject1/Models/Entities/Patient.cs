using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ClinicProject1.Models.Enums;

namespace ClinicProject1.Models.Entities
{
    public class Patient 
    {
        [Key]
        [ForeignKey("UserId")]
        public int PatientId { get; set; }

        [Required]
        [Range(0, 100)]
        public int Age { get; set; }

        [Required]
        public Gender Gender { get; internal set; }

        public string? ChronicDiseases { get; set; }

        public string? MedicalComplaint { get; set; }

        public  User User { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}
