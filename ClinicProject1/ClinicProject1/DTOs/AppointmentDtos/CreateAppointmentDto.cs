using ClinicProject1.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClinicProject1.DTOs.AppointmentDtos
{
    public class CreateAppointmentDto
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public WorkDays AppointmentDay { get; set; }

        [Required]
        public AppointmentTimes Time { get; set; }
    }
}
