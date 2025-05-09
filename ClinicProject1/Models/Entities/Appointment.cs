using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ClinicProject1.Models.Enums;

namespace ClinicProject1.Models.Entities
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required]
        [ForeignKey("PatientId")]
        public int PatientId { get; set; }

        [Required]
        [ForeignKey("DoctorId")]
        public int DoctorId { get; set; }

        [Required]
        public WorkDays AppointmentDay { get; set; }

        [Required]
        public AppointmentTimes Time { get; set; }

        [Required]
        public AppointmentStatus Status { get; set; }

        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
    }
}
