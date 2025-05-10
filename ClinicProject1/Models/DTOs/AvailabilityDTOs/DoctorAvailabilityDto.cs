using ClinicProject1.Models.Enums;

namespace ClinicProject1.Models.DTOs.AvailabilityDTOs
{
    public class DoctorAvailabilityDto
    {
        public int DoctorId { get; set; }
        public List<String> AvailableDays { get; set; }
        public string WorkingHours { get; set; }
        public bool IsAvailable { get; set; }
        public Dictionary<string, List<string>> AvailableTimeSlots { get; set; }
    }
}
