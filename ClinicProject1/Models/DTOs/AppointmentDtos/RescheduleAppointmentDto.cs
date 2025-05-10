using ClinicProject1.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClinicProject1.Models.DTOs.AppointmentDtos
{
    public class RescheduleAppointmentDto
    {
        [Required]
        public WorkDays NewAppointmentDay { get; set; }

        [Required]
        public AppointmentTimes NewTime { get; set; }
    }
}
