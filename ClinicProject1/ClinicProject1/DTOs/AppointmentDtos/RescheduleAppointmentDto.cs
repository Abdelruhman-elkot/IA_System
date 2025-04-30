using ClinicProject1.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClinicProject1.DTOs.AppointmentDtos
{
    public class RescheduleAppointmentDto
    {
        [Required]
        public WorkDays NewAppointmentDay { get; set; }

        [Required]
        public AppointmentTimes NewTime { get; set; }
    }
}
