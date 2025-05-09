using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ClinicProject1.Models.Enums;

namespace ClinicProject1.Models.Entities
{
    public class Doctor
    {
        [Key]
        [ForeignKey("UserId")]
        public int DoctorId { get; set; }

        [Required]
        public Specialization Specialization { get; set; }

        public User User { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<MedicalRecord> MedicalRecords { get; set; }
        public ICollection<DoctorAvailability> Availabilities { get; set; }
    }
}
