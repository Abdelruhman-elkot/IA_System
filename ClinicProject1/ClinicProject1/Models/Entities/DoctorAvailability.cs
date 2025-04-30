using ClinicProject1.Data.Enums;
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
        public string StartTime { get; set; } = "05:00";

        [Required]
        public string EndTime { get; set; } = "10:00";

        public bool IsAvailable { get; set; } = true;

        public Doctor Doctor { get; set; }
    }
}
