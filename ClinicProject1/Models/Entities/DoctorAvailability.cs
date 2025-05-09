using ClinicProject1.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicProject1.Models.Entities
{
    public class DoctorAvailability
    {
        [Key]
        public int AvailabilityId { get; set; }

        [Required]
        [ForeignKey("DoctorId")]
        public int DoctorId { get; set; }

        [Required]
        public WorkDays Day1 { get; set; }

        [Required]
        public WorkDays Day2 { get; set; }

        [Required]
        public string StartTime { get; set; } = "05:00 PM";

        [Required]
        public string EndTime { get; set; } = "10:00 PM";

        public bool IsAvailable { get; set; } = true;

        public Doctor Doctor { get; set; }
    }
}
