using ClinicProject1.Data.Enums;

namespace ClinicProject1.DTOs.AvailabilityDTOs
{
    public class DoctorAvailabilityDto
    {
        public int DoctorId { get; set; }
        public List<string> AvailableDays { get; set; }
        public string WorkingHours { get; set; }
    }
}
